using UnityEngine;
using UnityEngine.InputSystem;
using static PauseManager;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);
        GameState.Paused = true;
        CursorMode.SetUI();
        if (PlayerInput.all.Count > 0) PlayerInput.all[0].DeactivateInput();
    }

    public void Retry()
    {
        GameState.Paused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        ManageScenes.Instance.ReloadScene();
    }

    public void MainMenu()
    {
        GameState.Paused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        ManageScenes.Instance.LoadScene("Main Menu");
    }
}
