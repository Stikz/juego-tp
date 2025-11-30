using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryCanvas;
    public AudioSource menuAudio;

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);

        // Block player input
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();

        Time.timeScale = 0f;
    }


    public void GoToLevelSelector()
    {
        StartCoroutine(PlaySoundAndLoad("LevelSelector"));
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();
    }

    public void GoToMainMenu()
    {
        StartCoroutine(PlaySoundAndLoad("Main Menu"));
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

    }

    public void OpenOptions()
    {
        menuAudio.PlayOneShot(menuAudio.clip);
        Debug.Log("Abriendo Opciones...");
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();
    }

    IEnumerator PlaySoundAndLoad(string sceneName)
    {
        Time.timeScale = 1f;
        menuAudio.PlayOneShot(menuAudio.clip);
        yield return new WaitForSecondsRealtime(menuAudio.clip.length);
        SceneManager.LoadScene(sceneName);
    }
}