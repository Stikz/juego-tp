using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public void uploadLevel(string sceneName)
    {
        ManageScenes.Instance.LoadScene(sceneName);
    }

    public void backToMenu()
    {
        ManageScenes.Instance.LoadScene("Main Menu");
    }
}
