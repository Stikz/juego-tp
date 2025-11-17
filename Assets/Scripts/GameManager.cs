using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Tilemap mesasTilemap;
    public TileBase mesaConDinero_Tile;

    public VictoryManager victoryManager; 

    private int totalMoney;
    private int moneyCollected;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        countInitialMoney();
    }

    void countInitialMoney()
    {
        totalMoney = 0;
        moneyCollected = 0;

        if (victoryManager != null)
        {
            victoryManager.victoryCanvas.SetActive(false);
        }

        foreach (var pos in mesasTilemap.cellBounds.allPositionsWithin)
        {
            if (mesasTilemap.GetTile(pos) == mesaConDinero_Tile)
            {
                totalMoney++;
            }
        }
        Debug.Log("Dinero total en el nivel: " + totalMoney);
    }

    public void collectMoney()
    {
        moneyCollected++;
        Debug.Log("Dinero recogido: " + moneyCollected);

        if (moneyCollected >= totalMoney)
        {
            winGame(); 
        }
    }

    void winGame()
    {
        Debug.Log("¡GANASTE!");

        victoryManager.ShowVictoryScreen();
    }
}