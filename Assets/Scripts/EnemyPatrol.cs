using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;

    [Header("Investigación")]
    public float investigateDuration = 3f;

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float investigateSpeed = 4f;

    [Header("Mirar alrededor")]
    public float lookAroundAngle = 90f;
    public float snapInterval = 0.5f;  // tiempo entre snaps

    [Header("Detección")]
    public float viewDistance = 5f;
    public float viewAngle = 45f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform raycastOrigin;

    [Header("Pathfinding")]
    private AIPath aiPath;
    private Seeker seeker;

    private int currentPoint = 0;
    private bool isWaiting = false;
    private bool isInvestigating = false;
    private Vector3 investigateTarget;
    private Transform player;
    private bool isDead = false;
    public Animator animator; 

    private Coroutine lookAroundCoroutine;
    private float originalRotationSpeed;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        if (aiPath == null) Debug.LogError("Falta AIPath en el enemigo");
        if (seeker == null) Debug.LogError("Falta Seeker en el enemigo");
    }

    private void Start()
    {
        if (patrolPoints.Length == 0) return;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        aiPath.canMove = true;
        aiPath.destination = patrolPoints[currentPoint].position;
        aiPath.maxSpeed = patrolSpeed;
        originalRotationSpeed = aiPath.rotationSpeed;
    }

    private void Update()
    {
        if (isDead) return;

        // Ajustar velocidad según estado
        aiPath.maxSpeed = isInvestigating ? investigateSpeed : patrolSpeed;

        if (isInvestigating)
        {
            aiPath.destination = investigateTarget;

            // Llegó a investigar
            if (!aiPath.pathPending && Vector2.Distance(transform.position, investigateTarget) < 0.1f)
            {
                if (lookAroundCoroutine == null)
                    lookAroundCoroutine = StartCoroutine(LookAroundCoroutine());
            }
        }
        else if (!isWaiting)
        {
            Patrol();
        }
        UpdateAnimation();
        DetectPlayer();
    }
    private void UpdateAnimation()
    {
        if (animator == null || aiPath == null) return;

        float speed = aiPath.desiredVelocity.magnitude;

        animator.SetFloat("Speed", speed);
    }

    private void Patrol()
    {
        aiPath.rotationSpeed = originalRotationSpeed; // restaurar giro automático en patrulla
        if (aiPath.destination != patrolPoints[currentPoint].position)
            aiPath.destination = patrolPoints[currentPoint].position;

        if (Vector2.Distance(transform.position, patrolPoints[currentPoint].position) < 0.1f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
        aiPath.destination = patrolPoints[currentPoint].position;
        isWaiting = false;
    }

    private IEnumerator LookAroundCoroutine()
    {
        aiPath.canMove = false;
        aiPath.rotationSpeed = 0f; // desactivar giro automático

        Quaternion originalRotation = transform.rotation;
        float elapsed = 0f;
        float snapInterval = 0.5f; // tiempo entre snaps
        float[] angles = new float[] { 0f, 90f, 180f, 270f }; // <-- array de floats

        int index = 0;

        while (elapsed < investigateDuration)
        {
            transform.rotation = originalRotation * Quaternion.Euler(0, 0, angles[index]);
            index = (index + 1) % angles.Length;

            float timer = 0f;
            while (timer < snapInterval)
            {
                timer += Time.deltaTime;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        transform.rotation = originalRotation;
        isInvestigating = false;
        aiPath.canMove = true;
        aiPath.destination = patrolPoints[currentPoint].position;
        lookAroundCoroutine = null;
    }



    public void Investigate(Vector3 position)
    {
        investigateTarget = position;
        isInvestigating = true;
        isWaiting = false;
        aiPath.canMove = true;

        // Detener coroutine anterior si existía
        if (lookAroundCoroutine != null)
        {
            StopCoroutine(lookAroundCoroutine);
            lookAroundCoroutine = null;
        }
    }

    private void DetectPlayer()
    {
        if (player == null || raycastOrigin == null) return;

        Vector2 dir = (player.position - raycastOrigin.position).normalized;
        float dist = Vector2.Distance(raycastOrigin.position, player.position);
        float angle = Vector2.Angle(raycastOrigin.right, dir);

        if (dist < viewDistance && angle < viewAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, dir, viewDistance, playerMask | obstacleMask);
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (lookAroundCoroutine != null)
            StopCoroutine(lookAroundCoroutine);

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) script.enabled = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (aiPath != null)
            aiPath.canMove = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Destroy(gameObject, 0.2f);
    }

    private void OnDrawGizmos()
    {
        if (raycastOrigin == null) return;

        Gizmos.color = Color.red;
        Vector3 forward = raycastOrigin.right * viewDistance;
        Vector3 left = Quaternion.Euler(0, 0, viewAngle) * forward;
        Vector3 right = Quaternion.Euler(0, 0, -viewAngle) * forward;

        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + left);
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + right);
        Gizmos.DrawRay(raycastOrigin.position, forward);
    }
}
