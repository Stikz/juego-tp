using UnityEngine;
using UnityEngine.InputSystem;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryCanvas;
    public AudioSource menuAudio;

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();

        Time.timeScale = 0f;
    }

    public void GoToLevelSelector()
    {
        ManageScenes.Instance.LoadScene("LevelSelector");
    }

    public void GoToMainMenu()
    {
        ManageScenes.Instance.LoadScene("Main Menu");
    }

    public void OpenOptions()
    {
        if (menuAudio != null && menuAudio.clip != null)
            menuAudio.PlayOneShot(menuAudio.clip);
    }
}
