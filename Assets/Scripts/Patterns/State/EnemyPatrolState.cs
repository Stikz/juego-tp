using UnityEngine;

// ACA SE APLICA EL PATRON STATE (estado concreto: patrullar)
public class EnemyPatrolState : IEnemyState
{
    public void Enter(EnemyPatrol enemy)
    {
        if (enemy.PatrolPoints == null || enemy.PatrolPoints.Length == 0) return;

        if (enemy.AIPath != null)
        {
            enemy.AIPath.canMove = true;
            enemy.AIPath.isStopped = false;
            enemy.AIPath.maxSpeed = enemy.PatrolSpeed;
            enemy.AIPath.destination = enemy.PatrolPoints[enemy.CurrentPoint].position;
        }
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
        if (enemy.PatrolPoints == null || enemy.PatrolPoints.Length == 0) return;

        Transform targetPoint = enemy.PatrolPoints[enemy.CurrentPoint];
        if (targetPoint == null) return;

        if (enemy.AIPath != null)
        {
            if (enemy.AIPath.destination != targetPoint.position)
                enemy.AIPath.destination = targetPoint.position;

            if (Vector2.Distance(enemy.transform.position, targetPoint.position) < 0.2f)
            {
                enemy.ChangeState(enemy.WaitState);
                return;
            }
        }

        enemy.HandleFootsteps();
        enemy.DetectPlayer();
    }

    public void Exit(EnemyPatrol enemy)
    {
        if (enemy.FootstepSource != null)
            enemy.FootstepSource.Stop();
    }
}
