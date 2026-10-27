using System;

// ACA SE APLICA EL PATRON OBSERVER (subject con eventos: los emisores no conocen a los que escuchan)
public static class GameEvents
{
    public static event Action OnPlayerDetected;
    public static event Action OnVictory;

    public static void TriggerPlayerDetected()
    {
        OnPlayerDetected?.Invoke();
    }

    public static void TriggerVictory()
    {
        OnVictory?.Invoke();
    }
}
