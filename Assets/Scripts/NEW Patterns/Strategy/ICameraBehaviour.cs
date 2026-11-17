// PATRON STRATEGY
public interface ICameraBehaviour
{
    bool IsWatching { get; }
    void Begin(CameraConeOscillatingWithPause camera);
    void Tick(CameraConeOscillatingWithPause camera, float deltaTime);
}
