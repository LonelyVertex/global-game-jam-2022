using UnityEngine;

public class FadeInAwake : MonoBehaviour
{
    protected void Awake()
    {
        var fadeInController = FindObjectOfType<FadeInOutController>();
        if (fadeInController != null) {
            StartCoroutine(fadeInController.FadeIn());
        }
    }
}
