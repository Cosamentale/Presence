using System;
using UnityEngine;
using Windows.Kinect;

public class DepthSourceManagerMy : MonoBehaviour
{
    private KinectSensor _Sensor;
    private DepthFrameReader _Reader;
    private ushort[] _Data;
    private byte[] _RawData;
    private Texture2D _Texture;

    [SerializeField]
    private Material mat;

    private bool _KinectActive = false; // Add a boolean variable to control Kinect activation

    public bool KinectActive // Property to get/set the Kinect activation status
    {
        get { return _KinectActive; }
        set
        {
            _KinectActive = value;
            if (_Sensor != null)
            {
                if (_KinectActive && !_Sensor.IsOpen) // Activate Kinect if not already open
                {
                    _Sensor.Open();
                }
                else if (!_KinectActive && _Sensor.IsOpen) // Deactivate Kinect if already open
                {
                    _Sensor.Close();
                }
            }
        }
    }

    public ushort[] GetData()
    {
        return _Data;
    }

    void Start()
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.DepthFrameSource.OpenReader();
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            _Data = new ushort[frameDesc.LengthInPixels];
            _RawData = new byte[frameDesc.LengthInPixels * 2];

            // 16bit のテクスチャ。適切な単色のフォーマットがないので
            // RGBA4444 or ARGB4444 or R16 で16bit分確保する
            //_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA4444, false);
            //_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.ARGB4444, false);
            _Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.R16, false);

            if (!_Sensor.IsOpen && _KinectActive) // Open Kinect if not already open and Kinect is active
            {
                _Sensor.Open();
            }
        }

        mat.SetTexture("_MainTex", _Texture);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Check if key "0" is pressed
        {
            KinectActive = !KinectActive; // Toggle Kinect activation status
        }

        if (_Sensor != null)
        {
            if (_KinectActive && !_Sensor.IsOpen) // If _KinectActive is true and the sensor is not open, open the sensor
            {
                _Sensor.Open();
            }
            else if (!_KinectActive && _Sensor.IsOpen) // If _KinectActive is false and the sensor is open, close the sensor
            {
                _Sensor.Close();
            }
        }

        if (_Reader != null && _Sensor != null && _Sensor.IsOpen && _KinectActive) // Check if Kinect is active before reading frames
        {
            var frame = _Reader.AcquireLatestFrame();
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            if (frame != null)
            {
                frame.CopyFrameDataToArray(_Data);

                Buffer.BlockCopy(_Data, 0, _RawData, 0, _Data.Length * 2);

                _Texture.LoadRawTextureData(_RawData);
                _Texture.Apply();
                frame.Dispose();
                frame = null;
            }
        }

    }

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            _Sensor = null;
        }
    }

    void OnDestroy()
    {
        if (_Texture != null)
        {
            Destroy(_Texture);
            _Texture = null;
        }
    }
}