using UnityEngine;
using Pathfinding;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 2f;

    [Header("Investigación")]
    public float investigateDuration = 3f;

    [Header("Velocidades")]
    public float patrolSpeed = 2f;

    [Header("Mirar alrededor")]
    public float lookAroundAngle = 90f;
    public float snapInterval = 0.5f;

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

    [Header("Game Over")]
    public GameOverManager gameOverManager;

    [Header("Footstep sound")]
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float stepInterval = 0.4f;
    private float stepTimer = 0f;

    public Animator anim;
    public string deathTriggerName = "Die";
    public Light2D enemyLight;

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

        float currentSpeed = aiPath.desiredVelocity.magnitude;
        anim.SetFloat("Speed", currentSpeed);

        if (!isWaiting)
        {
            Patrol();
        }

        HandleFootsteps();
        DetectPlayer();
    }

    private void HandleFootsteps()
    {
        if (footstepSource == null || footstepClip == null) return;

        float speed = aiPath.desiredVelocity.magnitude;

        if (speed < 0.1f || isInvestigating)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;

        float adjustedInterval = stepInterval / (speed / patrolSpeed + 0.1f);

        if (stepTimer >= adjustedInterval)
        {
            footstepSource.PlayOneShot(footstepClip);
            stepTimer = 0f;
        }
    }

    private void Patrol()
    {
        aiPath.rotationSpeed = originalRotationSpeed;

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
                gameOverManager.ShowGameOver();
                aiPath.canMove = false;
                this.enabled = false;
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

        if (anim != null)
        {
            anim.SetTrigger(deathTriggerName);
        }
        if (enemyLight != null)
            enemyLight.enabled = false;
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
