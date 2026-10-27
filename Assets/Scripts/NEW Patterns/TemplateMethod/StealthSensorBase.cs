using UnityEngine;

public abstract class StealthSensorBase : MonoBehaviour
{
    [Header("Detection Settings")]
    public float viewDistance = 5f;
    public float viewAngle = 45f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    protected Transform targetPlayer;

    protected virtual void Start()
    {
        FindTarget();
    }

    protected void FindTarget()
    {
        if (targetPlayer != null) return;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) targetPlayer = p.transform;
    }

    public bool CheckDetection()
    {
        if (CheatManager.Instance != null && CheatManager.Instance.Undetectable) return false;

        if (targetPlayer == null)
        {
            FindTarget();
            if (targetPlayer == null) return false;
        }

        Vector2 origin = GetSensorOrigin();
        Vector2 targetPos = targetPlayer.position;
        Vector2 dirToTarget = (targetPos - origin).normalized;

        if (Vector2.Distance(origin, targetPos) > viewDistance) return false;
        if (Vector2.Angle(GetSensorForward(), dirToTarget) > viewAngle) return false;

        RaycastHit2D hit = Physics2D.Raycast(origin, dirToTarget, viewDistance, obstacleMask | playerMask);
        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerMask) != 0)
        {
            OnTargetDetected();
            return true;
        }

        return false;
    }

    protected abstract Vector2 GetSensorOrigin();
    protected abstract Vector2 GetSensorForward();

    protected virtual void OnTargetDetected()
    {
        // ACA SE APLICA EL PATRON OBSERVER (el sensor avisa, no conoce al GameOverManager)
        GameEvents.TriggerPlayerDetected();
    }
}
