using UnityEngine;

/// <summary>
/// Estrategia Concreta del Patron Strategy: Taser electrico estandar.
/// Dispara dardos electrificados con velocidad lineal que neutralizan a los guardias.
/// (Basado en la Diapositiva 5 de la presentacion Strategy de la catedra UADE).
/// </summary>
public class TaserWeaponStrategy : IWeaponStrategy
{
    private GameObject bulletPrefab;
    private float bulletSpeed;
    private int maxAmmo;
    private int currentAmmo;
    private float cooldown;
    private float lastShootTime = -999f;

    public string WeaponName => "Taser";
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;

    public TaserWeaponStrategy(GameObject bulletPrefab, float bulletSpeed = 15f, int maxAmmo = 10, float cooldown = 0.5f)
    {
        this.bulletPrefab = bulletPrefab;
        this.bulletSpeed = bulletSpeed;
        this.maxAmmo = maxAmmo;
        this.currentAmmo = maxAmmo;
        this.cooldown = cooldown;
    }

    public bool CanShoot()
    {
        return (Time.time - lastShootTime >= cooldown) && currentAmmo > 0;
    }

    public void Attack(Transform firePoint, Collider2D playerCollider, PlayerSoundController soundController)
    {
        if (!CanShoot()) return;
        if (firePoint == null) return;

        GameObject bullet;

        // Si existe BulletPool activo se reutiliza, de lo contrario se usa Instantiate de respaldo
        if (BulletPool.Instance != null)
        {
            bullet = BulletPool.Instance.GetBullet(firePoint.position, firePoint.rotation);
        }
        else if (bulletPrefab != null)
        {
            bullet = Object.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            return;
        }

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        if (playerCollider != null && bulletCol != null)
        {
            Physics2D.IgnoreCollision(bulletCol, playerCollider);
        }

        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
        {
            rbBullet.linearVelocity = firePoint.right * bulletSpeed;
        }

        if (soundController != null)
        {
            soundController.PlayTaser();
        }

        lastShootTime = Time.time;
        currentAmmo--;

        // Patron Observer: Notifica cambio de municion
        GameEvents.TriggerWeaponAmmoChanged(WeaponName, currentAmmo, maxAmmo);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        GameEvents.TriggerWeaponAmmoChanged(WeaponName, currentAmmo, maxAmmo);
    }
}
