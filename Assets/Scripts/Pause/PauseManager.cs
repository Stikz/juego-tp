using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public AudioSource menuAudio;
    public GameObject defaultPauseSelected;
    public GameObject optionsCanvas;
    private bool isPaused = false;

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
        GameFlowFacade.StopGameplay();
    }

    public void Resume()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);
        GameFlowFacade.ResumeGameplay();
    }

    public void GoToMainMenu()
    {
        GameFlowFacade.PrepareSceneChange();
        ManageScenes.Instance.LoadScene("Main Menu");
    }
}
