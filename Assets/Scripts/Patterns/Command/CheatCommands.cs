using UnityEngine;

// ACA SE APLICA EL PATRON COMMAND (comandos concretos de los cheats)
public class ToggleHelpCommand : ICommand
{
    private readonly CheatManager receiver;
    public ToggleHelpCommand(CheatManager receiver) { this.receiver = receiver; }
    public void Execute() { receiver.ToggleHelp(); }
}

public class ToggleUndetectableCommand : ICommand
{
    private readonly CheatManager receiver;
    public ToggleUndetectableCommand(CheatManager receiver) { this.receiver = receiver; }
    public void Execute() { receiver.ToggleUndetectable(); }
}

public class ToggleSpeedCommand : ICommand
{
    private readonly CheatManager receiver;
    public ToggleSpeedCommand(CheatManager receiver) { this.receiver = receiver; }
    public void Execute() { receiver.ToggleSpeed(); }
}

public class KillAllEnemiesCommand : ICommand
{
    public void Execute()
    {
        var enemies = Object.FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
            enemy.Die();
    }
}
