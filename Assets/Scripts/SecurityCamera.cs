using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Alerta a enemigos")]
    public float alertRadius = 7f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet")) // asegurate de ponerle tag "Bullet" a tus balas
        {
            DestroyCamera();
        }
    }
    public void DestroyCamera()
    {
        // Avisar a enemigos cercanos
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, alertRadius);
        foreach (Collider2D hit in hits)
        {
            EnemyPatrol enemy = hit.GetComponent<EnemyPatrol>();
            if (enemy != null)
            {
                enemy.Investigate(transform.position); // Llamada al pathfinding de investigación
            }
        }

        Destroy(gameObject); // destruir la cámara
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }

}
