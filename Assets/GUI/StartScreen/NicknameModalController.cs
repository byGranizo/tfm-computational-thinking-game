using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NicknameModalController : MonoBehaviour
{
    private GUIController guiController;

    private UIDocument doc;
    private VisualElement root;

    private Button submitButton;
    private TextField nicknameField;

    private void Awake()
    {
        guiController = GetComponentInParent<GUIController>();

        doc = GetComponent<UIDocument>();

        root = doc.rootVisualElement;

        nicknameField = root.Q<TextField>("NicknameField");

        submitButton = root.Q<Button>("SubmitButton");
        submitButton.clicked += SubmitButtonOnClicked;
    }

    private async void SubmitButtonOnClicked()
    {
        Debug.Log("Submit button clicked");
        await GameState.Instance.SubmitNickname(nicknameField.text);
        HideUI();
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
