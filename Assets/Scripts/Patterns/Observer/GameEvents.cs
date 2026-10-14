using System;
using UnityEngine;

/// <summary>
/// Patron Observer: Despachador central de eventos del juego.
/// Permite desacoplar completamente emisores (sensores, enemigos, camaras, mesas)
/// de los receptores (GameManager, GameOverManager, VictoryManager, HUD).
/// </summary>
public static class GameEvents
{
    // Evento disparado cuando cualquier sensor (camara o guardia) detecta al jugador
    public static event Action OnPlayerDetected;

    // Evento disparado cuando se recolecta dinero (dinero recolectado, total de dinero)
    public static event Action<int, int> OnMoneyCollected;

    // Evento disparado cuando el jugador cambia o dispara un arma (nombre, municion actual, municion maxima)
    public static event Action<string, int, int> OnWeaponAmmoChanged;

    // Evento disparado al ganar la partida
    public static event Action OnVictory;

    // Evento disparado al perder la partida
    public static event Action OnGameOver;

    public static void TriggerPlayerDetected()
    {
        OnPlayerDetected?.Invoke();
    }

    public static void TriggerMoneyCollected(int current, int total)
    {
        OnMoneyCollected?.Invoke(current, total);
    }

    public static void TriggerWeaponAmmoChanged(string weaponName, int currentAmmo, int maxAmmo)
    {
        OnWeaponAmmoChanged?.Invoke(weaponName, currentAmmo, maxAmmo);
    }

    public static void TriggerVictory()
    {
        OnVictory?.Invoke();
    }

    public static void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }
}
