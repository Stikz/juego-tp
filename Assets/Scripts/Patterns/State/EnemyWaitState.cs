using UnityEngine;

/// <summary>
/// Estado Concreto del Patron State: El enemigo se detiene en un punto de patrulla durante waitTimeAtPoint
/// antes de continuar hacia el siguiente punto.
/// </summary>
public class EnemyWaitState : IEnemyState
{
    private float timer = 0f;

    public void Enter(EnemyPatrol enemy)
    {
        timer = 0f;

        if (enemy.AIPath != null)
        {
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
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
        timer += deltaTime;

        // Durante la espera el enemigo sigue vigilando su entorno
        enemy.DetectPlayer();

        if (timer >= enemy.WaitTimeAtPoint)
        {
            if (enemy.PatrolPoints != null && enemy.PatrolPoints.Length > 0)
            {
                enemy.CurrentPoint = (enemy.CurrentPoint + 1) % enemy.PatrolPoints.Length;
            }

            enemy.ChangeState(enemy.PatrolState);
        }
    }

    public void Exit(EnemyPatrol enemy)
    {
        if (enemy.AIPath != null)
        {
            enemy.AIPath.isStopped = false;
        }
    }
}
