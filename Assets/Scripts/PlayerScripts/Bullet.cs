using UnityEngine;

/// <summary>
/// Proyectil del Taser. Compatible con el patron Object Pool para reutilizacion continua sin destruccion.
/// </summary>
public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;

    private void OnEnable()
    {
        CancelInvoke(nameof(DeactivateOrDestroy));
        Invoke(nameof(DeactivateOrDestroy), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(DeactivateOrDestroy));
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyPatrol enemy = collision.gameObject.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.Die();
        }

        DeactivateOrDestroy();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.Die();
        }

        DeactivateOrDestroy();
    }

    private void DeactivateOrDestroy()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IgnoreCollision(Collider2D col)
    {
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider != null && col != null)
            Physics2D.IgnoreCollision(bulletCollider, col);
    }
}
