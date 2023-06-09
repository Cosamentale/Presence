﻿using UnityEngine;

namespace OscSimpl.Examples
{
	public class ReceiverInstal : MonoBehaviour
	{

		[SerializeField] OscIn _oscIn;

		public string address1 = "/c1";
		public string address2 = "/c2";
        public string address3 = "/c3";
        public string address4 = "/c4";

        public string address8 = "/l1";
      
        public float v1 = 0;
        public float v2 = 0;
        public float v3 = 0;
        public float v4 = 0;
        public float v5 = 0;
        public float v6 = 0;
        public float v7 = 0;
        public float v8 = 0;
        public float compo1;
        public float compo2;
        public Material[] materials;
        public int numberOfMaterials;
        public InfraredDetectionFrameLine script1;
        public WebcamCompo01Compute script2;
        public InfraredDetectionFrame script3;

        public Face script4;
        public bool C_Value1_In = false;
		public float DynamicValue_In;
        public bool noOSC = false;
        public float vf1;
        public float vf2;
        public float vf3;
        public float vf4;
        public float vf5;
        public float vf6;
        public float vf7;
        public float vf8;
        public float puissance;
        public contorlLightinstal lightt;
   
      
        //public bool changed = true;
        public bool flag = false;  // Flag to track if the boolean became true
        public bool controlEnabled = true;  // Flag to track if the control is enabled
        public bool isTrue = false;
        void Start()
		{
			// Ensure that we have a OscIn component and start receiving on port 7000.
			if (!_oscIn) _oscIn = gameObject.AddComponent<OscIn>();
			_oscIn.Open(7000);

			C_Value1_In = false;

		}


		void OnEnable()
		{
			// You can "map" messages to methods in two ways:

			// 1) For messages with a single argument, route the value using the type specific map methods.
			_oscIn.MapFloat(address1, In_Trigger1);
            _oscIn.MapFloat(address2, In_Trigger2);
            _oscIn.MapFloat(address3, In_Trigger3);
            _oscIn.MapFloat(address4, In_Trigger4);

            _oscIn.MapFloat(address8, In_Trigger8);
         
            //_oscIn.MapFloat(address10, In_ChangingValue1);

            // 2) For messages with multiple arguments, route the message using the Map method.
            //_oscIn.Map( address2, OnTest2 );
        }
        float fract(float t) { return t - Mathf.Floor(t); }
        float rd(float x) { float fx = Mathf.Floor(x); return fract(Mathf.Sin(Vector2.Dot(new Vector2(fx, fx), new Vector2(54.56f, 54.56f))) * 7845.236f); }
        float no(float x) { return Mathf.Lerp(rd(x), rd(x + 1), Mathf.SmoothStep(0, 1, fract(x))); }
        void Update()
		{
          
            ///////////////////// TO MOUVE IN CONTROLLER SECTION
            /* if (Input.GetKeyDown(KeyCode.V))
             {
                 if (!C_Value1_In)
                 {
                     C_Value1_In = true;
                 }
                 else
                 {
                     C_Value1_In = false;
                 }
             }  */

            float n1 = no(Time.time + 45.98f);
           
                 vf1 = v1 * puissance + n1*0.02f;
           vf2 = v2 * puissance + n1 * 0.02f;
           vf3 = v3 * puissance + n1 * 0.02f;
           vf4 = v4 * puissance ;
          
                vf8 = v8;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_c1", (vf1));
                    materials[i].SetFloat("_c2", (vf2));
                    materials[i].SetFloat("_c3", Mathf.Floor(vf3));
                    materials[i].SetFloat("_c4", (vf4));
                  
                }
                script1._c1 = Mathf.Floor(vf1);
       
                script2._c1 = Mathf.Floor(vf1);
                script2._c2 = Mathf.Floor(vf2);
                script2._c3 = Mathf.Floor(vf3);
                script3._c1 = Mathf.Floor(vf1);
                script4.c1 = Mathf.Floor(vf1);
                script4.c2 = Mathf.Floor(vf2);
                script4.c3 = Mathf.Floor(vf3);
                script4.c4 = Mathf.Floor(vf4);
            
           
           
            lightt.vl1 = vf8;
            // script1._c4 = vf4;
            //script1._c1 = vf1;
            //script1._c1 = vf1;
            //script1._c1 = vf1;
          
        }
       
      /*  void OnDisable()
		{
			
		//	_oscIn.UnmapFloat(In_ChangingValue1);
		}
                 */
        void In_Trigger1(float value)
		{
          
            v1 += Mathf.Abs(value);
               
        }
        void In_Trigger2(float value)
        {
           
            v2 += Mathf.Abs(value) ;
          
        }
        void In_Trigger3(float value)
        {
        
            v3 += Mathf.Abs(value) ;
           
        }
        void In_Trigger4(float value)
        {
           
            v4 += Mathf.Abs(value);
         
        }
       
        void In_Trigger8(float value)
        {

            v8 += Mathf.Abs(value);

        }
      

    }
}