using UnityEngine;

public class PlayerAimShoot : MonoBehaviour
{
    public Camera cam;            
    public Transform firePoint;     
    public GameObject bulletPrefab;

    [Header("Shooting")]
    public float bulletSpeed = 40f;
    public LayerMask aimMask = ~0; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //shoots if player presses left mouse button
            Shoot();
    }

    void Shoot()
    {
        if (cam == null || firePoint == null || bulletPrefab == null)
            return;

        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
            targetPoint = hit.point;              
        else
            targetPoint = ray.GetPoint(1000f);    

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
