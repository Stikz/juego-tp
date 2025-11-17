using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class MainMenuManager : MonoBehaviour
{
    public AudioSource menuAudio;

    public void initGame()
    {
        StartCoroutine(PlaySoundAndLoad("LevelSelector"));
    }

    public void openOptions()
    {
        menuAudio.Play();
        Debug.Log("Abriendo opciones...");
    }

    public void exitGame()
    {
        StartCoroutine(PlaySoundAndQuit());
    }

    IEnumerator PlaySoundAndLoad(string sceneName)
    {
        menuAudio.Play();

        yield return new WaitForSeconds(menuAudio.clip.length);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlaySoundAndQuit()
    {
        menuAudio.Play();
        yield return new WaitForSeconds(menuAudio.clip.length);
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}