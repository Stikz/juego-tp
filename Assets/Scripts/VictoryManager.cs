using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryCanvas;
    public AudioSource menuAudio;

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);
        Time.timeScale = 0f; 
    }


    public void GoToLevelSelector()
    {
        StartCoroutine(PlaySoundAndLoad("LevelSelector"));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(PlaySoundAndLoad("Main Menu"));
    }

    public void OpenOptions()
    {
        menuAudio.PlayOneShot(menuAudio.clip);
        Debug.Log("Abriendo Opciones...");
    }

    IEnumerator PlaySoundAndLoad(string sceneName)
    {
        Time.timeScale = 1f;
        menuAudio.PlayOneShot(menuAudio.clip);
        yield return new WaitForSecondsRealtime(menuAudio.clip.length);
        SceneManager.LoadScene(sceneName);
    }
}