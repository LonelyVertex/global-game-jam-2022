using UnityEngine;
using UnityEngine.Rendering;

public class GroundMaskBlurPrepass : MonoBehaviour
{
    [SerializeField] GroundCheckModel groundCheckModel;

    [Space]
    [SerializeField] Material material;
    [SerializeField] Material tileMaterial;

    [Space]
    [SerializeField] int downsample;

    int maskTextureId = Shader.PropertyToID("_MaskTexture");
    int blurTextureId = Shader.PropertyToID("_BlurTexture");

    private RenderTexture frameTexture;

    void Start()
    {
        RenderPipelineManager.beginFrameRendering += HandleRenderPipelineManagerBeginFrameRendering;
        RenderPipelineManager.endFrameRendering += HandleRenderPipelineManagerEndFrameRendering;
    }

    void OnDestroy()
    {
        RenderPipelineManager.beginFrameRendering -= HandleRenderPipelineManagerBeginFrameRendering;
        RenderPipelineManager.endFrameRendering -= HandleRenderPipelineManagerEndFrameRendering;
    }

    private void HandleRenderPipelineManagerBeginFrameRendering(ScriptableRenderContext ctx, Camera[] cameras)
    {
        var texture = groundCheckModel.groundCheckTexture;

        frameTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0);

        Blur(texture, frameTexture);

        tileMaterial.SetTexture(blurTextureId, frameTexture);
    }

    private void HandleRenderPipelineManagerEndFrameRendering(ScriptableRenderContext ctx, Camera[] cameras)
    {
        RenderTexture.ReleaseTemporary(frameTexture);
    }

    private void Blur(Texture2D initTexture, RenderTexture targetTexture)
    {
        var rts = new RenderTexture[downsample];

        for (var i = 0; i < downsample; i++) {
            rts[i] = RenderTexture.GetTemporary(initTexture.width >> (i + 1), initTexture.height >> (i + 1), 0);
        }

        Graphics.Blit(initTexture, rts[0], material, 0);
        for (var i = 1; i < downsample; i++) {
            Graphics.Blit(rts[i - 1], rts[i], material, 0);
        }

        for (var i = downsample - 1; i >= 1; i--) {
            Graphics.Blit(rts[i], rts[i - 1], material, 1);
        }
        Graphics.Blit(rts[0], targetTexture, material, 1);

        for (var i = 0; i < downsample; i++) {
            RenderTexture.ReleaseTemporary(rts[i]);
        }
    }
}
