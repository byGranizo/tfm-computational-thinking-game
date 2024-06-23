using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField]
    private Tile tilePrefab;
    [SerializeField]
    private TileType initialTileType;

    [SerializeField]
    private List<TileType> tileRiverTypes;
    [SerializeField]
    private List<TileType> tileWoodTypes;
    [SerializeField]
    private List<TileType> tileTownTypes;

    [Space(10)]
    [Header("Cards")]
    [SerializeField]
    private List<CardType> cardTypes;
    [SerializeField]
    private CardType[] activeCards = new CardType[3];
    private readonly List<CardType> completedCards = new List<CardType>();

    [Space(10)]
    [Header("Turns")]
    [SerializeField]
    private int turnDuration = 30;

    [Space(10)]
    [Header("Debug")]
    [SerializeField]
    private bool secuentialTiles = false;

    private GUIGameController guiGameController;
    private CommonUIController commonUIController;
    private int nextCardIndex = -1;
    private List<(BiomeType, int)> lastModifiedBiomes;
    private int currentTurn = 0;
    private float currentTurnDuration = 0;
    private bool timerRunning = false;
    private bool gameEnded = false;
    private int tilesPlaced = 0;


    public CardType[] ActiveCards
    {
        get
        {
            return activeCards;
        }
    }

    private void Awake()
    {
        guiGameController = FindObjectOfType<GUIGameController>();
        commonUIController = FindObjectOfType<CommonUIController>();

        BoardController boardController = FindObjectOfType<BoardController>();
        boardController.InitBoardGrid();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void InitializeGame()
    {
        SelectNextCard();
        FillCards();
        RefreshCardsUI();
        guiGameController.RefreshTileUI(tileRiverTypes.Count, tileWoodTypes.Count, tileTownTypes.Count);
        GameState.Instance.SubmitStartGame();
    }

    /* TILES */
    public void NewTileUI(BiomeType biomeType)
    {
        lastModifiedBiomes = null;
        List<TileType> tileTypes = GetTileTypes(biomeType);
        InstantiateGameTile(tileTypes);
        guiGameController.RefreshTileUI(tileRiverTypes.Count, tileWoodTypes.Count, tileTownTypes.Count);
    }

    private List<TileType> GetTileTypes(BiomeType biomeType)
    {
        return biomeType switch
        {
            BiomeType.River => tileRiverTypes,
            BiomeType.Wood => tileWoodTypes,
            BiomeType.Town => tileTownTypes,
            _ => new List<TileType>()
        };
    }

    private void InstantiateGameTile(List<TileType> tileTypes)
    {
        if (tileTypes.Count == 0)
        {
            Debug.Log("No more tiles to instantiate");
            return;
        }

        Tile tile = Instantiate<Tile>(tilePrefab, Vector3.zero, tilePrefab.gameObject.transform.rotation);

        int index = !secuentialTiles ? Random.Range(0, tileTypes.Count) : 0;

        TileType tileType = tileTypes[index];
        tileTypes.RemoveAt(index);

        tile.SetTileType(tileType);
        tile.Grab();

        tilesPlaced++;
    }

    public void InstantiateInitialTile(HexagonalCell cell)
    {
        Tile tile = Instantiate<Tile>(tilePrefab, cell.transform);

        tile.SetTileType(initialTileType);

        cell.PlaceTile(tile);

        tilesPlaced++;

        StartTurn();
    }

    /* CARDS */
    private bool FillCards()
    {
        bool cardPlaced = false;
        for (int i = 0; i < activeCards.Length; i++)
        {
            if (activeCards[i] == null && nextCardIndex != -1)
            {
                CardType cardType = cardTypes[nextCardIndex];

                cardType.TurnStart = currentTurn;

                activeCards[i] = cardType;
                cardPlaced = true;

                cardTypes.RemoveAt(nextCardIndex);
                if (cardTypes.Count == 0)
                {
                    nextCardIndex = -1;
                }
                else
                {
                    SelectNextCard();
                }
            }
        }

        if (!cardPlaced)
        {
            Debug.Log("No more space for cards");
        }

        return cardPlaced;
    }

    private void SelectNextCard()
    {
        nextCardIndex = cardTypes.Count == 0 ? -1 : Random.Range(0, cardTypes.Count);
    }

    private void RefreshCardsUI()
    {
        if (nextCardIndex == -1)
        {
            guiGameController.ChangeNextCardDifficulty(CardMissionDifficulty.None);
            guiGameController.RefreshCardUI(cardTypes.Count);
            return;
        }

        CardType nextCardType = cardTypes[nextCardIndex];
        guiGameController.ChangeNextCardDifficulty(nextCardType?.CardMissionDifficulty ?? CardMissionDifficulty.None);
        guiGameController.RefreshCardUI(cardTypes.Count);
    }


    /* MISSIONS CHECK */
    public void CheckMissionsAtPlace(List<(BiomeType, int)> modifiedBiomes)
    {
        EndTurn();
        lastModifiedBiomes = modifiedBiomes;
        CheckMissions();
        if (!CheckWildcard() && !gameEnded) StartTurn();
    }

    private bool CheckWildcard()
    {
        int wildcardIndex = lastModifiedBiomes.FindIndex(biome => biome.Item1 == BiomeType.Wildcard);
        if (wildcardIndex == -1) return false;

        if (lastModifiedBiomes[wildcardIndex].Item2 <= 1)
        {
            lastModifiedBiomes.RemoveAt(wildcardIndex);
        }
        else
        {
            lastModifiedBiomes[wildcardIndex] = (BiomeType.Wildcard, lastModifiedBiomes[wildcardIndex].Item2 - 1);
        }

        guiGameController.ShowActiveMissionSelectUIAsync();
        return true;
    }

    public void CompleteMissionWithWildcard(int missionIndex)
    {
        CompleteMission(activeCards[missionIndex], true);
        activeCards[missionIndex] = null;
        HandleNewCards();
    }

    private void CompleteMission(CardType cardType, bool withWildcard)
    {
        cardType.TurnEnd = currentTurn;
        cardType.CompletedWithWildCard = withWildcard;
        GameState.Instance.SubmitCompletedMission(cardType);
        completedCards.Add(cardType);
    }

    private void HandleNewCards()
    {
        bool newCardsInstantiated = FillCards();
        RefreshCardsUI();
        RefreshMissionsUI();
        CheckGameEnded();
        if (!newCardsInstantiated) return;
        CheckMissions();
        if (!CheckWildcard() && !gameEnded) StartTurn();
    }

    private void CheckMissions()
    {
        for (int i = 0; i < activeCards.Length; i++)
        {
            if (activeCards[i] == null) continue;
            if (IsMissionCompleted(activeCards[i]))
            {
                CompleteMission(activeCards[i], false);
                activeCards[i] = null;
            }
        }
        HandleNewCards();
    }

    private bool IsMissionCompleted(CardType cardType)
    {
        foreach (var (biomeType, count) in lastModifiedBiomes)
        {
            if (cardType.Biome != biomeType) continue;
            if ((cardType.MissionType == CardMissionType.EqualsOrGreater && cardType.Value <= count) ||
                (cardType.MissionType == CardMissionType.Equals && cardType.Value == count))
            {
                return true;
            }
        }
        return false;
    }

    private void RefreshMissionsUI()
    {
        int completedEasyMissions = 0;
        int completedMediumMissions = 0;
        int completedHardMissions = 0;

        completedCards.ForEach(card =>
        {
            switch (card.CardMissionDifficulty)
            {
                case CardMissionDifficulty.Easy: completedEasyMissions++; break;
                case CardMissionDifficulty.Medium: completedMediumMissions++; break;
                case CardMissionDifficulty.Hard: completedHardMissions++; break;
            }
        });

        guiGameController.RefreshMissionsUI(completedEasyMissions, completedMediumMissions, completedHardMissions);
    }

    private void CheckGameEnded()
    {
        if (tileRiverTypes.Count == 0 && tileWoodTypes.Count == 0 && tileTownTypes.Count == 0)
        {
            LoseGame();
        }

        if (cardTypes.Count == 0 && activeCards[0] == null && activeCards[1] == null && activeCards[2] == null)
        {
            WinGame();
        }


    }

    private void WinGame()
    {
        gameEnded = true;
        guiGameController.ShowEndGameUI(EndGameUIState.Win);
        GameState.Instance.SubmitEndGame(currentTurn, tilesPlaced, completedCards.Count, EndGameUIState.Win);
    }

    private void LoseGame()
    {
        gameEnded = true;
        guiGameController.ShowEndGameUI(EndGameUIState.Lose);
        GameState.Instance.SubmitEndGame(currentTurn, tilesPlaced, completedCards.Count, EndGameUIState.Lose);
    }


    public bool IsCameraMovementAllowed()
    {
        return !commonUIController.CommonUIVisible;
    }

    private void UpdateTimer()
    {
        if (!timerRunning) return;

        currentTurnDuration += Time.deltaTime;
        UpdataTimerOnGUI();

        if (currentTurnDuration >= turnDuration)
        {
            EndTurn();
            LoseGame();
        }
    }

    private void UpdataTimerOnGUI()
    {
        guiGameController.UpdateTimer(turnDuration - (int)currentTurnDuration);
    }

    public void EndTurn()
    {
        timerRunning = false;

        TurnType turnType = new TurnType(currentTurn, currentTurnDuration);
        GameState.Instance.SubmitTurn(turnType);
    }

    public void StartTurn()
    {
        currentTurn++;
        currentTurnDuration = 0;
        timerRunning = true;
    }
}