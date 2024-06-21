using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RulesUIController : MonoBehaviour
{
    UIDocument uiDocument;
    VisualElement root;

    Button closeButton;

    CommonUIController commonUIController;

    VisualElement popupContent;
    Label rulesLabel;

    [SerializeField]
    private TextAsset rulesText;


    void Awake()
    {
        commonUIController = GetComponentInParent<CommonUIController>();

        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += commonUIController.HideRulesUI;

        popupContent = root.Q<VisualElement>("Content");
        rulesLabel = popupContent.Q<Label>("Text");
    }

    void Start()
    {
        rulesLabel.text = rulesText.text;
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
