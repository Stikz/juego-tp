using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private bool hasKeycard = false;

    public Tilemap WinCondition;
    public TileBase tableWithMoney;
    public TileBase tableEmpty;
    private Door doorNear;

    public float keycardPickupRadius = 1.2f;
    public GameObject keycardIconUI;

    private void Start()
    {
        if (keycardIconUI != null)
            keycardIconUI.SetActive(false);
    }
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

        if (keycardIconUI != null)
            keycardIconUI.SetActive(true);
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
