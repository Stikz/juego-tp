using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [SerializeField] private GameObject cheatsPanel;
    [SerializeField] private TMP_Text cheatsText;
    [SerializeField] private float speedMultiplier = 2f;

    public bool Undetectable { get; private set; }
    public bool SpeedBoost { get; private set; }

    private PlayerMovement cachedPlayer;
    private float baseMoveSpeed;

    private Dictionary<Key, ICommand> commands;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ACA SE APLICA EL PATRON COMMAND (invoker: cada tecla tiene asociado un comando)
        commands = new Dictionary<Key, ICommand>
        {
            { Key.F1, new ToggleHelpCommand(this) },
            { Key.F2, new ToggleUndetectableCommand(this) },
            { Key.F3, new ToggleSpeedCommand(this) },
            { Key.F4, new KillAllEnemiesCommand() }
        };
    }

    private void Start()
    {
        if (cheatsPanel) cheatsPanel.SetActive(false);

        if (cheatsText)
        {
            cheatsText.text =
                "CHEATS\n" +
                "F1 - Mostrar/Ocultar ayuda\n" +
                "F2 - Invincible (Indetectable)\n" +
                "F3 - Speed (x2)\n" +
                "F4 - Kill All Enemies\n";
        }
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || commands == null) return;

        foreach (var pair in commands)
        {
            if (kb[pair.Key].wasPressedThisFrame)
                pair.Value.Execute();
        }
    }

    public void ToggleHelp()
    {
        if (!cheatsPanel) return;
        cheatsPanel.SetActive(!cheatsPanel.activeSelf);
    }

    public void ToggleUndetectable()
    {
        Undetectable = !Undetectable;
    }

    public void ToggleSpeed()
    {
        SpeedBoost = !SpeedBoost;

        if (cachedPlayer == null) cachedPlayer = FindFirstObjectByType<PlayerMovement>();
        if (cachedPlayer == null) return;

        if (SpeedBoost)
        {
            baseMoveSpeed = cachedPlayer.moveSpeed;
            cachedPlayer.moveSpeed = baseMoveSpeed * speedMultiplier;
        }
        else
        {
            cachedPlayer.moveSpeed = baseMoveSpeed;
        }
    }
}
