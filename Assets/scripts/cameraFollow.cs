using UnityEngine;

public class CameraUnderwaterFollow : MonoBehaviour
{
    public Transform target;
    public PlayerMovementUnderwater controller;

    [Header("Camera Settings")]
    public float distance = 6f;
    public float height = 1.5f;
    public float smooth = 10f;

    [Header("Collision")]
    public LayerMask collisionMask;
    public float collisionRadius = 0.3f;

    void LateUpdate()
    {
        if (!target || !controller)
            return;

        Follow();
    }

    void Follow()
    {
        float yaw = controller.GetYaw();
        float pitch = controller.GetPitch();

        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPos =
            target.position
            - camRot * Vector3.forward * distance
            + Vector3.up * height;

        if (Physics.SphereCast(target.position, collisionRadius,
            (desiredPos - target.position).normalized,
            out RaycastHit hit,
            distance,
            collisionMask))
        {
            desiredPos = hit.point + hit.normal * 0.3f;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smooth);
        transform.rotation = camRot;
    }
}
