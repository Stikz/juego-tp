using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("LevelSelector");
    }

   
    public void AbrirOpciones()
    {
        Debug.Log("Abriendo opciones...");
    }

    
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); 
    }
}