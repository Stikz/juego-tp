using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static PauseManager;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullets")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;

    [Header("Ammo")]
    public int maxAmmo = 10;

    [Header("Shooting")]
    public float cooldown = 0.5f;

    [Header("HUD")]
    public TMP_Text ammoText;

    private PlayerSoundController soundController;
    private Collider2D playerCollider;
    private BulletPool bulletPool;
    private int currentAmmo;
    private float lastShootTime = -999f;

    private void Awake()
    {
        playerCollider = GetComponentInParent<Collider2D>();
        soundController = GetComponent<PlayerSoundController>();

        // ACA SE APLICA EL PATRON OBJECT POOL (el taser crea su pool de balas)
        if (bulletPrefab != null)
            bulletPool = new BulletPool(bulletPrefab.GetComponent<Bullet>());
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateHUD();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (GameState.Paused) return;
        if (context.started)
            Shoot();
    }

    private void Shoot()
    {
        if (firePoint == null || bulletPool == null) return;
        if (currentAmmo <= 0 || Time.time - lastShootTime < cooldown) return;

        Bullet bullet = bulletPool.Get();
        bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        if (playerCollider != null && bulletCol != null)
            Physics2D.IgnoreCollision(bulletCol, playerCollider);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = firePoint.right * bulletSpeed;

        if (soundController != null)
            soundController.PlayTaser();

        lastShootTime = Time.time;
        currentAmmo--;
        UpdateHUD();
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (ammoText != null)
            ammoText.text = $"Taser: {currentAmmo} / {maxAmmo}";
    }
}
