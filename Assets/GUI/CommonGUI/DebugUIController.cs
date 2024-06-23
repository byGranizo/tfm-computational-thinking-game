using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugUIController : MonoBehaviour
{
    VisualElement root;

    CommonUIController commonUIController;

    // Start is called before the first frame update
    void Awake()
    {
        commonUIController = GetComponentInParent<CommonUIController>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        Button closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += commonUIController.HideDebugUI;

        Button startGameButton = root.Q<Button>("StartGameButton");
        startGameButton.clicked += StartGameButtonOnClicked;

        Button endGameButton = root.Q<Button>("EndGameButton");
        endGameButton.clicked += EndGameButtonOnClicked;

        Button turnButton = root.Q<Button>("TurnButton");
        turnButton.clicked += TurnButtonOnClicked;

        Button missionButton = root.Q<Button>("MissionButton");
        missionButton.clicked += MissionButtonOnClicked;
    }

    public void ShowUI()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void HideUI()
    {
        root.style.display = DisplayStyle.None;
    }

    private void StartGameButtonOnClicked()
    {
        GameState.Instance.SubmitStartGame();
        Debug.Log("Start game button clicked");
    }

    private void EndGameButtonOnClicked()
    {
        GameState.Instance.SubmitEndGame(5, 15, 3, EndGameUIState.Win);
        Debug.Log("End game button clicked");
    }

    private void TurnButtonOnClicked()
    {
        TurnType turnType = new TurnType(5, 15.8f);
        GameState.Instance.SubmitTurn(turnType);
        Debug.Log("Turn button clicked");

    }

    private void MissionButtonOnClicked()
    {
        CardType cardType = ScriptableObject.CreateInstance<CardType>();
        GameState.Instance.SubmitCompletedMission(cardType);
        Debug.Log("Mission button clicked");
    }

}
