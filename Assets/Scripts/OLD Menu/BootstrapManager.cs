using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    [SerializeField] private string startScene = "Main Menu";

    private void Start()
    {
        SceneManager.LoadScene(startScene);
    }
}
