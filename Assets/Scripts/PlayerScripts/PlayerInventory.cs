using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private bool hasKeycard = false;

    [Header("Win Condition")]
    public Tilemap WinCondition;
    public TileBase tableWithMoney;
    public TileBase tableEmpty;
    private Door doorNear;
    [Header("Keycard")]
    public float keycardPickupRadius = 1.2f;

    // This function is the one that needs to be assigned to the "Interact" event of the PlayerInput
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        TryPickupKeycard();
        DetectNearbyDoor();
        TryOpenDoor();
        Interactuar();

    }

    void TryPickupKeycard()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, keycardPickupRadius);
        foreach (var hit in hits)
        {
            Keycard keycard = hit.GetComponentInParent<Keycard>();
            if (keycard != null)
            {
                CollectKeycard();
                Destroy(keycard.gameObject);
                break;
            }
        }
    }

    void Interactuar()
    {
        if (WinCondition == null) return;

        Vector3Int currentCell = WinCondition.WorldToCell(transform.position);
        TileBase cellTile = WinCondition.GetTile(currentCell);

        if (cellTile == tableWithMoney)
        {
            WinCondition.SetTile(currentCell, tableEmpty);
            GameManager.Instance.collectMoney();
        }
    }

    private void Update()
    {
        DetectNearbyDoor();
    }

    void DetectNearbyDoor()
    {
        doorNear = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f);

        foreach (var hit in hits)
        {
            Door door = hit.GetComponentInParent<Door>();
            if (door != null)
            {
                doorNear = door;
                break;
            }
        }
    }
    void TryOpenDoor()
    {
        if (doorNear != null)
        {
            doorNear.TryOpen(gameObject);
        }
    }


    public void CollectKeycard()
    {
        hasKeycard = true;
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
