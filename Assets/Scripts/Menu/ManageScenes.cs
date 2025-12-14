using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class ManageScenes : MonoBehaviour
{
    public static ManageScenes Instance;

    public AudioSource menuAudio;

    public AudioClip hoverClip;
    public AudioClip clickClip;

    [Range(0.8f, 1.2f)] public float pitchMin = 0.95f;
    [Range(0.8f, 1.2f)] public float pitchMax = 1.05f;

    [Range(0.5f, 1.2f)] public float hoverPitchMin = 0.85f;
    [Range(0.5f, 1.2f)] public float hoverPitchMax = 0.95f;

    [Range(0.5f, 1.5f)] public float clickPitchMin = 1.0f;
    [Range(0.5f, 1.5f)] public float clickPitchMax = 1.15f;

    public void PlayUIHover()
    {
        PlayUISfx(hoverClip, hoverPitchMin, hoverPitchMax);
    }

    public void PlayUIClick()
    {
        PlayUISfx(clickClip, clickPitchMin, clickPitchMax);
    }

    private void PlayUISfx(AudioClip clip, float pitchMin, float pitchMax)
    {
        if (menuAudio == null || clip == null) return;

        menuAudio.Stop();
        menuAudio.clip = clip;
        menuAudio.pitch = Random.Range(pitchMin, pitchMax);
        menuAudio.Play();
    }




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
        yield return null;      
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
