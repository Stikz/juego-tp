using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [Header("Balas")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;

    [Header("Munición")]
    public int maxAmmo = 10;
    private int currentAmmo;

    [Header("Disparo")]
    public float cooldown = 0.5f;
    private float lastShootTime;

    [Header("HUD")]
    public TMP_Text ammoText;

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    // Input System: acción "Fire" (Button)
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time - lastShootTime < cooldown) return;
        if (currentAmmo <= 0) return;
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Ignorar colisión con jugador
        Collider2D playerCol = GetComponentInParent<Collider2D>(); // si el shooting está en Body, sube al padre
        Collider2D bulletCol = bullet.GetComponent<Collider2D>();
        if (playerCol != null && bulletCol != null)
            Physics2D.IgnoreCollision(bulletCol, playerCol);

        // Velocidad a la bala (OJO: es 'velocity', no 'linearVelocity')
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        if (rbBullet != null)
            rbBullet.linearVelocity = firePoint.right * bulletSpeed;

        lastShootTime = Time.time;
        currentAmmo--;
        UpdateHUD();
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        UpdateHUD();
    }
}
