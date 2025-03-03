using UnityEngine;

namespace OscSimpl.Examples
{
    public class ReceiverSpout : MonoBehaviour
    {

        [SerializeField] OscIn _oscIn;

        public string address1 = "/scene";
        public PrefabManageryolo yolo;
        public GameObject face;
        public PrefabManager poseNet;

        void Start()
        {
            // Ensure that we have a OscIn component and start receiving on port 7000.
            if (!_oscIn) _oscIn = gameObject.AddComponent<OscIn>();
            _oscIn.Open(7000);

        }


        void OnEnable()
        {
            // You can "map" messages to methods in two ways:

            // 1) For messages with a single argument, route the value using the type specific map methods.
            _oscIn.MapFloat(address1, In_Trigger1);


            //_oscIn.MapFloat(address10, In_ChangingValue1);

            // 2) For messages with multiple arguments, route the message using the Map method.
            //_oscIn.Map( address2, OnTest2 );
        }
        float fract(float t) { return t - Mathf.Floor(t); }
        float rd(float x) { float fx = Mathf.Floor(x); return fract(Mathf.Sin(Vector2.Dot(new Vector2(fx, fx), new Vector2(54.56f, 54.56f))) * 7845.236f); }
        float no(float x) { return Mathf.Lerp(rd(x), rd(x + 1), Mathf.SmoothStep(0, 1, fract(x))); }
        void Update()
        {
        }


            void In_Trigger1(float value)
            {
                if (value == 0)
                {
                    yolo.isPrefabActive = true;
                poseNet.isPrefabActive = false;
                    face.SetActive(false);
                }
                else
                {
                yolo.isPrefabActive = false;
                if (value == 1)
                    {
                        poseNet.isPrefabActive = true;
                        face.SetActive(false);
                    }
                    else
                    {
                        poseNet.isPrefabActive = false;
                        face.SetActive(true);
                    }
                }

            }

        
    }
}