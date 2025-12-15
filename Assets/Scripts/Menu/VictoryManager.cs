using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static PauseManager;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryCanvas;

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);
        GameState.Paused = true;
        CursorMode.SetUI();

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].DeactivateInput();
    }

    public void GoToNextLevel()
    {
        GameState.Paused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void GoToMainMenu()
    {
        GameState.Paused = false;

        if (PlayerInput.all.Count > 0)
            PlayerInput.all[0].ActivateInput();

        SceneManager.LoadScene("Main Menu");
    }
}
