using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static PauseManager;

/// <summary>
/// Contexto del Patron Strategy: Administra y ejecuta la estrategia de disparo/herramienta activa (IWeaponStrategy).
/// Permite alternar entre Taser y gadgets especiales sin modificar la logica de disparo.
/// (Basado en la Diapositiva 6 y 7 de la presentacion Strategy de la catedra UADE).
/// </summary>
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

    // --- Patron Strategy ---
    private IWeaponStrategy currentWeaponStrategy;
    public IWeaponStrategy CurrentWeaponStrategy => currentWeaponStrategy;

    private TaserWeaponStrategy taserStrategy;
    private EMPWeaponStrategy empStrategy;

    private void Awake()
    {
        playerCollider = GetComponentInParent<Collider2D>();
        soundController = GetComponent<PlayerSoundController>();
    }

    private void Start()
    {
        // Instanciacion de estrategias de armamento
        taserStrategy = new TaserWeaponStrategy(bulletPrefab, bulletSpeed, maxAmmo, cooldown);
        empStrategy = new EMPWeaponStrategy(6f, 3, 2f);

        // Estrategia por defecto: Taser
        SetWeapon(taserStrategy);
    }

    private void OnEnable()
    {
        GameEvents.OnWeaponAmmoChanged += OnAmmoChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnWeaponAmmoChanged -= OnAmmoChanged;
    }

    private void Update()
    {
        // Atajo para alternar estrategias con las teclas 1 y 2
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.digit1Key.wasPressedThisFrame)
            {
                SetWeapon(taserStrategy);
            }
            else if (kb.digit2Key.wasPressedThisFrame)
            {
                SetWeapon(empStrategy);
            }
        }
    }

    /// <summary>
    /// Metodo cliente del Patron Strategy para asignar la estrategia deseada.
    /// (Diapositiva 6 y 7 de la presentacion Strategy).
    /// </summary>
    public void SetWeapon(IWeaponStrategy weapon)
    {
        if (weapon == null) return;
        currentWeaponStrategy = weapon;
        UpdateHUD(currentWeaponStrategy.WeaponName, currentWeaponStrategy.CurrentAmmo, currentWeaponStrategy.MaxAmmo);
    }

    private void OnAmmoChanged(string weaponName, int currentAmmo, int maxAmmoCount)
    {
        UpdateHUD(weaponName, currentAmmo, maxAmmoCount);
    }

    private void UpdateHUD(string weaponName, int currentAmmo, int maxAmmoCount)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{weaponName}: {currentAmmo} / {maxAmmoCount}";
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (GameState.Paused) return;
        if (context.started)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (currentWeaponStrategy == null) return;

        // Patron Strategy: Se delega el algoritmo de ataque a la estrategia activa
        currentWeaponStrategy.Attack(firePoint, playerCollider, soundController);
    }

    public void Reload()
    {
        currentWeaponStrategy?.Reload();
    }
}
