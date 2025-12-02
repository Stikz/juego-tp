using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public AudioSource menuAudio; 

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();

        Time.timeScale = 0f;
    }

    public void Retry()
    {
        ManageScenes.Instance.ReloadScene();
    }

    public void MainMenu()
    {
        ManageScenes.Instance.LoadScene("Main Menu");
    }
}
