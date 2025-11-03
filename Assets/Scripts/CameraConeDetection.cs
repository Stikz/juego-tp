using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraConeOscillatingWithPause : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 30f;      // Velocidad en grados/seg
    public float maxRotationAngle = 90f;   // Desviación máxima desde el ángulo inicial
    public float waitTimeAtEdge = 1f;      // Tiempo que espera en los extremos
    private float currentRotation = 0f;    // Ángulo relativo al inicial
    private bool rotatingRight = true;
    private bool isWaiting = false;
    public float visualRotationOffset = 0f; // por ejemplo, -90 si el sprite mira hacia abajo

    [Header("Detección")]
    public float viewDistance = 5f;
    public float viewAngle = 45f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Debug")]
    public bool showGizmos = true;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogWarning("No se encontró un GameObject con tag Player.");
    }

    private void Update()
    {
        if (!isWaiting)
            RotateOscillating();

        DetectPlayer();
    }

    private void RotateOscillating()
    {
        float delta = rotationSpeed * Time.deltaTime;
        if (!rotatingRight) delta = -delta;

        transform.Rotate(0f, 0f, delta);
        currentRotation += delta;

        // Revisar si llegó a los extremos
        if (currentRotation > maxRotationAngle)
        {
            currentRotation = maxRotationAngle;
            rotatingRight = false;
            StartCoroutine(WaitAtEdge());
        }
        else if (currentRotation < -maxRotationAngle)
        {
            currentRotation = -maxRotationAngle;
            rotatingRight = true;
            StartCoroutine(WaitAtEdge());
        }
    }

    private System.Collections.IEnumerator WaitAtEdge()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtEdge);
        isWaiting = false;
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 forward = Quaternion.Euler(0, 0, visualRotationOffset) * transform.right;
        float angleToPlayer = Vector2.Angle(forward, directionToPlayer);

        if (distanceToPlayer < viewDistance && angleToPlayer < viewAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleMask | playerMask);
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0)
            {
                Debug.Log("¡Jugador detectado! Perdiste");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.yellow;

        // Aplica el mismo offset visual
        Vector3 forward = Quaternion.Euler(0, 0, visualRotationOffset) * transform.right * viewDistance;
        Vector3 leftRay = Quaternion.Euler(0, 0, viewAngle) * forward;
        Vector3 rightRay = Quaternion.Euler(0, 0, -viewAngle) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftRay);
        Gizmos.DrawLine(transform.position, transform.position + rightRay);
        Gizmos.DrawRay(transform.position, forward);
    }
}
