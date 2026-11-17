// PATRON STRATEGY
public class RotatingCameraBehaviour : ICameraBehaviour
{
    private float currentRotation;
    private bool rotatingRight = true;
    private float waitTimer;

    public bool IsWatching => true;

    public void Begin(CameraConeOscillatingWithPause camera)
    {
    }

    public void Tick(CameraConeOscillatingWithPause camera, float deltaTime)
    {
        if (waitTimer > 0f)
        {
            waitTimer -= deltaTime;
            return;
        }

        float delta = camera.rotationSpeed * deltaTime;
        if (!rotatingRight) delta = -delta;

        camera.transform.Rotate(0f, 0f, delta);
        currentRotation += delta;

        if (currentRotation > camera.maxRotationAngle)
        {
            currentRotation = camera.maxRotationAngle;
            rotatingRight = false;
            waitTimer = camera.waitTimeAtEdge;
        }
        else if (currentRotation < -camera.maxRotationAngle)
        {
            currentRotation = -camera.maxRotationAngle;
            rotatingRight = true;
            waitTimer = camera.waitTimeAtEdge;
        }
    }
}
