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
    public int TotalMoney => totalMoney;
    public int MoneyCollected => moneyCollected;

    void Awake()
    {
        // Patron Singleton (Diapositiva 4 y 7 de la presentacion UADE)
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        countInitialMoney();
    }

    void countInitialMoney()
    {
        totalMoney = 0;
        moneyCollected = 0;

        if (victoryManager != null && victoryManager.victoryCanvas != null)
        {
            victoryManager.victoryCanvas.SetActive(false);
        }

        if (mesasTilemap != null && mesaConDinero_Tile != null)
        {
            foreach (var pos in mesasTilemap.cellBounds.allPositionsWithin)
            {
                if (mesasTilemap.GetTile(pos) == mesaConDinero_Tile)
                {
                    totalMoney++;
                }
            }
        }

        GameEvents.TriggerMoneyCollected(moneyCollected, totalMoney);
    }

    public void collectMoney()
    {
        moneyCollected++;
        GameEvents.TriggerMoneyCollected(moneyCollected, totalMoney);

        if (moneyCollected >= totalMoney)
        {
            winGame(); 
        }
    }

    void winGame()
    {
        GameEvents.TriggerVictory();
        if (victoryManager != null)
        {
            victoryManager.ShowVictoryScreen();
        }
    }
}