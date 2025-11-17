using UnityEngine;
using UnityEngine.Tilemaps; 

public class PlayerInventory : MonoBehaviour
{
    private bool hasKeycard = false;

    public Tilemap mesasTilemap;
    public TileBase mesaConDinero_Tile;
    public TileBase mesaVacia_Tile;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactuar();
        }
    }

    void Interactuar()
    {
        Vector3Int celdaActual = mesasTilemap.WorldToCell(transform.position);

        TileBase tileEnCelda = mesasTilemap.GetTile(celdaActual);

        if (tileEnCelda == mesaConDinero_Tile)
        {
            mesasTilemap.SetTile(celdaActual, mesaVacia_Tile);

            GameManager.Instance.RecogerDinero();
        }
    }

    public void CollectKeycard()
    {
        hasKeycard = true;
        Debug.Log("¡Keycard recogida!");
    }

    public bool HasKeycard()
    {
        return hasKeycard;
    }
}