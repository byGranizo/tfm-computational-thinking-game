using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndGameUIController : MonoBehaviour
{
    UIDocument uiDocument;
    VisualElement root;

    Button goToMenuButton;

    Label resultLabel;

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        goToMenuButton = root.Q<Button>("GoToMenuButton");
        goToMenuButton.clicked += GoToMenu;

        resultLabel = root.Q<Label>("GameResult");


    }

    void GoToMenu()
    {
        CustomSceneManager.LoadMainMenuScene();
    }

    public void ShowUI(EndGameUIState state)
    {
        switch (state)
        {
            case EndGameUIState.Win:
                resultLabel.text = "Has ganado";
                break;
            case EndGameUIState.Lose:
                resultLabel.text = "Has perdido";
                break;
        }

        ShowUI();
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

public enum EndGameUIState
{
    Win,
    Lose
}
