using UnityEngine;
using UnityEngine.Pool;

// ACA SE APLICA EL PATRON OBJECT POOL (ObjectPool<T> de Unity, como en la clase)
public class BulletPool
{
    private readonly Bullet prefab;
    private readonly ObjectPool<Bullet> pool;

    public BulletPool(Bullet prefab, int defaultCapacity = 10, int maxSize = 30)
    {
        this.prefab = prefab;
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
