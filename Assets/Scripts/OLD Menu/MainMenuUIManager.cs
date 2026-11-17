using UnityEngine;
using static PauseManager;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject optionsPanel;

    private SkinMenuUI skinMenu;

    private void Start()
    {
        Canvas canvas = mainMenuPanel != null ? mainMenuPanel.GetComponentInParent<Canvas>() : GetComponentInParent<Canvas>();
        if (canvas != null)
            skinMenu = SkinMenuUI.Build(canvas.transform, this);

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        SetPanels(main: true, levels: false, options: false, skins: false);
        CursorMode.SetUI();
        GameState.Paused = false;
    }

    public void ShowLevelSelect()
    {
        SetPanels(main: false, levels: true, options: false, skins: false);
        CursorMode.SetUI();
    }

    public void ShowOptions()
    {
        SetPanels(main: false, levels: false, options: true, skins: false);
        CursorMode.SetUI();
    }

    public void ShowSkins()
    {
        SetPanels(main: false, levels: false, options: false, skins: true);
        CursorMode.SetUI();
    }

    private void SetPanels(bool main, bool levels, bool options, bool skins)
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(main);
        if (levelSelectPanel) levelSelectPanel.SetActive(levels);
        if (optionsPanel) optionsPanel.SetActive(options);

        if (skinMenu != null)
        {
            skinMenu.gameObject.SetActive(skins);
            skinMenu.OpenButton.SetActive(main);
        }
    }

    public void Quit()
    {
        ManageScenes.Instance.QuitGame();
    }
}
