using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class MainMenuManager : MonoBehaviour
{
    public AudioSource menuAudio;

    public void IniciarJuego()
    {
        StartCoroutine(PlaySoundAndLoad("LevelSelector"));
    }

    public void AbrirOpciones()
    {
        menuAudio.Play();
        Debug.Log("Abriendo opciones...");
    }

    public void SalirDelJuego()
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