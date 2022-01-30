using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] ParticleSystem portalParticles;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color activeColor;
    [SerializeField] Color disabledColor;
    [SerializeField] Collider2D portalCollider;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnPortalEnter();

            StartCoroutine(DestroyPortal());
        }
    }

    IEnumerator DestroyPortal()
    {
        portalCollider.enabled = false;
        portalParticles.Stop();

        yield return new WaitForSeconds(0.2f);

        for (var d = 0.0f; d < 0.5f; d += Time.deltaTime) {
            spriteRenderer.color = Color.Lerp(activeColor, disabledColor, d / 0.5f);
            yield return null;
        }
    }
}
