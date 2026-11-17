using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Tilemap mesasTilemap;
    public TileBase mesaConDinero_Tile;

    private int totalMoney;
    private int moneyCollected;

    private void Awake()
    {
        // PATRON SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CountInitialMoney();
    }

    private void CountInitialMoney()
    {
        totalMoney = 0;
        moneyCollected = 0;

        if (mesasTilemap == null || mesaConDinero_Tile == null) return;

        foreach (var pos in mesasTilemap.cellBounds.allPositionsWithin)
        {
            if (mesasTilemap.GetTile(pos) == mesaConDinero_Tile)
                totalMoney++;
        }
    }

    public void collectMoney()
    {
        moneyCollected++;

        if (moneyCollected >= totalMoney)
        {
            // PATRON OBSERVER
            GameEvents.TriggerVictory();
        }
    }
}
