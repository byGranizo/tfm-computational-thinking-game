using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndGameUIController : MonoBehaviour
{
    VisualElement root;

    Label resultLabel;

    void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        Button goToMenuButton = root.Q<Button>("GoToMenuButton");
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
