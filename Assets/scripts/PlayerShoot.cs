using UnityEngine;

public class PlayerAimShoot : MonoBehaviour
{
    [Header("References")]
    public Camera cam;              // your main camera
    public Transform firePoint;     // where the bullet spawns
    public GameObject bulletPrefab;

    [Header("Shooting")]
    public float bulletSpeed = 40f;
    public LayerMask aimMask = ~0;  // what the ray can hit (default: everything)

    void Update()
    {
        // shoot when left mouse is pressed
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        if (cam == null || firePoint == null || bulletPrefab == null)
            return;

        // ray from the center of the screen
        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
            targetPoint = hit.point;                 // hit something
        else
            targetPoint = ray.GetPoint(1000f);       // just far in that direction

        Vector3 dir = (targetPoint - firePoint.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(dir)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = dir * bulletSpeed;
    }
}
