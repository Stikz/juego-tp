using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Jugador

    [Header("Dead Zone")]
    public Vector2 deadZoneSize = new Vector2(2f, 2f);

    [Header("Suavizado")]
    public float smoothTime = 0.2f;


    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 delta = target.position - transform.position;

        // Calculamos cuánto nos salimos de la dead zone
        float deltaX = 0f;
        if (Mathf.Abs(delta.x) > deadZoneSize.x / 2f)
            deltaX = delta.x - Mathf.Sign(delta.x) * deadZoneSize.x / 2f;

        float deltaY = 0f;
        if (Mathf.Abs(delta.y) > deadZoneSize.y / 2f)
            deltaY = delta.y - Mathf.Sign(delta.y) * deadZoneSize.y / 2f;

        Vector3 targetPos = transform.position + new Vector3(deltaX, deltaY, 0f);

        // Suavizado
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        transform.position = smoothPos;
    }
}
