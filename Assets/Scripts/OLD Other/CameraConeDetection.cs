using System.Collections;
using UnityEngine;
using static PauseManager;

public class CameraConeOscillatingWithPause : StealthSensorBase
{
    public float rotationSpeed = 30f;
    public float maxRotationAngle = 90f;
    public float waitTimeAtEdge = 1f;
    public float visualRotationOffset = 0f;
    public bool showGizmos = true;

    private float currentRotation = 0f;
    private bool rotatingRight = true;
    private bool isWaiting = false;

    private void Update()
    {
        if (GameState.Paused) return;

        if (!isWaiting)
            RotateOscillating();

        CheckDetection();
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

    private IEnumerator WaitAtEdge()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtEdge);
        isWaiting = false;
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
