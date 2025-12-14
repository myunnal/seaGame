using UnityEngine;

public class UnderwaterPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 8f;

    [Header("Vertical Swimming")]
    public KeyCode ascendKey = KeyCode.Space;
    public KeyCode descendKey = KeyCode.LeftShift;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    public float cameraDistance = 5f;
    public float cameraHeight = 1f;
    public float cameraSmooth = 10f;

    private Vector3 currentVelocity;
    private float yaw;   // Horizontal rotation
    private float pitch; // Vertical camera rotation

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Toggle cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool locked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = locked ? true : false;
        }

        HandleMouseLook();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void LateUpdate()
    {
        HandleCameraFollow();
    }

    // --------------------------------------------
    // CAMERA ROTATION
    // --------------------------------------------

    void HandleMouseLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

        // Rotate player horizontally only
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    // --------------------------------------------
    // PLAYER MOVEMENT
    // --------------------------------------------

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float f = Input.GetAxisRaw("Vertical");

        float v = 0f;

        if (Input.GetKey(ascendKey))
            v += 1f;
        if (Input.GetKey(descendKey))
            v -= 1f;

        Vector3 moveDir =
            transform.right * h +
            transform.forward * f +
            Vector3.up * v;

        moveDir = moveDir.normalized;

        Vector3 targetVel = moveDir * moveSpeed;

        if (moveDir.magnitude > 0.1f)
            currentVelocity = Vector3.Lerp(currentVelocity, targetVel, acceleration * Time.fixedDeltaTime);
        else
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);

        rb.velocity = currentVelocity;
    }

    // --------------------------------------------
    // CAMERA FOLLOW
    // --------------------------------------------

    void HandleCameraFollow()
    {
        if (cameraTransform == null)
            return;

        // Camera rotation is independent from player rotation!
        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPos =
            transform.position
            - camRot * Vector3.forward * cameraDistance
            + Vector3.up * cameraHeight;

        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            desiredPos,
            Time.deltaTime * cameraSmooth
        );

        cameraTransform.rotation = camRot;
    }
}
