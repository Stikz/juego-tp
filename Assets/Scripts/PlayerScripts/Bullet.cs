using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
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

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.Die();
        }


        Destroy(gameObject);
    }

    public void IgnoreCollision(Collider2D col)
    {
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider != null)
            Physics2D.IgnoreCollision(bulletCollider, col);
    }
}
