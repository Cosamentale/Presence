using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using YoloV4Tiny;
using System.Linq;
using OscSimpl.Examples;
using Unity.Barracuda;

public class VisualizerYolo : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] RenderTexture texture;
    [SerializeField, Range(0, 1)] float _threshold = 0.5f;
    [SerializeField] ResourceSet _resources = null;
    public Vector4[] Data;
    //public Material mat;
    public int nb;
    public int nbmax;
    public string[] label;
    public string[] score;
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
    public static string[] _labels = new[]
{
        "Plane", "Bicycle", "Bird", "Boat",
        "Bottle", "Bus", "Car", "Cat",
        "Chair", "Cow", "Table", "Dog",
        "Horse", "Motorbike", "Person", "Plant",
        "Sheep", "Sofa", "Train", "TV"
    };
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
        
        nb = Mathf.Clamp(detectionCount, 0, nbmax);
        Data = new Vector4[nb];
        label = new string[nb];
        score = new string[nb];
        for (int i = 0; i < nb; i++)
        {
            Data[i] = new Vector4(detections[i].x-detections[i].w*0.5f, detections[i].x+detections[i].w*0.5f,(detections[i].y - detections[i].h * 0.5f),  (detections[i].y + detections[i].h * 0.5f));
            //label[i] = detections[i].classIndex+1;
            label[i] = _labels[(int)detections[i].classIndex];
            score[i] = $"{Mathf.Round(detections[i].score * 100f) / 100f:F2}%";
        }

        //mat.SetVectorArray("_data", Data);


    }

    #endregion
}
