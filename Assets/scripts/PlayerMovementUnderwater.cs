using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementUnderwater : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float verticalSpeed = 4f;
    public float acceleration = 12f;
    public float deceleration = 10f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private Rigidbody rb;
    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mx;
        pitch -= my;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float f = Input.GetAxisRaw("Vertical"); 
        float v = 0f;
        if (Input.GetKey(KeyCode.Space)) v += 1f; //makes the medusa go up
        if (Input.GetKey(KeyCode.LeftShift)) v -= 1f; //makes the medusa go down

        Vector3 moveDir =
            transform.forward * f +
            transform.right * h +
            Vector3.up * v;

        moveDir = moveDir.normalized;

        Vector3 targetVel =
            transform.forward * f * moveSpeed +
            transform.right * h * moveSpeed +
            Vector3.up * v * verticalSpeed;

        if (moveDir.magnitude > 0.1f)
            currentVelocity = Vector3.Lerp(currentVelocity, targetVel, acceleration * Time.fixedDeltaTime);
        else
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);

        rb.velocity = currentVelocity;
    }

    public float GetPitch() => pitch;
    public float GetYaw() => yaw;
}
