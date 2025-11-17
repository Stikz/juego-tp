using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Referencias")]
    public Transform legs;
    public Transform body;
    public Transform firePoint;
    public Animator legsAnimator;

    [Header("Referencias")]
    public Transform bodyPivot;
    public Transform legsPivot;
    public Transform bodyGraphics;
    public float legsAimOffsetDegrees = 0f;

    [Header("Ajustes de Apunta")]
    public float bodyRotateLerp = 30f;
    public float legsRotateLerp = 20f;
    public float aimOffsetDegrees = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Camera mainCam;
    private float lastLegsAngle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
    }

    // Vinculado a la acción "Move" (Invoke Unity Events)
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

    void RotateBodyToMouse()
    {
        if (bodyPivot == null) return;

        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 world = mainCam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 0f));
        world.z = 0f;

        Vector2 dir = (Vector2)(world - bodyPivot.position);
        if (dir.sqrMagnitude < 0.0001f) return;

        float target = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + aimOffsetDegrees;

        float smooth = Mathf.LerpAngle(bodyPivot.eulerAngles.z, target, Time.deltaTime * bodyRotateLerp);
        bodyPivot.rotation = Quaternion.Euler(0f, 0f, smooth);

        if (firePoint != null) firePoint.rotation = bodyPivot.rotation;
    }

    private void RotateLegsToMoveDir()
    {
        if (legsPivot == null) return;

        if (moveInput.sqrMagnitude > 0.0001f)
        {
            float target = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + legsAimOffsetDegrees;
            lastLegsAngle = target;
        }

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

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (legsPivot.gameObject.activeSelf != isMoving)
        {
            legsPivot.gameObject.SetActive(isMoving);
        }
    }
}
