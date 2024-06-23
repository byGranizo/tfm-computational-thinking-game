using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RulesUIController : MonoBehaviour
{
    VisualElement root;

    Label rulesLabel;

    [SerializeField]
    private TextAsset rulesText;


    void Awake()
    {
        CommonUIController commonUIController = GetComponentInParent<CommonUIController>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        Button closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += commonUIController.HideRulesUI;

        VisualElement popupContent = root.Q<VisualElement>("Content");
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
