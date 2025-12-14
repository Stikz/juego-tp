using UnityEngine;
using UnityEngine.InputSystem;
using static PauseManager;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryCanvas;

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);
        GameState.Paused = true;
        CursorMode.SetUI();
        if (PlayerInput.all.Count > 0) PlayerInput.all[0].DeactivateInput();
    }

    public void GoToLevelSelector()
    {
        GameState.Paused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        ManageScenes.Instance.LoadScene("Main Menu");
    }

    public void GoToMainMenu()
    {
        GameState.Paused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        ManageScenes.Instance.LoadScene("Main Menu");
    }
}
