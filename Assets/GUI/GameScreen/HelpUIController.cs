using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelpUIController : MonoBehaviour
{
    GUIGameController guiGameController;

    void Start()
    {
        guiGameController = GetComponentInParent<GUIGameController>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        Button openRulesButton = root.Q<Button>("OpenRulesButton");
        openRulesButton.clicked += OnClickRules;

        Button openControlsButton = root.Q<Button>("OpenControlsButton");
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
