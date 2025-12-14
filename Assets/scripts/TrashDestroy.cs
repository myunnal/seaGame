using UnityEngine;

public class TrashDestroy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);     
            Destroy(other.gameObject); 
        }
    }
}