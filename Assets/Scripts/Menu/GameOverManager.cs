using UnityEngine;
using UnityEngine.InputSystem;
using static PauseManager;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;

    private void OnEnable()
    {
        GameEvents.OnPlayerDetected += ShowGameOver;
        GameEvents.OnGameOver += ShowGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDetected -= ShowGameOver;
        GameEvents.OnGameOver -= ShowGameOver;
    }

    public void ShowGameOver()
    {
        if (gameOverCanvas != null)
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
