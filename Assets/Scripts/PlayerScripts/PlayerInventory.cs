using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private bool hasKeycard = false;

    [Header("Win Condition / Interacción con tiles")]
    public Tilemap WinCondition;
    public TileBase mesaConDinero_Tile;
    public TileBase mesaVacia_Tile;

    [Header("Keycard")]
    public float keycardPickupRadius = 1.2f;

    // This function is the one that needs to be assigned to the "Interact" event of the PlayerInput
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Debug.Log("OnInteract (PlayerInventory)");  

        TryPickupKeycard();

        Interactuar();
    }

    void TryPickupKeycard()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, keycardPickupRadius);
        Debug.Log($"TryPickupKeycard: {hits.Length} colliders alrededor del jugador.");

        foreach (var hit in hits)
        {
            Debug.Log($"  - Collider: {hit.name}");

            Keycard keycard = hit.GetComponentInParent<Keycard>();
            if (keycard != null)
            {
                Debug.Log($"Keycard encontrada en: {keycard.gameObject.name}");

                CollectKeycard();
                Destroy(keycard.gameObject);
                break;
            }
        }
    }

    void Interactuar()
    {
        if (WinCondition == null) return;

        Vector3Int celdaActual = WinCondition.WorldToCell(transform.position);
        TileBase tileEnCelda = WinCondition.GetTile(celdaActual);

        if (tileEnCelda == mesaConDinero_Tile)
        {
            WinCondition.SetTile(celdaActual, mesaVacia_Tile);
            GameManager.Instance.collectMoney();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, keycardPickupRadius);
    }
}
