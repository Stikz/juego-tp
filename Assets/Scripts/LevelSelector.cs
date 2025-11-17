using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // Esta función es súper útil. Puedes reusarla para CUALQUIER nivel.
    // Simplemente le dices el nombre de la escena que debe cargar.
    public void CargarNivel(string nombreDeLaEscena)
    {
        SceneManager.LoadScene(nombreDeLaEscena);
    }

    // Función para el botón de volver
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}