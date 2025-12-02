using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool requiresKeycard = false;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private Collider2D doorCollider;
    public bool playerNear = false;

    private GameObject player;

    private void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = closedRotation;
        doorCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    public void TryOpen(GameObject playerObj)
    {
        PlayerInventory inventory = playerObj.GetComponent<PlayerInventory>();

        if (requiresKeycard && (inventory == null || !inventory.HasKeycard()))
        {
            return;
        }

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        targetRotation = isOpen ? closedRotation * Quaternion.Euler(0f, 0f, openAngle) : closedRotation;

        if (doorCollider != null)
            doorCollider.enabled = !isOpen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            player = null;
        }
    }
}
