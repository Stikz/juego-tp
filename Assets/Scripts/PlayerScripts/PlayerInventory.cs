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

    // ESTA función es la que tiene que estar asignada al evento "Interact" del PlayerInput
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Debug.Log("OnInteract (PlayerInventory)");   // para confirmar que entra acá

        // 1) Intentar agarrar keycard
        TryPickupKeycard();

        // 2) Interactuar con la mesa / dinero / puerta
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, keycardPickupRadius);
    }
}
