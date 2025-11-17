using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Tilemap mesasTilemap;
    public TileBase mesaConDinero_Tile;

    public VictoryManager victoryManager; 

    private int totalDinero;
    private int dineroRecogido;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        ContarDineroInicial();
    }

    void ContarDineroInicial()
    {
        totalDinero = 0;
        dineroRecogido = 0;

        if (victoryManager != null)
        {
            victoryManager.victoryCanvas.SetActive(false);
        }

        foreach (var pos in mesasTilemap.cellBounds.allPositionsWithin)
        {
            if (mesasTilemap.GetTile(pos) == mesaConDinero_Tile)
            {
                totalDinero++;
            }
        }
        Debug.Log("Dinero total en el nivel: " + totalDinero);
    }

    public void RecogerDinero()
    {
        dineroRecogido++;
        Debug.Log("Dinero recogido: " + dineroRecogido);

        if (dineroRecogido >= totalDinero)
        {
            GanarJuego(); 
        }
    }

    void GanarJuego()
    {
        Debug.Log("¡GANASTE!");

        victoryManager.ShowVictoryScreen();
    }
}