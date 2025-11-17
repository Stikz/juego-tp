using UnityEngine;
using UnityEngine.Tilemaps; 

public class PlayerInteraction : MonoBehaviour
{
    public Tilemap mesasTilemap;      
    public TileBase MesaPlata_Tile; 
    public TileBase MesaVacia;     

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

        if (tileEnCelda == MesaPlata_Tile)
        {
            mesasTilemap.SetTile(celdaActual, MesaVacia);

            GameManager.Instance.RecogerDinero();
        }
    }
}