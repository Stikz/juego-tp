using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverCanvas;

    // ACA SE APLICA EL PATRON OBSERVER (observer: se suscribe a la deteccion del jugador)
    private void OnEnable()
    {
        GameEvents.OnPlayerDetected += ShowGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDetected -= ShowGameOver;
    }

    public void ShowGameOver()
    {
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        GameFlowFacade.StopGameplay();
    }

    public void Retry()
    {
        GameFlowFacade.PrepareSceneChange();
        ManageScenes.Instance.ReloadScene();
    }

    public void MainMenu()
    {
        GameFlowFacade.PrepareSceneChange();
        ManageScenes.Instance.LoadScene("Main Menu");
    }
}
