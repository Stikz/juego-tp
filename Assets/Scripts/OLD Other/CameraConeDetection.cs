using UnityEngine;
using static PauseManager;

public enum CameraMode { Giratoria, FijaIntermitente }

public class CameraConeOscillatingWithPause : StealthSensorBase
{
    public CameraMode mode = CameraMode.Giratoria;
    public float rotationSpeed = 30f;
    public float maxRotationAngle = 90f;
    public float waitTimeAtEdge = 1f;
    public float blinkInterval = 4f;
    public float visualRotationOffset = 0f;
    public bool showGizmos = true;

    private ICameraBehaviour behaviour;

    protected override void Start()
    {
        base.Start();

        // PATRON STRATEGY
        if (mode == CameraMode.FijaIntermitente)
            behaviour = new BlinkingCameraBehaviour(blinkInterval);
        else
            behaviour = new RotatingCameraBehaviour();

        behaviour.Begin(this);
    }

    private void Update()
    {
        if (GameState.Paused) return;

        behaviour.Tick(this, Time.deltaTime);

        if (behaviour.IsWatching)
            CheckDetection();
    }

    protected override Vector2 GetSensorOrigin()
    {
        return transform.position;
    }

    protected override Vector2 GetSensorForward()
    {
        return Quaternion.Euler(0, 0, visualRotationOffset) * transform.right;
    }

    protected override void OnTargetDetected()
    {
        base.OnTargetDetected();
        enabled = false;
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
