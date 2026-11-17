using UnityEngine.Rendering.Universal;

// PATRON STRATEGY
public class BlinkingCameraBehaviour : ICameraBehaviour
{
    private readonly float interval;
    private float timer;
    private bool isOn = true;
    private Light2D coneLight;

    public BlinkingCameraBehaviour(float interval)
    {
        this.interval = interval;
    }

    public bool IsWatching => isOn;

    public void Begin(CameraConeOscillatingWithPause camera)
    {
        coneLight = camera.GetComponentInChildren<Light2D>();
        SetOn(true);
    }

    public void Tick(CameraConeOscillatingWithPause camera, float deltaTime)
    {
        timer += deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            SetOn(!isOn);
        }
    }

    private void SetOn(bool value)
    {
        isOn = value;
        if (coneLight != null)
            coneLight.enabled = value;
    }
}
