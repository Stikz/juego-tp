using UnityEngine;

public class Keycard : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    // Dar la keycard al jugador
                    PlayerInventory inventory = hit.GetComponent<PlayerInventory>();
                    if (inventory != null)
                        inventory.CollectKeycard();

                    // Destruir el objeto
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
