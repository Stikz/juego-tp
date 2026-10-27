using UnityEngine;

// ACA SE APLICA EL PATRON STATE (estado concreto: jugador detectado)
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
            enemy.Anim.SetFloat("Speed", 0f);

        if (enemy.FootstepSource != null)
            enemy.FootstepSource.Stop();

        // ACA SE APLICA EL PATRON OBSERVER (el guardia avisa, no conoce al GameOverManager)
        GameEvents.TriggerPlayerDetected();
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
    }

    public void Exit(EnemyPatrol enemy)
    {
    }
}
