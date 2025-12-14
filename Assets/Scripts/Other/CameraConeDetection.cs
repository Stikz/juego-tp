
using UnityEngine;
using UnityEngine.SceneManagement;
using static PauseManager;

public class CameraConeOscillatingWithPause : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 30f;      
    public float maxRotationAngle = 90f;   
    public float waitTimeAtEdge = 1f;      
    private float currentRotation = 0f;    
    private bool rotatingRight = true;
    private bool isWaiting = false;
    public float visualRotationOffset = 0f;

    [Header("Detection")]
    public float viewDistance = 5f;
    public float viewAngle = 45f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Debug")]
    public bool showGizmos = true;

    public GameOverManager gameOverManager;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

    }

    private void Update()
    {
        if (GameState.Paused) return;
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
        if (CheatManager.Instance != null && CheatManager.Instance.Undetectable)
            return;
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
                gameOverManager.ShowGameOver();
                this.enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.yellow;

        Vector3 forward = Quaternion.Euler(0, 0, visualRotationOffset) * transform.right * viewDistance;
        Vector3 leftRay = Quaternion.Euler(0, 0, viewAngle) * forward;
        Vector3 rightRay = Quaternion.Euler(0, 0, -viewAngle) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftRay);
        Gizmos.DrawLine(transform.position, transform.position + rightRay);
        Gizmos.DrawRay(transform.position, forward);
    }
}