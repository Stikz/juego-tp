using UnityEngine;

/// <summary>
/// Estrategia Concreta Alternativa del Patron Strategy: Pulso / Dardo EMP de Distraccion.
/// Permite desactivar temporalmente sensores/camaras cercanas o aturdir enemigos en silencio.
/// Demuestra la capacidad de intercambiar familias de algoritmos en tiempo de ejecucion (OCP).
/// </summary>
public class EMPWeaponStrategy : IWeaponStrategy
{
    private float range;
    private int maxAmmo;
    private int currentAmmo;
    private float cooldown;
    private float lastShootTime = -999f;

    public string WeaponName => "Pulso EMP";
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;

    public EMPWeaponStrategy(float range = 6f, int maxAmmo = 3, float cooldown = 2f)
    {
        this.range = range;
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

        // Desactiva temporalmente camaras de seguridad en el radio de impacto
        Collider2D[] colliders = Physics2D.OverlapCircleAll(firePoint.position, range);
        foreach (var col in colliders)
        {
            var cameraSensor = col.GetComponentInParent<CameraConeOscillatingWithPause>();
            if (cameraSensor != null)
            {
                cameraSensor.enabled = false;
            }

            var enemy = col.GetComponentInParent<EnemyPatrol>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }

        if (soundController != null)
        {
            soundController.PlayTaser();
        }

        lastShootTime = Time.time;
        currentAmmo--;

        GameEvents.TriggerWeaponAmmoChanged(WeaponName, currentAmmo, maxAmmo);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        GameEvents.TriggerWeaponAmmoChanged(WeaponName, currentAmmo, maxAmmo);
    }
}
