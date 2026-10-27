using UnityEngine;

// ACA SE APLICA EL PATRON STATE (estado concreto: esperar en el punto)
public class EnemyWaitState : IEnemyState
{
    private float timer = 0f;

    public void Enter(EnemyPatrol enemy)
    {
        timer = 0f;

        if (enemy.AIPath != null)
            enemy.AIPath.isStopped = true;

        if (enemy.Anim != null)
            enemy.Anim.SetFloat("Speed", 0f);

        if (enemy.FootstepSource != null)
            enemy.FootstepSource.Stop();
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
        timer += deltaTime;

        enemy.DetectPlayer();

        if (timer >= enemy.WaitTimeAtPoint)
        {
            if (enemy.PatrolPoints != null && enemy.PatrolPoints.Length > 0)
                enemy.CurrentPoint = (enemy.CurrentPoint + 1) % enemy.PatrolPoints.Length;

            enemy.ChangeState(enemy.PatrolState);
        }
    }

    public void Exit(EnemyPatrol enemy)
    {
        if (enemy.AIPath != null)
            enemy.AIPath.isStopped = false;
    }
}
