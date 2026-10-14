using UnityEngine;

/// <summary>
/// Patron Strategy: Interfaz comun para la familia de estrategias de armamento y herramientas.
/// (Basado en la Diapositiva 4 de la presentacion Strategy de la catedra UADE).
/// </summary>
public interface IWeaponStrategy
{
    string WeaponName { get; }
    int CurrentAmmo { get; }
    int MaxAmmo { get; }
    bool CanShoot();
    void Attack(Transform firePoint, Collider2D playerCollider, PlayerSoundController soundController);
    void Reload();
}
