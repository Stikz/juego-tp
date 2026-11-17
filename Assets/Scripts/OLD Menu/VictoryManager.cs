using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryCanvas;

    private void Awake()
    {
        if (victoryCanvas != null)
            victoryCanvas.SetActive(false);
    }

    // PATRON OBSERVER
    private void OnEnable()
    {
        GameEvents.OnVictory += ShowVictoryScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnVictory -= ShowVictoryScreen;
    }

    public void ShowVictoryScreen()
    {
        if (victoryCanvas != null)
            victoryCanvas.SetActive(true);

        GameFlowFacade.StopGameplay();
    }

    public void GoToNextLevel()
    {
        GameFlowFacade.PrepareSceneChange();

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("Main Menu");
    }

    public void GoToMainMenu()
    {
        GameFlowFacade.PrepareSceneChange();
        SceneManager.LoadScene("Main Menu");
    }
}
