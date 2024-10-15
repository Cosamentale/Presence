using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using YoloV4Tiny;
using System.Linq;

public sealed class VisualizerYoloMesh : MonoBehaviour
{
    [SerializeField] RenderTexture tex;
    [SerializeField, Range(0, 1)] float _threshold = 0.5f;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Canvas _preview = null;
    [SerializeField] Marker _markerPrefab = null;

    ObjectDetector _detector;
    Marker[] _markers = new Marker[10];
    RenderTexture _reducedTex;
    public Vector4[] Data;
    public float[] ratio;
    public Material mat;
    void Start()
    {
        _detector = new ObjectDetector(_resources);
        for (var i = 0; i < _markers.Length; i++)
            _markers[i] = Instantiate(_markerPrefab, _preview.transform);

        _reducedTex = new RenderTexture(tex.width / 2, tex.height / 2, 0);
    }

    void OnDisable()
      => _detector.Dispose();

    void OnDestroy()
    {
        for (var i = 0; i < _markers.Length; i++) Destroy(_markers[i]);
        Destroy(_reducedTex);
    }

    void Update()
    {
        Graphics.Blit(tex, _reducedTex);
        _detector.ProcessImage(_reducedTex, _threshold);
        var detections = _detector.Detections.ToArray();
       
        var i = 0;
        foreach (var d in _detector.Detections)
        {
            if (i == _markers.Length) break;
            _markers[i++].SetAttributes(d);
            
        }
        Data = new Vector4[4];
        ratio = new float[4];

        for (int j = 0; j < Mathf.Clamp(detections.Length, 0, 4); j++)
        {
            Data[j] = new Vector4(detections[j].x - detections[j].w * 0.5f, detections[j].x + detections[j].w * 0.5f, 1f - (detections[j].y - detections[j].h * 0.5f), 1f - (detections[j].y + detections[j].h * 0.5f));
            ratio[j] = detections[j].w / detections[j].h;
        }

        mat.SetVectorArray("_data", Data);
        mat.SetFloatArray("_ratio", ratio);
        for (; i < _markers.Length; i++) _markers[i].Hide();
    }
}
