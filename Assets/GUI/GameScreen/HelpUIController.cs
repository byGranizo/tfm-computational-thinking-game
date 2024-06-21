using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelpUIController : MonoBehaviour
{
    GUIGameController guiGameController;
    UIDocument uiDocument;
    VisualElement root;

    Button openRulesButton;
    Button openControlsButton;

    void Start()
    {
        guiGameController = GetComponentInParent<GUIGameController>();

        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        openRulesButton = root.Q<Button>("OpenRulesButton");
        openRulesButton.clicked += OnClickRules;

        openControlsButton = root.Q<Button>("OpenControlsButton");
        openControlsButton.clicked += OnClickControls;
    }

    public void OnClickRules()
    {
        guiGameController.OpenRules();
    }

    public void OnClickControls()
    {
        guiGameController.OpenControls();
    }

}
