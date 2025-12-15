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

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        if (kb == null) return;

        if (kb.f1Key.wasPressedThisFrame) ToggleHelp();
        if (kb.f2Key.wasPressedThisFrame) ToggleUndetectable();
        if (kb.f3Key.wasPressedThisFrame) ToggleSpeed();
        if (kb.f4Key.wasPressedThisFrame) KillAllEnemies();
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

        if (cachedPlayer != null)
        {
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

    public void KillAllEnemies()
    {
        var enemies = FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None);
        int killed = 0;

        foreach (var e in enemies)
        {
            if (e == null) continue;
            e.Die();
            killed++;
        }

    }
}
