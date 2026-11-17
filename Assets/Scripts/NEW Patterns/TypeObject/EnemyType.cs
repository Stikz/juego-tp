using UnityEngine;

// PATRON TYPE OBJECT
[CreateAssetMenu(menuName = "Enemigos/Tipo de guardia")]
public class EnemyType : ScriptableObject
{
    public string typeName = "Guardia";
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 2f;
    public float viewDistance = 4f;
    public float viewAngle = 35f;
}
