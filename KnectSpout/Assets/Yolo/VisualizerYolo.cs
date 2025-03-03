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
    public float nb;
    public string[] label;
    public float[] score;
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
        
        nb = detections.Length;
        Data = new Vector4[detections.Length];
        label = new string[detections.Length];
        score = new float[detections.Length];
        for (int i = 0; i < Mathf.Clamp(detections.Length, 0, 10); i++)
        {
            Data[i] = new Vector4(detections[i].x-detections[i].w*0.5f, detections[i].x+detections[i].w*0.5f,(detections[i].y - detections[i].h * 0.5f),  (detections[i].y + detections[i].h * 0.5f));
            //label[i] = detections[i].classIndex+1;
            label[i] = _labels[(int)detections[i].classIndex];
            score[i] = detections[i].score;
        }

        //mat.SetVectorArray("_data", Data);


    }

    #endregion
}
