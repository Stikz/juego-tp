using UnityEngine;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string firstScene = "Main Menu";

    private void Start()
    {
        ManageScenes.Instance.LoadScene(firstScene);
    }
}
