using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static PauseManager;

// PATRON STATE
public class EnemyPatrol : MonoBehaviour
{
    [Header("Type")]
    public EnemyType enemyType;

    [Header("Patrol")]
    public Transform[] patrolPoints;

    [Header("Detection")]
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform raycastOrigin;

    private AIPath aiPath;
    private int currentPoint = 0;
    private Transform player;
    private bool isDead = false;

    [Header("Footstep sound")]
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float stepInterval = 0.4f;
    private float stepTimer = 0f;

    public Animator anim;
    public string deathTriggerName = "Die";
    public Light2D enemyLight;

    private IEnemyState currentState;

    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyWaitState WaitState { get; private set; }
    public EnemyAlertState AlertState { get; private set; }
    public EnemyDeadState DeadState { get; private set; }

    public AIPath AIPath => aiPath;
    public int CurrentPoint { get => currentPoint; set => currentPoint = value; }
    public Transform[] PatrolPoints => patrolPoints;
    public float WaitTimeAtPoint => enemyType.waitTimeAtPoint;
    public float PatrolSpeed => enemyType.patrolSpeed;
    public Animator Anim => anim;
    public string DeathTriggerName => deathTriggerName;
    public Light2D EnemyLight => enemyLight;
    public AudioSource FootstepSource => footstepSource;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        ApplyType();

        PatrolState = new EnemyPatrolState();
        WaitState = new EnemyWaitState();
        AlertState = new EnemyAlertState();
        DeadState = new EnemyDeadState();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (aiPath != null)
        {
            aiPath.canMove = true;
            aiPath.maxSpeed = enemyType.patrolSpeed;
        }

        ChangeState(PatrolState);
    }

    // PATRON TYPE OBJECT
    private void ApplyType()
    {
        if (enemyType == null)
            enemyType = Resources.Load<EnemyType>("EnemyTypes/Guardia");

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (isDead && newState != DeadState) return;

        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    private void Update()
    {
        if (isDead) return;

        if (GameState.Paused)
        {
            if (aiPath != null && !aiPath.isStopped)
                aiPath.isStopped = true;

            if (footstepSource != null)
                footstepSource.Stop();

            if (anim != null)
                anim.SetFloat("Speed", 0f);
            return;
        }

        if (aiPath != null && aiPath.isStopped && currentState == PatrolState)
            aiPath.isStopped = false;

        if (aiPath != null && anim != null)
            anim.SetFloat("Speed", aiPath.desiredVelocity.magnitude);

        currentState?.UpdateState(Time.deltaTime, this);
    }

    public void HandleFootsteps()
    {
        if (footstepSource == null || footstepClip == null || aiPath == null) return;

        float speed = aiPath.desiredVelocity.magnitude;

        if (speed < 0.1f)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;
        float adjustedInterval = stepInterval / (speed / enemyType.patrolSpeed + 0.1f);

        if (stepTimer >= adjustedInterval)
        {
            footstepSource.pitch = Random.Range(0.9f, 1.1f);
            footstepSource.PlayOneShot(footstepClip);
            stepTimer = 0f;
        }
    }

    public void DetectPlayer()
    {
        if (CheatManager.Instance != null && CheatManager.Instance.Undetectable)
            return;
        if (player == null || raycastOrigin == null) return;

        Vector2 dir = (player.position - raycastOrigin.position).normalized;
        float dist = Vector2.Distance(raycastOrigin.position, player.position);
        float angle = Vector2.Angle(raycastOrigin.right, dir);

        if (dist < enemyType.viewDistance && angle < enemyType.viewAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, dir, enemyType.viewDistance, playerMask | obstacleMask);

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0)
                ChangeState(AlertState);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        ChangeState(DeadState);
    }

    private void OnDrawGizmos()
    {
        if (raycastOrigin == null || enemyType == null) return;

        Gizmos.color = Color.red;
        Vector3 forward = raycastOrigin.right * enemyType.viewDistance;
        Vector3 left = Quaternion.Euler(0, 0, enemyType.viewAngle) * forward;
        Vector3 right = Quaternion.Euler(0, 0, -enemyType.viewAngle) * forward;

        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + left);
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + right);
        Gizmos.DrawRay(raycastOrigin.position, forward);
    }
}
