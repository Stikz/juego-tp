// ACA SE APLICA EL PATRON STATE (interfaz de estado)
public interface IEnemyState
{
    void Enter(EnemyPatrol enemy);
    void UpdateState(float deltaTime, EnemyPatrol enemy);
    void Exit(EnemyPatrol enemy);
}
