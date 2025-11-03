using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemyDestinationSetter : MonoBehaviour
{
    private AIPath aiPath;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
    }

    public void SetDestination(Vector3 target)
    {
        aiPath.destination = target;
        aiPath.canMove = true;
    }
}
