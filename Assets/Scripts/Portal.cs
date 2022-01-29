using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnPortalEnter();
            DestroyPortal();
        }
    }

    void DestroyPortal()
    {
        Destroy(gameObject);
    }
}
