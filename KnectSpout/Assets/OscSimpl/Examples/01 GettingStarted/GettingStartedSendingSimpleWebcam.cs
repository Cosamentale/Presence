using UnityEngine;

namespace OscSimpl.Examples
{
	public class GettingStartedSendingSimpleWebcam : MonoBehaviour
	{
        [SerializeField] OscOut _oscOut;
        OscMessage _message2; // Cached message.

        private int Nbr_portOut;
        //"nose", "leftShoulder", "rightShoulder", "leftElbow", "rightElbow", "leftWrist", "rightWrist",  "Hip", "leftKnee", "rightKnee", "leftAnkle", "rightAnkle"
        public string address1 = "/v1";
        public string address2 = "/v2";
        public string address3 = "/v3";
        public string address4 = "/v4";
        public string address5 = "/v5";
        public string address6 = "/v6";
        public string address7 = "/v7";
        public string address8 = "/v8";
        public string address9 = "/v9";
        public string address10 = "/v10";
        public float v1;
        public float v2;
        public float v3;
        public float v4;
        public float v5;
        public float v6;
        public float v7;
        public float v8;
        public float v9;
        public float v10;
        public float vv3;
        public float vv4;
        public float vv5;
        public float vv6;
        public float vv7;
        public float vv8;
        public float vv9;
        public float vv10;
        public float decrease;
        private string LocalIPTarget;
        public PoseEstimatorSimpleWebcam script2;

        void Start()
        {
            LocalIPTarget = _oscOut.remoteIpAddress;
            Nbr_portOut = _oscOut.port;
            if (!_oscOut) _oscOut = gameObject.AddComponent<OscOut>();
            _oscOut.Open(Nbr_portOut, LocalIPTarget);

        }
        //"nose", "leftShoulder", "rightShoulder", "leftElbow", "rightElbow", "leftWrist", "rightWrist", "leftHip", "rightHip", "leftKnee", "rightKnee", "leftAnkle", "rightAnkle"

        void Update()
        {
            v1 +=(script2.pos1[0].x - 0.5f)*0.01f* script2.score1[0] ;
            v2 += (script2.pos1[0].y - 0.5f) * 0.01f * script2.score1[0];
            v1 = Mathf.Clamp01(v1);
            v2 = Mathf.Clamp01(v2);
            vv3 = Mathf.Abs(script2.pos1[5].x - vv3) * script2.score1[5];
            v3 += vv3 - decrease;
            v3 = Mathf.Pow(Mathf.Clamp01(v3),2f);
            vv4 = Mathf.Abs(script2.pos1[5].y - vv4) * script2.score1[5];
            v4 += vv4 - decrease;
            v4 = Mathf.Pow(Mathf.Clamp01(v4), 2f);
            vv5 = Mathf.Abs(script2.pos1[6].x - vv5) * script2.score1[6];
            v5 += vv5 - decrease;
            v5 = Mathf.Pow(Mathf.Clamp01(v5), 2f);
            vv6 = Mathf.Abs(script2.pos1[6].y - vv6) * script2.score1[6];
            v6 += vv6 - decrease;
            v6 = Mathf.Pow(Mathf.Clamp01(v6), 2f);
            vv7 = Mathf.Abs(script2.pos1[4].x - vv7) * script2.score1[4];
            v7 += vv7 - decrease;
            v7 = Mathf.Pow(Mathf.Clamp01(v7), 2f);
            vv8 = Mathf.Abs(script2.pos1[4].y - vv8) * script2.score1[4];
            v8 += vv8 - decrease;
            v8 = Mathf.Pow(Mathf.Clamp01(v8), 2f);
            vv9 = Mathf.Abs(script2.pos1[0].x - vv9) * script2.score1[0];
            v9 += vv9 - decrease;
            v9 = Mathf.Pow(Mathf.Clamp01(v9), 2f);
            vv10 = Mathf.Abs(script2.pos1[0].y - vv10) * script2.score1[0];
            v10 += vv10 - decrease;
            v10 = Mathf.Pow(Mathf.Clamp01(v10), 2f);
            _oscOut.Send(address1,v1);
            _oscOut.Send(address2, v2);
            _oscOut.Send(address3, v3);
            _oscOut.Send(address4, v4 );
            _oscOut.Send(address5, v5);
            _oscOut.Send(address6, v6);
            _oscOut.Send(address7, v7);
            _oscOut.Send(address8, v8 );
            _oscOut.Send(address9,v9);
            _oscOut.Send(address10, v10);

        }
    }
}