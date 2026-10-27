using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 5f;

    private BulletPool pool;
    private bool released;

    public void SetPool(BulletPool bulletPool)
    {
        pool = bulletPool;
    }

    private void OnEnable()
    {
        released = false;
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyPatrol enemy = collision.gameObject.GetComponent<EnemyPatrol>();
        if (enemy != null)
            enemy.Die();

        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
        if (enemy != null)
            enemy.Die();

        ReturnToPool();
    }

    // ACA SE APLICA EL PATRON OBJECT POOL (la bala vuelve al pool en vez de destruirse)
    private void ReturnToPool()
    {
        if (released) return;
        released = true;

        if (pool != null)
            pool.Release(this);
        else
            Destroy(gameObject);
    }
}
