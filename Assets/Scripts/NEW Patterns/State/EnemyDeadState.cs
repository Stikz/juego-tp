using UnityEngine;

// ACA SE APLICA EL PATRON STATE (estado concreto: neutralizado)
public class EnemyDeadState : IEnemyState
{
    public void Enter(EnemyPatrol enemy)
    {
        if (enemy.AIPath != null)
        {
            enemy.AIPath.canMove = false;
            enemy.AIPath.isStopped = true;
        }

        Collider2D col = enemy.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (enemy.Anim != null && !string.IsNullOrEmpty(enemy.DeathTriggerName))
            enemy.Anim.SetTrigger(enemy.DeathTriggerName);

        if (enemy.EnemyLight != null)
            enemy.EnemyLight.enabled = false;

        if (enemy.FootstepSource != null)
            enemy.FootstepSource.Stop();
    }

    public void UpdateState(float deltaTime, EnemyPatrol enemy)
    {
    }

    public void Exit(EnemyPatrol enemy)
    {
    }
}
