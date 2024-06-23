using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUIController : MonoBehaviour
{
    GUIGameController guiGameController;

    UIDocument uiDocument;
    VisualElement root;
    Button newCardButton;
    readonly Button[] cardsButtons = new Button[3];

    readonly Button[] tileButtons = new Button[3];

    readonly Label[] completedMissionsLabels = new Label[3];

    Label timerLabel;

    void Start()
    {
        guiGameController = GetComponentInParent<GUIGameController>();

        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        newCardButton = root.Q<Button>("NewCard");

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            cardsButtons[index] = root.Q<Button>("Card_" + index);
            cardsButtons[index].clicked += () => OnCardButtonClicked(index);

            completedMissionsLabels[index] = root.Q<VisualElement>("MissionType_" + index).Q<Label>();
        }

        tileButtons[0] = root.Q<Button>("NewTileBlue");
        tileButtons[0].clicked += () => OnTileButtonClicked(BiomeType.River);

        tileButtons[1] = root.Q<Button>("NewTileGreen");
        tileButtons[1].clicked += () => OnTileButtonClicked(BiomeType.Wood);

        tileButtons[2] = root.Q<Button>("NewTileRed");
        tileButtons[2].clicked += () => OnTileButtonClicked(BiomeType.Town);

        timerLabel = root.Q<Label>("Timer");
    }

    public void RefreshCardUI(Texture2D cardTexture, int index)
    {
        cardsButtons[index].style.backgroundImage = new StyleBackground(cardTexture);

    }

    public void ChangeNextCardTexture(Texture2D cardBackTexture)
    {
        newCardButton.style.backgroundImage = new StyleBackground(cardBackTexture);
    }

    private void OnCardButtonClicked(int index)
    {
        guiGameController.OnCardButtonClicked(index);
    }

    private void OnTileButtonClicked(BiomeType biomeType)
    {
        guiGameController.OnTileButtonClicked(biomeType);
    }

    public void ChangeNextCardNumber(int cardNumber)
    {
        newCardButton.text = cardNumber.ToString();
    }

    public void ChangeNextTileNumber(int remainingRiverTiles, int remainingWoodTiles, int remainingTownTiles)
    {
        tileButtons[0].text = remainingRiverTiles.ToString();
        tileButtons[1].text = remainingWoodTiles.ToString();
        tileButtons[2].text = remainingTownTiles.ToString();
    }

    public void ChangeCompletedMissions(int completedEasyMissions, int completedMediumMissions, int completedHardMissions)
    {
        completedMissionsLabels[0].text = completedEasyMissions.ToString();
        completedMissionsLabels[1].text = completedMediumMissions.ToString();
        completedMissionsLabels[2].text = completedHardMissions.ToString();
    }

    public void UpdateTimer(int timeLeft)
    {
        timerLabel.text = timeLeft.ToString();
    }

    public void ShowUI()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void HideUI()
    {
        root.style.display = DisplayStyle.None;
    }


}
