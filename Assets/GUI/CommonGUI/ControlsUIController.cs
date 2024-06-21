using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlsUIController : MonoBehaviour
{
    UIDocument uiDocument;
    VisualElement root;

    Button closeButton;

    CommonUIController commonUIController;

    VisualElement popupContent;
    Label controlsLabel;

    [SerializeField]
    private TextAsset controlsText;

    void Awake()
    {
        commonUIController = GetComponentInParent<CommonUIController>();

        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += commonUIController.HideControlsUI;

        popupContent = root.Q<VisualElement>("Content");
        controlsLabel = popupContent.Q<Label>("Text");
    }

    void Start()
    {
        controlsLabel.text = controlsText.text;
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
