using UnityEngine;

using UnityEngine.Tilemaps;



public class PlayerInventory : MonoBehaviour

{

    private bool hasKeycard = false;



    public Tilemap WinCondition;

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

        Vector3Int celdaActual = WinCondition.WorldToCell(transform.position);



        TileBase tileEnCelda = WinCondition.GetTile(celdaActual);



        if (tileEnCelda == mesaConDinero_Tile)

        {

            WinCondition.SetTile(celdaActual, mesaVacia_Tile);



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