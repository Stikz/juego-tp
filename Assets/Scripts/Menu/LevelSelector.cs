using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        ManageScenes.Instance.LoadScene(sceneName);
    }
}
