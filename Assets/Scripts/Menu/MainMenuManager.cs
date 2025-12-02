using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource menuAudio;

    public void initGame()
    {
        ManageScenes.Instance.LoadScene("LevelSelector");
    }

    public void openOptions()
    {
        if (menuAudio != null && menuAudio.clip != null)
            menuAudio.Play();
    }

    public void exitGame()
    {
        ManageScenes.Instance.QuitGame();
    }
}
