using UnityEngine;

namespace OscSimpl.Examples
{
    public class ReceiverSpout : MonoBehaviour
    {

        [SerializeField] OscIn _oscIn;

        public string address1 = "/scene";
        public string address2 = "/yolonbmax";
        public string address3 = "/yolomodel1";
        public string address4 = "/yolomodel2";
        public PrefabManageryolo yolo1;
        public PrefabManageryolo yolo2;
        public PrefabManageryolo yolo3;
        public GameObject face;
        public PrefabManager poseNet;
        public bool yolo1t;
        public bool yolo2t;
        public bool yolo3t;
        public PrefabManageryolo prefab1;
        public PrefabManageryolo prefab2;
        public PrefabManageryolo prefab3;
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
            _oscIn.MapFloat(address2, In_Trigger2);
            _oscIn.MapFloat(address3, In_Trigger3);
            _oscIn.MapFloat(address4, In_Trigger4);
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
            if(value == 0)
            {
                poseNet.isPrefabActive = false;
                face.SetActive(false);
                yolo1.isPrefabActive = false;
                yolo2.isPrefabActive = false;
                yolo3.isPrefabActive = false;
            }
            else {
                if (value == 1)
                {
                    if (yolo1t == true)
                    {
                        yolo1.isPrefabActive = true;
                    }
                    else
                    {
                        yolo1.isPrefabActive = false;
                    }
                    if (yolo2t == true)
                    {
                        yolo2.isPrefabActive = true;
                    }
                    else
                    {
                        yolo2.isPrefabActive = false;
                    }
                    if (yolo3t == true)
                    {
                        yolo3.isPrefabActive = true;
                    }
                    else
                    {
                        yolo3.isPrefabActive = false;
                    }
                    poseNet.isPrefabActive = false;
                    face.SetActive(false);
                }
                else
                {

                    if (value == 2)
                    {
                        poseNet.isPrefabActive = true;
                        face.SetActive(false);
                        yolo1.isPrefabActive = false;
                        yolo2.isPrefabActive = false;
                        yolo3.isPrefabActive = false;
                    }
                    else
                    {
                        if (value == 3)
                        {
                            poseNet.isPrefabActive = false;
                            face.SetActive(true);
                            yolo1.isPrefabActive = false;
                            yolo2.isPrefabActive = false;
                            yolo3.isPrefabActive = false;
                        }
                        else
                        {
                            poseNet.isPrefabActive = false;
                            face.SetActive(false);
                            if (yolo1t == true)
                            {
                                yolo1.isPrefabActive = true;
                            }
                            else
                            {
                                yolo1.isPrefabActive = false;
                            }
                            if (yolo2t == true)
                            {
                                yolo2.isPrefabActive = true;
                            }
                            else
                            {
                                yolo2.isPrefabActive = false;
                            }
                            if (yolo3t == true)
                            {
                                yolo3.isPrefabActive = true;
                            }
                            else
                            {
                                yolo3.isPrefabActive = false;
                            }



                        }

                    }
                }

            }
        }
        void In_Trigger2(float value)
        {
            prefab1.nbmax = (int)value;
            prefab2.nbmax = (int)value;
            prefab3.nbmax = (int)value;
        }
        void In_Trigger3(float value)
        {
           
                if (value == 0)
                {
                    yolo1t = false;
                }
                else
                {
                    yolo1t = true;
                }
            
        }
            void In_Trigger4(float value)
            {
                if (value == 0)
                {
                    yolo2t = false;
                }
                else
                {
                    yolo2t = true;
                }
            }
        
    }
}