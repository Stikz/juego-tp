using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public AudioSource menuAudio;
    public GameObject defaultPauseSelected;
    private bool isPaused = false;
    public GameObject optionsCanvas;  
    public static class GameState
    {
        public static bool Paused;
    }
    public void OpenOptions()
    {
        if (pauseCanvas) pauseCanvas.SetActive(false);
        if (optionsCanvas) optionsCanvas.SetActive(true);

    }

    public void CloseOptionsBackToPause()
    {
        if (optionsCanvas) optionsCanvas.SetActive(false);
        if (pauseCanvas) pauseCanvas.SetActive(true);
    }
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);
        GameState.Paused = true;

        CursorMode.SetUI();

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        isPaused = false;
        GameState.Paused = false;

        CursorMode.SetGameplay();

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();
    }


    public void GoToMainMenu()
    {
        ManageScenes.Instance.LoadScene("Main Menu");
    }

}
