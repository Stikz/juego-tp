using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool isPaused = false;
    public AudioSource menuAudio;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        menuAudio.PlayOneShot(menuAudio.clip);

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
        menuAudio.PlayOneShot(menuAudio.clip);
        Debug.Log("Abrir Opciones (te hago menú si querés)");
    }

    IEnumerator PlaySoundAndLoad(string sceneName)
    {
        Time.timeScale = 1f;

        menuAudio.PlayOneShot(menuAudio.clip);

        yield return new WaitForSecondsRealtime(menuAudio.clip.length);

        SceneManager.LoadScene(sceneName);
    }
}
