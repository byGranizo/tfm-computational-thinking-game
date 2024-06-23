using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class MainMenuController : MonoBehaviour
{
    private GUIController guiController;

    private VisualElement root;

    void OnEnable()
    {
        guiController = GetComponentInParent<GUIController>();

        UIDocument doc = GetComponent<UIDocument>();

        root = doc.rootVisualElement;

        Button startButton = root.Q<Button>("StartButton");
        startButton.clicked += StartButtonOnClicked;

        Button exitButton = root.Q<Button>("ExitButton");
        exitButton.clicked += ExitButtonOnClicked;

        Button rulesButton = root.Q<Button>("RulesButton");
        rulesButton.clicked += OnClickRules;

        Button controlsButton = root.Q<Button>("ControlsButton");
        controlsButton.clicked += OnClickControls;

        Button debugButton = root.Q<Button>("DebugButton");
        debugButton.clicked += OnClickDebug;

    }

    private void StartButtonOnClicked()
    {
        Debug.Log("Create button clicked");
        CustomSceneManager.LoadGameScene();
    }

    private void ExitButtonOnClicked()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    private void OnClickRules()
    {
        guiController.OpenRules();
    }

    private void OnClickControls()
    {
        guiController.OpenControls();
    }

    private void OnClickDebug()
    {
        guiController.OpenDebug();
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
