using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public AudioSource menuAudio;

    private bool isPaused = false;

    // 👉 Esta función se va a enlazar al Action "Pause" del PlayerInput (Invoke Unity Events)
    public void OnPause(InputAction.CallbackContext ctx)
    {
        // Solo actuamos cuando la acción se "performea" (no en started/canceled)
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
    }

    public void Resume()
    {
        if (menuAudio != null && menuAudio.clip != null)
        {
            menuAudio.PlayOneShot(menuAudio.clip);
        }

        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        StartCoroutine(PlaySoundAndLoad("Main Menu"));
    }

    public void Options()
    {
        if (menuAudio != null && menuAudio.clip != null)
        {
            menuAudio.PlayOneShot(menuAudio.clip);
        }
        Debug.Log("Abrir Opciones (te hago menú si querés)");
    }

    IEnumerator PlaySoundAndLoad(string sceneName)
    {
        Time.timeScale = 1f;

        if (menuAudio != null && menuAudio.clip != null)
        {
            menuAudio.PlayOneShot(menuAudio.clip);
            yield return new WaitForSecondsRealtime(menuAudio.clip.length);
        }

        SceneManager.LoadScene(sceneName);
    }
}
