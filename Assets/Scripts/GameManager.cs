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
    private List<CardType> completedCards = new List<CardType>();

    [Space(10)]
    [Header("Turns")]
    [SerializeField]
    private int turnDuration = 30;

    [Space(10)]
    [Header("Debug")]
    [SerializeField]
    private bool secuentialTiles = false;

    private BoardController boardController;
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

        boardController = FindObjectOfType<BoardController>();
        boardController.InitBoardGrid();


    }

    private void Start()
    {
        SelectNextCard();
        FillCards();
        RefreshCardsUI();
        guiGameController.RefreshTileUI(tileRiverTypes.Count, tileWoodTypes.Count, tileTownTypes.Count);

        GameState.Instance.SubmitStartGame();
    }

    private void Update()
    {
        UpdateTimer();
    }

    /* TILES */
    public void NewTileUI(BiomeType biomeType)
    {
        lastModifiedBiomes = null;

        List<TileType> tileTypes = new List<TileType>();
        switch (biomeType)
        {
            case BiomeType.River:
                tileTypes = tileRiverTypes;
                break;
            case BiomeType.Wood:
                tileTypes = tileWoodTypes;
                break;
            case BiomeType.Town:
                tileTypes = tileTownTypes;
                break;
        }
        InstantiateGameTile(tileTypes);

        guiGameController.RefreshTileUI(tileRiverTypes.Count, tileWoodTypes.Count, tileTownTypes.Count);


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
                    continue;
                }

                SelectNextCard();
            }
        }

        if (!cardPlaced)
        {
            Debug.Log("No more space for cards");
            return false;
        }

        return true;
    }

    private void SelectNextCard()
    {
        if (cardTypes.Count == 0)
        {
            Debug.Log("No more cards to instantiate");
            nextCardIndex = -1;
        }
        else
        {
            int index = Random.Range(0, cardTypes.Count);
            nextCardIndex = index;
        }
    }

    private void RefreshCardsUI()
    {
        if (nextCardIndex == -1)
        {
            guiGameController.ChangeNextCardDifficulty(CardMissionDifficulty.None);
            guiGameController.RefreshCardUI(cardTypes.Count);
            return;
        };

        CardType nextCardType = cardTypes[nextCardIndex];

        CardMissionDifficulty nextCardDifficulty = nextCardType != null ? nextCardType.CardMissionDifficulty : CardMissionDifficulty.None;

        //TODO: Mix both methods
        guiGameController.ChangeNextCardDifficulty(nextCardDifficulty);
        guiGameController.RefreshCardUI(cardTypes.Count);
    }


    /* MISSIONS CHECK */
    public void CheckMissionsAtPlace(List<(BiomeType, int)> modifiedBiomes)
    {
        EndTurn();
        string biomes = "";
        modifiedBiomes.ForEach(biome => biomes += biome.Item1 + " " + biome.Item2 + " | ");
        Debug.Log(biomes);

        lastModifiedBiomes = modifiedBiomes;
        CheckMissions();
        bool wildcardCompleted = CheckWildcard();

        if (!wildcardCompleted && !gameEnded) StartTurn();
    }

    private bool CheckWildcard()
    {
        int wildcardIndex = -1;

        for (int i = 0; i < lastModifiedBiomes.Count; i++)
        {
            if (lastModifiedBiomes[i].Item1 == BiomeType.Wildcard)
            {
                wildcardIndex = i;
                break;
            }
        }

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
        CardType cardType = activeCards[missionIndex];

        cardType.TurnEnd = currentTurn;
        cardType.CompletedWithWildCard = true;

        GameState.Instance.SubmitCompletedMission(cardType);

        completedCards.Add(cardType);
        activeCards[missionIndex] = null;

        bool newCardsInstantiated = FillCards();

        RefreshCardsUI();
        RefreshMissionsUI();
        CheckGameEnded();

        if (!newCardsInstantiated) return;

        CheckMissions();
        bool wildcardCompleted = CheckWildcard();

        if (!wildcardCompleted && !gameEnded) StartTurn();
    }

    private void CheckMissions()
    {
        for (int i = 0; i < activeCards.Length; i++)
        {
            if (activeCards[i] == null) continue;

            CardType cardType = activeCards[i];

            bool missionCompleted = false;
            for (int j = 0; j < lastModifiedBiomes.Count; j++)
            {
                if (cardType.Biome != lastModifiedBiomes[j].Item1) continue;

                if (cardType.MissionType == CardMissionType.EqualsOrGreater && cardType.Value <= lastModifiedBiomes[j].Item2)
                {
                    missionCompleted = true;
                    break;
                }
                if (cardType.MissionType == CardMissionType.Equals && cardType.Value == lastModifiedBiomes[j].Item2)
                {
                    missionCompleted = true;
                    break;
                }
            }

            if (missionCompleted)
            {
                cardType.TurnEnd = currentTurn;
                completedCards.Add(cardType);

                GameState.Instance.SubmitCompletedMission(cardType);

                activeCards[i] = null;
            }
        }
        bool newCardsInstantiated = FillCards();

        RefreshCardsUI();
        RefreshMissionsUI();
        CheckGameEnded();

        if (!newCardsInstantiated) return;

        CheckMissions();
    }

    private void RefreshMissionsUI()
    {
        int completedEasyMissions = 0;
        int completedMediumMissions = 0;
        int completedHardMissions = 0;

        completedCards.ForEach(card =>
        {
            if (card.CardMissionDifficulty == CardMissionDifficulty.Easy) completedEasyMissions++;
            if (card.CardMissionDifficulty == CardMissionDifficulty.Medium) completedMediumMissions++;
            if (card.CardMissionDifficulty == CardMissionDifficulty.Hard) completedHardMissions++;
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
        if (timerRunning)
        {
            currentTurnDuration += Time.deltaTime;
            UpdataTimerOnGUI();

            if (currentTurnDuration >= turnDuration)
            {
                EndTurn();
                LoseGame();
            }
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