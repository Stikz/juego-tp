using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class LevelSelector : MonoBehaviour
{
    public AudioSource menuAudio;

    public void uploadLevel (string nombreDeLaEscena)
    {
        StartCoroutine(PlaySoundAndLoad(nombreDeLaEscena));
    }

    public void backToMenu()
    {
        StartCoroutine(PlaySoundAndLoad("Main Menu"));
    }

    IEnumerator PlaySoundAndLoad(string sceneName)
    {
        menuAudio.Play();

        yield return new WaitForSeconds(menuAudio.clip.length);

        SceneManager.LoadScene(sceneName);
    }
}