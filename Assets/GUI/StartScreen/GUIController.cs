using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
