using System.Collections.Generic;
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

        public string address1 = "/p01";
        public string address2 = "/p02";
        public string address3 = "/p03";
        public string address4 = "/yolo";
        public string address5 = "/yolonb";
        public string address6 = "/yolonom";
        public string address7 = "/face";
        public string address8 = "/facenb";
        public PoseNetSpout script;
        public VisualizerYolo yolo;
        public Visualizer face;
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

                if (script.pers01 > 0.4f)
                {
                    _oscOut.Send(
                    address1,
                    script.posePositionsArray[0].x, script.posePositionsArray[0].y, script.posePositionsArray[0].z,
                    script.posePositionsArray[1].x, script.posePositionsArray[1].y, script.posePositionsArray[1].z,
                    script.posePositionsArray[2].x, script.posePositionsArray[2].y, script.posePositionsArray[2].z,
                    script.posePositionsArray[3].x, script.posePositionsArray[3].y, script.posePositionsArray[3].z,
                    script.posePositionsArray[4].x, script.posePositionsArray[4].y, script.posePositionsArray[4].z,
                    script.posePositionsArray[5].x, script.posePositionsArray[5].y, script.posePositionsArray[5].z,
                    script.posePositionsArray[6].x, script.posePositionsArray[6].y, script.posePositionsArray[6].z,
                    script.posePositionsArray[7].x, script.posePositionsArray[7].y, script.posePositionsArray[7].z,
                    script.posePositionsArray[8].x, script.posePositionsArray[8].y, script.posePositionsArray[8].z,
                    script.posePositionsArray[9].x, script.posePositionsArray[9].y, script.posePositionsArray[9].z,
                    script.posePositionsArray[10].x, script.posePositionsArray[10].y, script.posePositionsArray[10].z,
                    script.posePositionsArray[11].x, script.posePositionsArray[11].y, script.posePositionsArray[11].z,
                    script.posePositionsArray[12].x, script.posePositionsArray[12].y, script.posePositionsArray[12].z,
                    script.posePositionsArray[13].x, script.posePositionsArray[13].y, script.posePositionsArray[13].z,
                    script.posePositionsArray[14].x, script.posePositionsArray[14].y, script.posePositionsArray[14].z,
                    script.posePositionsArray[15].x, script.posePositionsArray[15].y, script.posePositionsArray[15].z,
                    script.posePositionsArray[16].x, script.posePositionsArray[16].y, script.posePositionsArray[16].z
                );
                }
                if (script.pers02 > 0.4f)
                {
                    _oscOut.Send(
                      address2,
                      script.posePositionsArray2[0].x, script.posePositionsArray2[0].y, script.posePositionsArray2[0].z,
                      script.posePositionsArray2[1].x, script.posePositionsArray2[1].y, script.posePositionsArray2[1].z,
                      script.posePositionsArray2[2].x, script.posePositionsArray2[2].y, script.posePositionsArray2[2].z,
                      script.posePositionsArray2[3].x, script.posePositionsArray2[3].y, script.posePositionsArray2[3].z,
                      script.posePositionsArray2[4].x, script.posePositionsArray2[4].y, script.posePositionsArray2[4].z,
                      script.posePositionsArray2[5].x, script.posePositionsArray2[5].y, script.posePositionsArray2[5].z,
                      script.posePositionsArray2[6].x, script.posePositionsArray2[6].y, script.posePositionsArray2[6].z,
                      script.posePositionsArray2[7].x, script.posePositionsArray2[7].y, script.posePositionsArray2[7].z,
                      script.posePositionsArray2[8].x, script.posePositionsArray2[8].y, script.posePositionsArray2[8].z,
                      script.posePositionsArray2[9].x, script.posePositionsArray2[9].y, script.posePositionsArray2[9].z,
                      script.posePositionsArray2[10].x, script.posePositionsArray2[10].y, script.posePositionsArray2[10].z,
                      script.posePositionsArray2[11].x, script.posePositionsArray2[11].y, script.posePositionsArray2[11].z,
                      script.posePositionsArray2[12].x, script.posePositionsArray2[12].y, script.posePositionsArray2[12].z,
                      script.posePositionsArray2[13].x, script.posePositionsArray2[13].y, script.posePositionsArray2[13].z,
                      script.posePositionsArray2[14].x, script.posePositionsArray2[14].y, script.posePositionsArray2[14].z,
                      script.posePositionsArray2[15].x, script.posePositionsArray2[15].y, script.posePositionsArray2[15].z,
                      script.posePositionsArray2[16].x, script.posePositionsArray2[16].y, script.posePositionsArray2[16].z
                  );
                }
                if (script.pers03 > 0.4f)
                {
                    _oscOut.Send(
                  address3,
                  script.posePositionsArray3[0].x, script.posePositionsArray3[0].y, script.posePositionsArray3[0].z,
                  script.posePositionsArray3[1].x, script.posePositionsArray3[1].y, script.posePositionsArray3[1].z,
                  script.posePositionsArray3[2].x, script.posePositionsArray3[2].y, script.posePositionsArray3[2].z,
                  script.posePositionsArray3[3].x, script.posePositionsArray3[3].y, script.posePositionsArray3[3].z,
                  script.posePositionsArray3[4].x, script.posePositionsArray3[4].y, script.posePositionsArray3[4].z,
                  script.posePositionsArray3[5].x, script.posePositionsArray3[5].y, script.posePositionsArray3[5].z,
                  script.posePositionsArray3[6].x, script.posePositionsArray3[6].y, script.posePositionsArray3[6].z,
                  script.posePositionsArray3[7].x, script.posePositionsArray3[7].y, script.posePositionsArray3[7].z,
                  script.posePositionsArray3[8].x, script.posePositionsArray3[8].y, script.posePositionsArray3[8].z,
                  script.posePositionsArray3[9].x, script.posePositionsArray3[9].y, script.posePositionsArray3[9].z,
                  script.posePositionsArray3[10].x, script.posePositionsArray3[10].y, script.posePositionsArray3[10].z,
                  script.posePositionsArray3[11].x, script.posePositionsArray3[11].y, script.posePositionsArray3[11].z,
                  script.posePositionsArray3[12].x, script.posePositionsArray3[12].y, script.posePositionsArray3[12].z,
                  script.posePositionsArray3[13].x, script.posePositionsArray3[13].y, script.posePositionsArray3[13].z,
                  script.posePositionsArray3[14].x, script.posePositionsArray3[14].y, script.posePositionsArray3[14].z,
                  script.posePositionsArray3[15].x, script.posePositionsArray3[15].y, script.posePositionsArray3[15].z,
                  script.posePositionsArray3[16].x, script.posePositionsArray3[16].y, script.posePositionsArray3[16].z
              );
                }
            }

            List<float> messageData = new List<float>();

            if (yolo != null)
            {
                int count = (int)yolo.nb;

                if (count > 0)
                {
                    object[] oscMessageData = new object[count * 4];
                    object[] oscMessageData2 = new object[count];
                    for (int i = 0; i < count; i++)
                    {
                        int index = i * 4;
                        oscMessageData[index] = yolo.Data[i].x;
                        oscMessageData[index + 1] = yolo.Data[i].y;
                        oscMessageData[index + 2] = yolo.Data[i].z;
                        oscMessageData[index + 3] = yolo.Data[i].w;
                        oscMessageData2[i] = yolo.label[i];
                    }

                    _oscOut.Send(address4, oscMessageData);
                    _oscOut.Send(address6, oscMessageData2);
                }
                if (yolo.nb > 0)
                {
                    _oscOut.Send(address5, yolo.nb);
                }
            }
            int count2 = (int)face.nb;
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
            if (face.nb > 0)
            {
                _oscOut.Send(address8, face.nb);
            }
        }
    }
}