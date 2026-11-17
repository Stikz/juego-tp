using System;

// PATRON OBSERVER
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
