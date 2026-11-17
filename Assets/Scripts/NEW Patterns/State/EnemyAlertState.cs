using UnityEngine;

// PATRON STATE
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

        // PATRON OBSERVER
        GameEvents.TriggerPlayerDetected();
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
    }

    public void Exit(EnemyPatrol enemy)
    {
    }
}
