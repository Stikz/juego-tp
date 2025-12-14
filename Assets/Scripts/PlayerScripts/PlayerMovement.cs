using UnityEngine;
using UnityEngine.InputSystem;
using static PauseManager;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Transform legs;
    public Transform body;
    public Transform firePoint;
    public Animator legsAnimator;
    public Transform bodyPivot;
    public Transform legsPivot;
    public Transform bodyGraphics;
    public float legsAimOffsetDegrees = 0f;

    [Header("Leg rotation")]
    public float bodyRotateLerp = 30f;
    public float legsRotateLerp = 20f;
    public float aimOffsetDegrees = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Camera mainCam;
    private float lastLegsAngle;

    [Header("Aim joystick")]
    public float lookDeadzone = 0.2f;
    private float lastAimAngle;
    private bool usingStickAim = false;

    private Vector2 lastMousePos;
    private bool mouseInitialized = false;

    public float mouseMoveThreshold = 2f; 
    public float switchCooldown = 0.15f;  
    private float lastSwitchTime = -999f;

    [Header("Aim Input Actions")]
    public InputActionReference lookStickAction;  
    public InputActionReference lookMousePosAction;

    [Header("Aim Settings")]
    private Vector2 lookInput;
    private Vector2 mousePos;
    private Vector2 mouseScreen;

    [Header("Crosshair")]
    public Transform crosshair;         
    public float crosshairZ = -1f;        

    private SpriteRenderer crosshairSR;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        if (Mouse.current != null)
        {
            lastMousePos = Mouse.current.position.ReadValue();
            mouseInitialized = true;
        }
        if (crosshair != null)
            crosshairSR = crosshair.GetComponent<SpriteRenderer>();
    }
    void UpdateCrosshair()
    {

        if (crosshair == null) return;
        bool show = !usingStickAim && !GameState.Paused;
        if (crosshairSR != null)
            crosshairSR.enabled = show;
        else
            crosshair.gameObject.SetActive(show);

        if (!show) return;

        float zDist = Mathf.Abs(mainCam.transform.position.z - bodyPivot.position.z);
        Vector3 world = mainCam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, zDist));
        world.z = crosshairZ;

        crosshair.position = world;
    }
    private void OnEnable()
    {
        lookStickAction?.action.Enable();
        lookMousePosAction?.action.Enable();

        lookStickAction.action.performed += OnStickLook;
        lookStickAction.action.canceled += OnStickLook;

        lookMousePosAction.action.performed += OnMousePos;
    }

    private void OnDisable()
    {
        if (lookStickAction != null)
        {
            lookStickAction.action.performed -= OnStickLook;
            lookStickAction.action.canceled -= OnStickLook;
        }

        if (lookMousePosAction != null)
            lookMousePosAction.action.performed -= OnMousePos;
    }

    private void OnStickLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();

        if (lookInput.sqrMagnitude > lookDeadzone * lookDeadzone)
        {
            usingStickAim = true;
            lastAimAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg + aimOffsetDegrees;
        }
    }

    private void OnMousePos(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
        usingStickAim = false;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();

        if (lookInput.sqrMagnitude > lookDeadzone * lookDeadzone)
            usingStickAim = true;
    }

    public void OnAimMouse(InputAction.CallbackContext ctx)
    {
        mouseScreen = ctx.ReadValue<Vector2>();
        usingStickAim = false;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (GameState.Paused) return;

        RotateBodyAim();
        RotateLegsToMoveDir();
        UpdateLegsAnimator();
        HandleLegsVisibility();
    }

    private void FixedUpdate()
    {
        if (GameState.Paused) return;
        if (GameState.Paused) return;
        UpdateCrosshair();
        Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

    }

    void RotateBodyAim()
    {
        if (bodyPivot == null || mainCam == null) return;

        float targetAngle;

        if (usingStickAim)
        {
            if (lookInput.sqrMagnitude > lookDeadzone * lookDeadzone)
            {
                lastAimAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg + aimOffsetDegrees;
            }

            targetAngle = lastAimAngle;
        }
        else
        {
            float zDist = Mathf.Abs(mainCam.transform.position.z - bodyPivot.position.z);
            Vector3 world = mainCam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, zDist));
            world.z = bodyPivot.position.z;

            Vector2 dir = (Vector2)(world - bodyPivot.position);
            if (dir.sqrMagnitude < 0.0001f) return;

            targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + aimOffsetDegrees;
        }

        float smooth = Mathf.LerpAngle(
            bodyPivot.eulerAngles.z,
            targetAngle,
            Time.deltaTime * bodyRotateLerp
        );

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
