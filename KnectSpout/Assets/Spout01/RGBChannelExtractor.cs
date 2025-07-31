using UnityEngine;

[ExecuteInEditMode]
public class RGBChannelExtractor : MonoBehaviour
{
    [Header("Input")]
    public RenderTexture sourceTexture;

    [Header("Output")]
    [SerializeField] private RenderTexture redChannel;
    [SerializeField] private RenderTexture greenChannel;
    [SerializeField] private RenderTexture blueChannel;

    Material extractChannelMaterial;

    void OnEnable()
    {
        // Load the shader from Resources
        if (extractChannelMaterial == null)
        {
            var shader = Resources.Load<Shader>("ExtractChannel");
            if (shader != null)
                extractChannelMaterial = new Material(shader);
            else
                Debug.LogError("ExtractChannel shader not found. Make sure it's placed in a 'Resources' folder.");
        }

        AllocateBuffers();
    }

    void OnDisable()
    {
        ReleaseBuffers();
    }

    void OnDestroy()
    {
        ReleaseBuffers();
    }

    void AllocateBuffers()
    {
        if (sourceTexture == null) return;

        CreateBuffer(ref redChannel);
        CreateBuffer(ref greenChannel);
        CreateBuffer(ref blueChannel);
    }

    void CreateBuffer(ref RenderTexture buffer)
    {
        if (buffer == null || buffer.width != sourceTexture.width || buffer.height != sourceTexture.height)
        {
            if (buffer != null) buffer.Release();

            buffer = new RenderTexture(sourceTexture.width, sourceTexture.height, 0)
            {
                hideFlags = HideFlags.DontSave
            };
            buffer.Create();
        }
    }

    void ReleaseBuffers()
    {
        if (redChannel != null) redChannel.Release();
        if (greenChannel != null) greenChannel.Release();
        if (blueChannel != null) blueChannel.Release();

        redChannel = greenChannel = blueChannel = null;
    }

    void Update()
    {
        if (sourceTexture == null || extractChannelMaterial == null) return;

        AllocateBuffers();

        // Extract red channel
        ExtractChannel(sourceTexture, redChannel, 0);
        // Extract green channel
        ExtractChannel(sourceTexture, greenChannel, 1);
        // Extract blue channel
        ExtractChannel(sourceTexture, blueChannel, 2);
    }

    void ExtractChannel(RenderTexture source, RenderTexture target, int channel)
    {
        if (extractChannelMaterial != null && target != null)
        {
            extractChannelMaterial.SetFloat("_Channel", channel);
            Graphics.Blit(source, target, extractChannelMaterial);
        }
    }
}
