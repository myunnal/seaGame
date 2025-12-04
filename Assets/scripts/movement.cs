using UnityEngine;

public class SwimMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 4f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        float y = 0f;

        if (Input.GetKey(KeyCode.Space))
            y = 1f;
        else if (Input.GetKey(KeyCode.LeftControl))
            y = -1f;

        Vector3 input = new Vector3(x, y, z);

        // Move relative to camera direction
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        Vector3 camUp = Vector3.up;

        Vector3 move =
            camRight * input.x +
            camForward * input.z +
            camUp * input.y;

        transform.position += move.normalized * moveSpeed * Time.deltaTime;
    }
}
