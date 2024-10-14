using UnityEngine;
using UI = UnityEngine.UI;
using Klak.TestTools;
using System.Linq;
namespace UltraFace {

    public sealed class VisualizerScript : MonoBehaviour
    {
        #region Editable attributes

       // [SerializeField] ImageSource _source = null;
        [SerializeField, Range(0, 1)] float _threshold = 0.5f;
        [SerializeField] ResourceSet _resources = null;
        public Texture _source = null;
        public Material mat;
        public Vector4[] Data ;
        #endregion

        #region Private objects

        FaceDetector _detector;
    ComputeBuffer _drawArgs;
    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _detector = new FaceDetector(_resources);
        _drawArgs = new ComputeBuffer(4, sizeof(uint),
                                      ComputeBufferType.IndirectArguments);
        _drawArgs.SetData(new [] {6, 0, 0, 0});
    }

    void OnDestroy()
    {
        _detector?.Dispose();
        _drawArgs?.Dispose();
    }

    void Update()
    {
        _detector.ProcessImage(_source, _threshold);
            var detections = _detector.Detections.ToArray();

            int detectionCount = detections.Length;
            Data = new Vector4[10];
           

            for (int i = 0; i < Mathf.Clamp(detections.Length,0,10); i++)
            {
                Data[i] = new Vector4(detections[i].x1, detections[i].y1, detections[i].x2, detections[i].y2);
            }

          //  mat.SetVectorArray("_data", Data);
          //  mat.SetTexture("_Texture", _source);
      
        }

   

    #endregion
}

} // namespace UltraFace
