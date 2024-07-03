using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("csharpsquid", "S101:Types should be named in PascalCase", Justification = "Legacy naming convention")]
public class GUIGameController : MonoBehaviour
{
    private GameManager gameManager;
    private InGameUIController inGameUIController;
    private EndGameUIController endGameUIController;
    private ActiveMissionSelectUIController activeMissionSelectUIController;

    private CommonUIController commonUIController;

    [SerializeField]
    private Texture2D cardBackEasy;
    [SerializeField]
    private Texture2D cardBackMedium;
    [SerializeField]
    private Texture2D cardBackHard;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        inGameUIController = GetComponentInChildren<InGameUIController>();
        endGameUIController = GetComponentInChildren<EndGameUIController>();
        activeMissionSelectUIController = GetComponentInChildren<ActiveMissionSelectUIController>();

        commonUIController = FindObjectOfType<CommonUIController>();
    }

    public GameManager GameManager
    {
        get { return gameManager; }
    }

    void Start()
    {
        endGameUIController.HideUI();
        activeMissionSelectUIController.HideUI();
    }


    public void ShowActiveMissionSelectUIAsync()
    {
        inGameUIController.HideUI();
        StartCoroutine(ShowActiveMissionSelectUIAsyncCoroutine());
    }

    private IEnumerator ShowActiveMissionSelectUIAsyncCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        activeMissionSelectUIController.ShowUI();
    }

    public void HideActiveMissionSelectUI()
    {
        activeMissionSelectUIController.HideUI();
        inGameUIController.ShowUI();
    }

    public void ShowEndGameUI()
    {
        inGameUIController.HideUI();
        endGameUIController.ShowUI();
    }

    public void ShowEndGameUI(EndGameUIState state)
    {
        inGameUIController.HideUI();
        endGameUIController.ShowUI(state);
    }

    public void RefreshCardUI(int remainingCards)
    {
        for (int i = 0; i < gameManager.ActiveCards.Length; i++)
        {
            if (gameManager.ActiveCards[i] == null)
            {
                Texture2D texture = null;
                inGameUIController.RefreshCardUI(texture, i);
                activeMissionSelectUIController.RefreshCardUI(texture, i);
            }
            else
            {
                Texture2D texture = gameManager.ActiveCards[i].Texture;
                inGameUIController.RefreshCardUI(texture, i);
                activeMissionSelectUIController.RefreshCardUI(texture, i);
            }

        }

        inGameUIController.ChangeNextCardNumber(remainingCards);
    }

    public void RefreshTileUI(int remainingRiverTiles, int remainingWoodTiles, int remainingTownTiles)
    {
        inGameUIController.ChangeNextTileNumber(remainingRiverTiles, remainingWoodTiles, remainingTownTiles);
    }

    public void RefreshMissionsUI(int completedEasyMissions, int completedMediumMissions, int completedHardMissions)
    {
        inGameUIController.ChangeCompletedMissions(completedEasyMissions, completedMediumMissions, completedHardMissions);
    }


    public void ChangeNextCardDifficulty(CardMissionDifficulty nextCardDifficulty)
    {
        Texture2D cardBackTexture = null;

        switch (nextCardDifficulty)
        {
            case CardMissionDifficulty.Easy:
                cardBackTexture = cardBackEasy;
                break;
            case CardMissionDifficulty.Medium:
                cardBackTexture = cardBackMedium;
                break;
            case CardMissionDifficulty.Hard:
                cardBackTexture = cardBackHard;
                break;
        }

        inGameUIController.ChangeNextCardTexture(cardBackTexture);
    }

    public void OnCardButtonClicked(int index)
    {
        HideActiveMissionSelectUI();
        gameManager.CompleteMissionWithWildcard(index);
    }

    public void OnTileButtonClicked(BiomeType biomeType)
    {
        gameManager.NewTileUI(biomeType);

    }

    public void OpenControls()
    {
        commonUIController.ShowControlsUI();
    }

    public void OpenRules()
    {
        commonUIController.ShowRulesUI();
    }

    public void UpdateTimer(int timeLeft)
    {
        inGameUIController.UpdateTimer(timeLeft);
    }
}
