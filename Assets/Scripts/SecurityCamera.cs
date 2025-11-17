using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Alerta a enemigos")]
    public float alertRadius = 7f;
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }

}
