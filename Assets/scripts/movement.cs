using UnityEngine;

public class UnderwaterPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 8f;

    public float mouseSensitivity = 2f;
    public float rotationSpeed = 5f;

    public KeyCode ascendKey = KeyCode.Space; // Rise up
    public KeyCode descendKey = KeyCode.LeftControl; // Go down

    public Transform cameraTransform;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    private Vector3 currentVelocity;
    private float verticalRotation = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set up rigidbody for smooth movement
        if (rb != null)
        {
            rb.useGravity = false;
            rb.drag = 1f;
            rb.angularDrag = 5f;
        }

        // If camera not assigned, try to find main camera
        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
        }
    }

    void Update()
    {
        HandleMouseLook();

        // Toggle cursor lock with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally (Y-axis)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically (X-axis) with clamping
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }

    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float forward = Input.GetAxisRaw("Vertical");      // W/S or Up/Down
        float vertical = 0f;

        // Vertical movement (up/down in world space)
        if (Input.GetKey(ascendKey))
            vertical = 1f;
        else if (Input.GetKey(descendKey))
            vertical = -1f;

        // Check for sprint
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Calculate movement direction relative to player's forward
        Vector3 moveDirection = transform.right * horizontal + transform.forward * forward;

        // Add vertical movement in world space
        moveDirection += Vector3.up * vertical;

        // Normalize to prevent faster diagonal movement
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // Calculate target velocity
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // Smoothly interpolate current velocity
        if (moveDirection.magnitude > 0.1f)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // Apply movement
        if (rb != null)
        {
            rb.velocity = currentVelocity;
        }
        else
        {
            transform.position += currentVelocity * Time.fixedDeltaTime;
        }
    }
}