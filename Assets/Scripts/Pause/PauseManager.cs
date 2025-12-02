using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public AudioSource menuAudio;

    private bool isPaused = false;

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
        Time.timeScale = 0f;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();
    }

    public void Resume()
    {
        if (menuAudio != null && menuAudio.clip != null)
            menuAudio.PlayOneShot(menuAudio.clip);

        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();
    }

    public void GoToMainMenu()
    {
        ManageScenes.Instance.LoadScene("Main Menu");
    }

    public void Options()
    {
        if (menuAudio != null && menuAudio.clip != null)
            menuAudio.PlayOneShot(menuAudio.clip);
    }
}
