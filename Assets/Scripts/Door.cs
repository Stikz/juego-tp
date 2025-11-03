using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Configuración")]
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool requiresKeycard = false;
    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private Collider2D doorCollider;

    private bool playerNear = false;      // Detecta si el jugador está cerca
    private GameObject player;

    private void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = closedRotation;
        doorCollider = GetComponent<Collider2D>();
        if (doorCollider == null)
            Debug.LogWarning("La puerta necesita un Collider2D para bloquear al jugador.");
    }

    private void Update()
    {
        // Rotación suave
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);

        // Interacción
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            TryOpen(player);
        }
    }

    public void TryOpen(GameObject playerObj)
    {
        PlayerInventory inventory = playerObj.GetComponent<PlayerInventory>();

        if (requiresKeycard && (inventory == null || !inventory.HasKeycard()))
        {
            Debug.Log("¡Necesitas una keycard para abrir esta puerta!");
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
