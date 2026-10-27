using UnityEngine;
using static PauseManager;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;   
    [SerializeField] private GameObject levelSelectPanel; 
    [SerializeField] private GameObject optionsPanel;      

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (levelSelectPanel) levelSelectPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);

        CursorMode.SetUI();
        GameState.Paused = false;
    }

    public void ShowLevelSelect()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (levelSelectPanel) levelSelectPanel.SetActive(true);
        if (optionsPanel) optionsPanel.SetActive(false);

        CursorMode.SetUI();
    }

    public void ShowOptions()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (levelSelectPanel) levelSelectPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(true);

        CursorMode.SetUI();
    }


    public void Quit()
    {
        ManageScenes.Instance.QuitGame();
    }
}
