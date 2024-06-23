using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlsUIController : MonoBehaviour
{
    VisualElement root;

    Label controlsLabel;

    [SerializeField]
    private TextAsset controlsText;

    void Awake()
    {
        CommonUIController commonUIController = GetComponentInParent<CommonUIController>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        Button closeButton = root.Q<Button>("CloseButton");
        closeButton.clicked += commonUIController.HideControlsUI;

        VisualElement popupContent = root.Q<VisualElement>("Content");
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
