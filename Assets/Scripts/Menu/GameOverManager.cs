using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public AudioSource menuAudio;

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);

        // Bloquear input del jugador
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();

        Time.timeScale = 0f;
    }

    public void Retry()
    {
        StartCoroutine(PlaySoundAndReload());
    }

    public void MainMenu()
    {
        StartCoroutine(PlaySoundAndLoadScene("Main Menu"));
    }

    IEnumerator PlaySoundAndReload()
    {
        Time.timeScale = 1f;

        if (menuAudio != null && menuAudio.clip != null)
            menuAudio.PlayOneShot(menuAudio.clip);

        yield return new WaitForSecondsRealtime(menuAudio.clip.length);

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        Time.timeScale = 1f;

        if (menuAudio != null && menuAudio.clip != null)
            menuAudio.PlayOneShot(menuAudio.clip);

        yield return new WaitForSecondsRealtime(menuAudio.clip.length);

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        SceneManager.LoadScene(sceneName);
    }
}
