using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class ManageScenes : MonoBehaviour
{
    public static ManageScenes Instance;

    public AudioSource menuAudio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }

    public void QuitGame()
    {
        StartCoroutine(QuitRoutine());
    }
    IEnumerator LoadSceneRoutine(string sceneName)
    {
        Time.timeScale = 1f;

        DeactivateInputs();

        if (menuAudio != null && menuAudio.clip != null)
        {
            menuAudio.PlayOneShot(menuAudio.clip);
            yield return new WaitForSecondsRealtime(menuAudio.clip.length);
        }

        SceneManager.LoadScene(sceneName);

        ActivateInputs();
    }

    IEnumerator QuitRoutine()
    {
        if (menuAudio != null && menuAudio.clip != null)
        {
            menuAudio.PlayOneShot(menuAudio.clip);
            yield return new WaitForSecondsRealtime(menuAudio.clip.length);
        }

        Application.Quit();
    }

    private void DeactivateInputs()
    {
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();
    }

    private void ActivateInputs()
    {
        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();
    }
}
