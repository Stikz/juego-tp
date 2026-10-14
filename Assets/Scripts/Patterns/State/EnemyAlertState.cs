using UnityEngine;

/// <summary>
/// Estado Concreto del Patron State: El enemigo detecta al jugador o un estimulo suspechoso,
/// detiene su patrulla y desencadena la alerta/Game Over.
/// </summary>
public class EnemyAlertState : IEnemyState
{
    public void Enter(EnemyPatrol enemy)
    {
        if (enemy.AIPath != null)
        {
            enemy.AIPath.canMove = false;
            enemy.AIPath.isStopped = true;
        }

        if (enemy.Anim != null)
        {
            enemy.Anim.SetFloat("Speed", 0f);
        }

        if (enemy.FootstepSource != null)
        {
            enemy.FootstepSource.Stop();
        }

        // Notificar al sistema mediante el patron Observer
        GameEvents.TriggerPlayerDetected();

        if (enemy.GameOverManager != null)
        {
            enemy.GameOverManager.ShowGameOver();
        }
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
        // En alerta/deteccion no realiza patrullaje adicional
    }

    public void Exit(EnemyPatrol enemy)
    {
    }
}
