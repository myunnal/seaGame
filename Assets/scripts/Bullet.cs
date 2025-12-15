using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;

    [Header("Damage")]
    public bool isCharged = false;
    public int normalDamage = 1;
    public int chargedDamage = 6;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public int GetDamage()
    {
        return isCharged ? chargedDamage : normalDamage;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
