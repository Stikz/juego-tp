using UnityEngine;
using UnityEngine.Pool;

// PATRON OBJECT POOL
public class BulletPool
{
    private readonly Bullet prefab;
    private readonly RuntimeAnimatorController bulletLook;
    private readonly ObjectPool<Bullet> pool;

    public BulletPool(Bullet prefab, RuntimeAnimatorController bulletLook, int defaultCapacity = 10, int maxSize = 30)
    {
        this.prefab = prefab;
        this.bulletLook = bulletLook;
        pool = new ObjectPool<Bullet>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, defaultCapacity, maxSize);
    }

    public Bullet Get()
    {
        return pool.Get();
    }

    public void Release(Bullet bullet)
    {
        pool.Release(bullet);
    }

    private Bullet CreatePoolItem()
    {
        Bullet bullet = Object.Instantiate(prefab);
        bullet.SetPool(this);

        Animator animator = bullet.GetComponent<Animator>();
        if (animator != null && bulletLook != null)
            animator.runtimeAnimatorController = bulletLook;
        return bullet;
    }

    private void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Bullet bullet)
    {
        Object.Destroy(bullet.gameObject);
    }
}
