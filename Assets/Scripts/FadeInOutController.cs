using System.Collections;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{
    [SerializeField] Material fadeMaterial;
    [SerializeField] GameObject fadeGo;

    [Space]
    [SerializeField] float fadeDuration = 0.5f;

    [Space]
    [SerializeField] PlayerType playerType = PlayerType.Light;

    public IEnumerator FadeOut(PlayerType playerType, Vector2 position)
    {
        this.playerType = playerType;

        fadeGo.SetActive(true);

        fadeMaterial.SetFloat("_MaxRadius", CalculateMaxRadius(position));
        fadeMaterial.SetColor("_Color", playerType.ToVisulaColor());
        fadeMaterial.SetVector("_Position", position);

        fadeMaterial.SetFloat("_Invert", 0.0f);
        for (var d = 0.0f; d < fadeDuration; d += Time.deltaTime) {
            var t = (d / fadeDuration);

            fadeMaterial.SetFloat("_T", t);
            yield return null;
        }
        fadeMaterial.SetFloat("_T", 1.0f);
    }

    public IEnumerator FadeIn()
    {
        fadeGo.SetActive(true);

        fadeMaterial.SetFloat("_MaxRadius", CalculateMaxRadius(Vector2.zero));
        fadeMaterial.SetColor("_Color", playerType.ToVisulaColor());
        fadeMaterial.SetVector("_Position", Vector2.zero);

        fadeMaterial.SetFloat("_Invert", 1.0f);
        for (var d = 0.0f; d < fadeDuration; d += Time.deltaTime) {
            var t = Easing(d / fadeDuration);

            fadeMaterial.SetFloat("_T", t);
            yield return null;
        }
        fadeMaterial.SetFloat("_T", 1.0f);

        fadeGo.SetActive(false);
    }

    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private float CalculateMaxRadius(Vector2 position) => Vector2.Distance(new Vector2(-16, -16), new Vector2(Mathf.Abs(position.x), Mathf.Abs(position.y)));

    private float Easing(float t) => t == 0.0f ? 0 : Mathf.Pow(2, 10 * t - 10);
}
