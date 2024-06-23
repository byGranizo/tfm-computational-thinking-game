using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveMissionSelectUIController : MonoBehaviour
{
    GUIGameController guiGameController;
    VisualElement root;
    readonly Button[] cardsButtons = new Button[3];

    // Start is called before the first frame update
    void Start()
    {
        guiGameController = GetComponentInParent<GUIGameController>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            cardsButtons[index] = root.Q<Button>("Card_" + index);
            cardsButtons[index].clicked += () => OnCardButtonClicked(index);
        }
    }

    private void OnCardButtonClicked(int index)
    {
        guiGameController.OnCardButtonClicked(index);
    }

    public void RefreshCardUI(Texture2D cardTexture, int index)
    {
        cardsButtons[index].style.backgroundImage = new StyleBackground(cardTexture);

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
