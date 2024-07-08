using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("csharpsquid", "S101:Types should be named in PascalCase", Justification = "Legacy naming convention")]
public class GUIController : MonoBehaviour
{
    private MainMenuController mainMenu;
    private NicknameModalController nickNameModal;

    private CommonUIController commonUIController;

    void Awake()
    {
        mainMenu = GetComponentInChildren<MainMenuController>();
        nickNameModal = GetComponentInChildren<NicknameModalController>();

        commonUIController = FindObjectOfType<CommonUIController>();
    }

    void Start()
    {
        FirebaseUser user = LocalStorage.GetUser();
        bool isUserEmpty = user == null || string.IsNullOrEmpty(user.idToken);

        if (!isUserEmpty)
        {
            nickNameModal.HideUI();
        }

    }

    public void GoToMainMenu()
    {
        mainMenu.ShowUI();
        nickNameModal.HideUI();
    }

    public void ShowNicknameModal()
    {
        nickNameModal.ShowUI();
    }

    public void OpenControls()
    {
        commonUIController.ShowControlsUI();
    }

    public void OpenRules()
    {
        commonUIController.ShowRulesUI();
    }

    public void OpenDebug()
    {
        commonUIController.ShowDebugUI();
    }
}
