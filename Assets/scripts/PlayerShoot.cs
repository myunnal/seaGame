using System.Collections;
using UnityEngine;

public class PlayerAimShoot : MonoBehaviour
{
    public Camera cam;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Bullet Speed")]
    public float minBulletSpeed = 20f;
    public float maxBulletSpeed = 80f;

    [Header("Charge Settings")]
    public float maxChargeTime = 1.5f;
    public float maxBulletScale = 4f;

    [Header("Aiming")]
    public LayerMask aimMask = ~0;

    float chargeTimer;
    bool isCharging;

    void Update()
    {
        // Start charging
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            chargeTimer = 0f;
        }

        // Charging
        if (Input.GetMouseButton(0) && isCharging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeTime);
        }

        // Release
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ShootCharged();
            isCharging = false;
        }
    }

    void ShootCharged()
    {
        if (!cam || !firePoint || !bulletPrefab)
            return;

        float chargePercent = chargeTimer / maxChargeTime;

        float bulletSpeed = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, chargePercent);
        float bulletScale = Mathf.Lerp(1.5f, maxBulletScale, chargePercent);

        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f));

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

        // Visual grow
        StartCoroutine(GrowBullet(bullet.transform, bulletScale));

        // Physics
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb)
            rb.velocity = dir * bulletSpeed;

        // Collider size boost
        SphereCollider col = bullet.GetComponent<SphereCollider>();
        if (col)
            col.radius *= bulletScale;
    }

    IEnumerator GrowBullet(Transform t, float targetScale)
    {
        float time = 0f;
        float duration = 0.1f;

        Vector3 start = t.localScale;
        Vector3 end = start * targetScale;

        while (time < duration)
        {
            t.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        t.localScale = end;
    }
}
