using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CommonUIController : MonoBehaviour
{
    RulesUIController rulesUIController;
    ControlsUIController controlsUIController;
    DebugUIController debugUIController;

    private bool commonUIVisible = false;

    // Start is called before the first frame update
    void Awake()
    {
        rulesUIController = GetComponentInChildren<RulesUIController>();

        controlsUIController = GetComponentInChildren<ControlsUIController>();

        debugUIController = GetComponentInChildren<DebugUIController>();
    }

    void Start()
    {
        HideRulesUI();
        HideControlsUI();
        HideDebugUI();
    }

    public bool CommonUIVisible
    {
        get { return commonUIVisible; }
    }

    public void ShowRulesUI()
    {
        rulesUIController.ShowUI();
        controlsUIController.HideUI();
        debugUIController.HideUI();
        commonUIVisible = true;
    }

    public void HideRulesUI()
    {
        rulesUIController.HideUI();
        commonUIVisible = false;
    }

    public void ShowControlsUI()
    {
        controlsUIController.ShowUI();
        rulesUIController.HideUI();
        debugUIController.HideUI();
        commonUIVisible = true;
    }

    public void HideControlsUI()
    {
        controlsUIController.HideUI();
        commonUIVisible = false;
    }

    public void ShowDebugUI()
    {
        debugUIController.ShowUI();
        rulesUIController.HideUI();
        controlsUIController.HideUI();
        commonUIVisible = true;
    }

    public void HideDebugUI()
    {
        debugUIController.HideUI();
        commonUIVisible = false;
    }
}
