using UnityEngine;

namespace OscSimpl.Examples
{
	public class Receiver : MonoBehaviour
	{
		[SerializeField] OscIn _oscIn;

		public string address1 = "/c1";
		public string address2 = "/c2";
        public string address3 = "/c3";
        public string address4 = "/c4";
        public string address5 = "/c5";
        public string address6 = "/c6";
        public string address7 = "/c7";
        public float v1 = 0;
        public float v2 = 0;
        public float v3 = 0;
        public float v4 = 0;
        public float v5 = 0;
        public float v6 = 0;
        public float v7 = 0;
        public float v8 = 0;
        public string address10 = "";
        public Material[] materials;
        public int numberOfMaterials;
        public InfraredSourceDessin script1;
        public WebcamCompo01Compute script2;
        public InfraredDetectionFrame script3;
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
        public float puissance;
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
            _oscIn.MapFloat(address5, In_Trigger5);
            _oscIn.MapFloat(address6, In_Trigger6);
            _oscIn.MapFloat(address7, In_Trigger7);
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
            if (Input.GetKeyDown(KeyCode.V))
			{
				if (!C_Value1_In)
				{
					C_Value1_In = true;
				}
				else
				{
					C_Value1_In = false;
				}
			}

            float n1 = no(Time.time + 45.98f);
            if (noOSC == false)
            {
                 vf1 = v1 * puissance + n1*0.02f;
           vf2 = v2 * puissance + n1 * 0.02f;
           vf3 = v3 * puissance + n1 * 0.02f;
           vf4 = v4 * puissance ;
           vf5 = v5 * puissance + n1 * 0.02f;
           vf6 = v6 * puissance + n1 * 0.02f;
           vf7 = v7 * puissance + n1 * 0.02f;  
            }
            else
            {
                // Debug.Log("Update method called");
                //float n1 = no(Time.time + 45.98f);
                float n2 = rd(Time.time + 95.12f);
                float n3 = rd(Time.time + 78.13f);
                float n4 = rd(Time.time + 13.62f);
                vf1 += n1 * 0.05f;
                vf2 += n2 * 0.05f ;
                vf3 += n3 * 0.05f ;
                vf4 += n4 * 0.05f;
                vf5 = n1;
                vf6 = n2;
                vf7 = n3;      
            }    
           
           
            /*material.SetFloat("_c1", vf1);
            material.SetFloat("_c2", vf2);
            material.SetFloat("_c3", vf3);
            material.SetFloat("_c4", vf4);

            material2.SetFloat("_c1", vf1);
            material2.SetFloat("_c2", vf2);
            material2.SetFloat("_c3", vf3);
      
            material3.SetFloat("_c1", vf1);
            material3.SetFloat("_c2", vf2);
            material3.SetFloat("_c3", vf3);
            material3.SetFloat("_c4", vf5);
            material3.SetFloat("_c5", vf6);
            material3.SetFloat("_c6", vf7); */
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_c1", Mathf.Floor(vf1));
                materials[i].SetFloat("_c2", Mathf.Floor(vf2));
                materials[i].SetFloat("_c3", Mathf.Floor(vf3));
                materials[i].SetFloat("_c4", (vf4));
                materials[i].SetFloat("_c5", Mathf.Floor(vf5));
                materials[i].SetFloat("_c6", Mathf.Floor(vf6));
                materials[i].SetFloat("_c7", Mathf.Floor(vf7));
            }
            script1._c1 = Mathf.Floor(vf5);
            script1._c2 = Mathf.Floor(vf6);
            script1._c3 = Mathf.Floor(vf7);
            script2._c1 = Mathf.Floor(vf1);
            script2._c2 = Mathf.Floor(vf2);
            script2._c3 = Mathf.Floor(vf3);
            script3._c1 = Mathf.Floor(vf5);
            // script1._c4 = vf4;
            //script1._c1 = vf1;
            //script1._c1 = vf1;
            //script1._c1 = vf1;
        }

        void OnDisable()
		{
			
			_oscIn.UnmapFloat(In_ChangingValue1);
		}
   
        void In_Trigger1(float value)
		{
          
            v1 += Mathf.Abs(value)*0.1f;
               
        }
        void In_Trigger2(float value)
        {
           
            v2 += Mathf.Abs(value) * 0.1f;
          
        }
        void In_Trigger3(float value)
        {
        
            v3 += Mathf.Abs(value) * 0.1f;
           
        }
        void In_Trigger4(float value)
        {
           
            v4 += Mathf.Abs(value) * 0.1f;
         
        }
        void In_Trigger5(float value)
        {
           
            v5 += Mathf.Abs(value) * 0.1f;
      
        }
        void In_Trigger6(float value)
        {
            
            v6 += Mathf.Abs(value) * 0.1f;
         
        }
        void In_Trigger7(float value)
        {
        
            v7 += Mathf.Abs(value) * 0.1f;
        
        }
        void In_ChangingValue1(float value)
		{
			if (!C_Value1_In)
			{
				DynamicValue_In = 0;
			}
			else
			{
				DynamicValue_In = value;
			}
		}

	}
}