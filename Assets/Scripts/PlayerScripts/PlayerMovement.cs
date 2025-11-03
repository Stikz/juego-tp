using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Referencias")]
    public Transform legs;       // Asigná el hijo "Legs"
    public Transform body;       // Asigná el hijo "Body"
    public Transform firePoint;  // Asigná el hijo "FirePoint" dentro de Body
    public Animator legsAnimator; // Animator de las piernas (opcional, para caminar)

    [Header("Referencias")]
    public Transform bodyPivot;   // asigná BodyPivot (pivote en hombro)
    public Transform legsPivot;         // asigná "LegsPivot"
    public Transform bodyGraphics; // opcional: si corregís el arte en el editor
    public float legsAimOffsetDegrees = 0f; // poné -90 si tu sprite mira hacia abajo por defecto

    [Header("Ajustes de Apunta")]
    public float bodyRotateLerp = 30f;
    public float legsRotateLerp = 20f;  // suavizado rotación piernas
    public float aimOffsetDegrees = 0f; // poné -90 si tu sprite mira hacia abajo por defecto

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Camera mainCam;
    private float lastLegsAngle; // mantiene la orientación de piernas cuando estás quieto



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    // Input System: acción "Move" (Vector2)
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void Update()
    {
        RotateBodyToMouse();
        RotateLegsToMoveDir();
        UpdateLegsAnimator();
        HandleLegsVisibility();
    }

    private void FixedUpdate()
    {
        Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    // --- ROTACIÓN DEL TORSO (hacia mouse) ---
    void RotateBodyToMouse()
    {
        if (bodyPivot == null) return;

        // 1) Mouse -> mundo (en 2D ortográfico el z no importa, pero lo limpiamos igual)
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 world = mainCam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 0f));
        world.z = 0f;

        // 2) Dirección desde el pivote (hombro) hacia el mouse
        Vector2 dir = (Vector2)(world - bodyPivot.position);
        if (dir.sqrMagnitude < 0.0001f) return;

        // 3) Ángulo destino usando +X como "frente"
        float target = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + aimOffsetDegrees;

        // 4) Suavizar y aplicar
        float smooth = Mathf.LerpAngle(bodyPivot.eulerAngles.z, target, Time.deltaTime * bodyRotateLerp);
        bodyPivot.rotation = Quaternion.Euler(0f, 0f, smooth);

        // 5) Asegurar el FirePoint alineado al pivot (si no necesitás desfase)
        if (firePoint != null) firePoint.rotation = bodyPivot.rotation;
    }

    // --- ROTACIÓN DE PIERNAS (según dirección de movimiento) ---
    private void RotateLegsToMoveDir()
    {
        if (legsPivot == null) return;

        // Si te estás moviendo, actualizamos la dirección objetivo
        if (moveInput.sqrMagnitude > 0.0001f)
        {
            float target = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + legsAimOffsetDegrees;
            lastLegsAngle = target;
        }

        // Suavizado hacia la última dirección válida (así en idle no "salta")
        float smooth = Mathf.LerpAngle(legsPivot.eulerAngles.z, lastLegsAngle, Time.deltaTime * legsRotateLerp);
        legsPivot.rotation = Quaternion.Euler(0f, 0f, smooth);
    }

    private void UpdateLegsAnimator()
    {
        if (legsAnimator == null) return;
        float speed = moveInput.magnitude;
        legsAnimator.SetFloat("Speed", speed);

    }
    private void HandleLegsVisibility()
    {
        if (legsPivot == null) return;

        // Si te estás moviendo (con un pequeño umbral para evitar parpadeos)
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        // Activar o desactivar el GameObject de las piernas
        if (legsPivot.gameObject.activeSelf != isMoving)
        {
            legsPivot.gameObject.SetActive(isMoving);
        }
    }
}
