using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using YoloV4Tiny;
using System.Linq;

sealed class VisualizerYolo : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] RenderTexture texture;
    [SerializeField, Range(0, 1)] float _threshold = 0.5f;
    [SerializeField] ResourceSet _resources = null;
    public Vector4[] Data;
    public Material mat;

    #endregion

    #region Internal objects

    ObjectDetector _detector;
    ComputeBuffer _drawArgs;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _detector = new ObjectDetector(_resources);
        _drawArgs = new ComputeBuffer(4, sizeof(uint), ComputeBufferType.IndirectArguments);
        _drawArgs.SetData(new[] { 6, 0, 0, 0 });
       
    }

    void OnDisable()
    {
        _detector.Dispose();
        _drawArgs.Dispose();
    }

    void Update()
    {
        _detector.ProcessImage(texture, _threshold);
        var detections = _detector.Detections.ToArray();
        int detectionCount = detections.Length;
        Data = new Vector4[10];


        for (int i = 0; i < Mathf.Clamp(detections.Length, 0, 10); i++)
        {
            Data[i] = new Vector4(detections[i].x-detections[i].w*0.5f, detections[i].x+detections[i].w*0.5f,1f-(detections[i].y - detections[i].h * 0.5f), 1f - (detections[i].y + detections[i].h * 0.5f));
        }

        mat.SetVectorArray("_data", Data);


    }

    #endregion
}
