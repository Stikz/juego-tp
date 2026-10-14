/// <summary>
/// Patron State: Interfaz base que define las operaciones que cada estado concreto del enemigo debe implementar.
/// (Basado en la Diapositiva 3 y 10 de la presentacion State de la catedra UADE).
/// </summary>
public interface IEnemyState
{
    void Enter(EnemyPatrol enemy);
    void UpdateState(float deltaTime, EnemyPatrol enemy);
    void Exit(EnemyPatrol enemy);
}
