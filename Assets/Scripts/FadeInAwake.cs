using UnityEngine;

public class FadeInAwake : MonoBehaviour
{
    protected void Awake()
    {
        var fadeInController = FindObjectOfType<FadeInOutController>();
        StartCoroutine(fadeInController.FadeIn());
    }
}
