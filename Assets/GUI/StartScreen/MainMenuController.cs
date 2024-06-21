using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class MainMenuController : MonoBehaviour
{
    private GUIController guiController;

    private UIDocument doc;

    private VisualElement root;

    private Button startButton;
    private Button exitButton;
    private Button rulesButton;
    private Button controlsButton;
    private Button debugButton;

    void OnEnable()
    {
        guiController = GetComponentInParent<GUIController>();

        doc = GetComponent<UIDocument>();

        root = doc.rootVisualElement;

        startButton = root.Q<Button>("StartButton");
        startButton.clicked += StartButtonOnClicked;

        exitButton = root.Q<Button>("ExitButton");
        exitButton.clicked += ExitButtonOnClicked;

        rulesButton = root.Q<Button>("RulesButton");
        rulesButton.clicked += OnClickRules;

        controlsButton = root.Q<Button>("ControlsButton");
        controlsButton.clicked += OnClickControls;

        debugButton = root.Q<Button>("DebugButton");
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
