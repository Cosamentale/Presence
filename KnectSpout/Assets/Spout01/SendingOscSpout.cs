using System.Collections.Generic;
using System.Linq;
using UltraFace;
using UnityEngine;


namespace OscSimpl.Examples
{
	public class SedingOscSpout : MonoBehaviour
	{
		[SerializeField] OscOut _oscOut;

		OscMessage _message2; // Cached message.
        
        private int Nbr_portOut;
        //"nose", "leftShoulder", "rightShoulder", "leftElbow", "rightElbow", "leftWrist", "rightWrist", "leftHip", "rightHip", "leftKnee", "rightKnee", "leftAnkle", "rightAnkle"

        public string address1 = "/p";
        public string address2 = "/pnb";
        public string address4 = "/yolo1";
        public string address5 = "/yolo1nb";
        public string address6 = "/yolo1nom";
        public string address7 = "/face";
        public string address8 = "/facenb";
        public string address9 = "/yolo1score";
        public string address10 = "/yolo2";
        public string address11 = "/yolo2nb";
        public string address12 = "/yolo2nom";
        public string address13 = "/yolo2score";
        public string address14 = "/yolo3";
        public string address15 = "/yolo3nb";
        public string address16 = "/yolo3nom";
        public string address17 = "/yolo3score";
        public PoseNetSpout script;
        public VisualizerYolo yolo1;
        public VisualizerYolo yolo2;
        public VisualizerYolo yolo3;
        public Visualizer face;
        public GameObject faco;
        private string LocalIPTarget;
        void Start()
		{
            LocalIPTarget = _oscOut.remoteIpAddress;
            Nbr_portOut = _oscOut.port;
            if ( !_oscOut ) _oscOut = gameObject.AddComponent<OscOut>();
            _oscOut.Open(Nbr_portOut, LocalIPTarget);
           


        }

        void Update()
		{

            if (script != null)
            {
                List<float> allPersonMessageData = new List<float>();

                // Iterate over each person
                for (int i = 0; i < script.posePositionsArray.Length; i++)
                {
                    if (script.pers[i] > 0.4f) // Only send for detected persons with confidence > 0.4
                    {
                        // Iterate over each pose point for this person
                        for (int j = 0; j < script.posePositionsArray[i].Length; j++)
                        {
                            allPersonMessageData.Add(script.posePositionsArray[i][j].x);
                            allPersonMessageData.Add(script.posePositionsArray[i][j].y);
                           // allPersonMessageData.Add(script.posePositionsArray[i][j].z);
                        }
                    }
                }

                // Send all collected person data in one OSC message
                if (allPersonMessageData.Count > 0)
                {
                    // Convert List to object[] for OSC transmission
                    object[] messageArray = allPersonMessageData.Cast<object>().ToArray();
                    _oscOut.Send(address1, messageArray);
                }

                // Send the count of detected people (persons)
                _oscOut.Send(address2, script.pers.Count(p => p > 0.4f));
            }

            List<float> messageData = new List<float>();

            if (yolo1 != null)
            {
                int count = (int)yolo1.nb;

                if (count > 0)
                {
                    object[] oscMessageData = new object[count * 4];
                    object[] oscMessageData2 = new object[count];
                    object[] oscMessageData3 = new object[count];
                    for (int i = 0; i < count; i++)
                    {
                        int index = i * 4;
                        oscMessageData[index] = yolo1.Data[i].x;
                        oscMessageData[index + 1] = yolo1.Data[i].y;
                        oscMessageData[index + 2] = yolo1.Data[i].z;
                        oscMessageData[index + 3] = yolo1.Data[i].w;
                        oscMessageData2[i] = yolo1.label[i];
                        oscMessageData3[i] = yolo1.score[i];
                    }

                    _oscOut.Send(address4, oscMessageData);
                    _oscOut.Send(address6, oscMessageData2);
                    _oscOut.Send(address9, oscMessageData3);
                }
              
                    _oscOut.Send(address5, yolo1.nb);
                
            }
            if (yolo2 != null)
            {
                int count = (int)yolo2.nb;

                if (count > 0)
                {
                    object[] oscMessageData = new object[count * 4];
                    object[] oscMessageData2 = new object[count];
                    object[] oscMessageData3 = new object[count];
                    for (int i = 0; i < count; i++)
                    {
                        int index = i * 4;
                        oscMessageData[index] = yolo2.Data[i].x;
                        oscMessageData[index + 1] = yolo2.Data[i].y;
                        oscMessageData[index + 2] = yolo2.Data[i].z;
                        oscMessageData[index + 3] = yolo2.Data[i].w;
                        oscMessageData2[i] = yolo2.label[i];
                        oscMessageData3[i] = yolo2.score[i];
                    }

                    _oscOut.Send(address10, oscMessageData);
                    _oscOut.Send(address12, oscMessageData2);
                    _oscOut.Send(address13, oscMessageData3);
                }

                _oscOut.Send(address11, yolo2.nb);

            }
            if (yolo3 != null)
            {
                int count = (int)yolo3.nb;

                if (count > 0)
                {
                    object[] oscMessageData = new object[count * 4];
                    object[] oscMessageData2 = new object[count];
                    object[] oscMessageData3 = new object[count];
                    for (int i = 0; i < count; i++)
                    {
                        int index = i * 4;
                        oscMessageData[index] = yolo3.Data[i].x;
                        oscMessageData[index + 1] = yolo3.Data[i].y;
                        oscMessageData[index + 2] = yolo3.Data[i].z;
                        oscMessageData[index + 3] = yolo3.Data[i].w;
                        oscMessageData2[i] = yolo3.label[i];
                        oscMessageData3[i] = yolo3.score[i];
                    }

                    _oscOut.Send(address14, oscMessageData);
                    _oscOut.Send(address16, oscMessageData2);
                    _oscOut.Send(address17, oscMessageData3);
                }

                _oscOut.Send(address15, yolo3.nb);

            }
           
                int count2 = (int)Mathf.Clamp(face.nb, 0, 10);
                if (count2 > 0)
                {
                    object[] oscMessageData = new object[count2 * 4];
                    for (int i = 0; i < count2; i++)
                    {
                        int index = i * 4;
                        oscMessageData[index] = face.Data[i].x;
                        oscMessageData[index + 1] = face.Data[i].y;
                        oscMessageData[index + 2] = face.Data[i].z;
                        oscMessageData[index + 3] = face.Data[i].w;
                    }

                    _oscOut.Send(address7, oscMessageData);

                }
            if (faco.activeSelf == true)
            {
                _oscOut.Send(address8, face.nb);
            }
        }
    }
}