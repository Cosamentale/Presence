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
        public string address8 = "/l1";
        public string address9 = "/f1";
        public string address10 = "/f2";
        public string address11 = "/f3";
        public string address12 = "/f4";
        public string address13 = "/f5";
        public string address14 = "/f6";
        public string address15 = "/f7";
        public string address16 = "/b1";
        public string address17 = "/b2";
        public string address18 = "/b3";
        public string address19 = "/b4";
        public string address20 = "/p";
        public string address21 = "/b5";
        public string address22 = "/b6";
        public string address23 = "/b7";
        public string address24 = "/b8";
        public string address25 = "/f8";
        public string address26 = "/f9";
        public string address27 = "/f10";
        public float v1 = 0;
        public float v2 = 0;
        public float v3 = 0;
        public float v4 = 0;
        public float v5 = 0;
        public float v6 = 0;
        public float v7 = 0;
        public float v8 = 0;
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
        public float vf8;
        public float puissance;
        public contorlLight lightt;
        public GameObject scene1;
        public GameObject scene2;
        public GameObject scene3;
        public float acti;
        public float acti2;
        public float acti3;
        public float acti4;
        public float acti5;
        public float selecspeed;
        public float speed;
        public float solo;
       // float vsolo;
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
            _oscIn.MapFloat(address5, In_Trigger5);
            _oscIn.MapFloat(address6, In_Trigger6);
            _oscIn.MapFloat(address7, In_Trigger7);
            _oscIn.MapFloat(address8, In_Trigger8);
            _oscIn.MapFloat(address9, In_Trigger9);
            _oscIn.MapFloat(address10, In_Trigger10);
            _oscIn.MapFloat(address11, In_Trigger11);
            _oscIn.MapFloat(address12, In_Trigger12);
            _oscIn.MapFloat(address13, In_Trigger13);
            _oscIn.MapFloat(address14, In_Trigger14);
            _oscIn.MapFloat(address15, In_Trigger15);
            _oscIn.MapFloat(address16, In_Trigger16);
            _oscIn.MapFloat(address17, In_Trigger17);
            _oscIn.MapFloat(address18, In_Trigger18);
            _oscIn.MapFloat(address19, In_Trigger19);
            _oscIn.MapFloat(address20, In_Trigger20);
            _oscIn.MapFloat(address21, In_Trigger21);
            _oscIn.MapFloat(address22, In_Trigger22);
            _oscIn.MapFloat(address23, In_Trigger23);
            _oscIn.MapFloat(address24, In_Trigger24);
            _oscIn.MapFloat(address25, In_Trigger25);
            _oscIn.MapFloat(address26, In_Trigger26);
            _oscIn.MapFloat(address27, In_Trigger27);
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
                vf8 = v8;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_c1", (vf1));
                    materials[i].SetFloat("_c2", (vf2));
                    materials[i].SetFloat("_c3", Mathf.Floor(vf3));
                    materials[i].SetFloat("_c4", (vf4));
                    materials[i].SetFloat("_c5", (vf5));
                    materials[i].SetFloat("_c6", (vf6));
                    materials[i].SetFloat("_c7", (vf7));
                }
                script1._c1 = Mathf.Floor(vf5);
                script1._c2 = Mathf.Floor(vf6);
                script1._c3 = Mathf.Floor(vf7);
                script2._c1 = Mathf.Floor(vf1);
                script2._c2 = Mathf.Floor(vf2);
                script2._c3 = Mathf.Floor(vf3);
                script3._c1 = Mathf.Floor(vf5);
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
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_c1", Mathf.Floor(vf1) + n1 * 0.02f);
                    materials[i].SetFloat("_c2", Mathf.Floor(vf2) + n1 * 0.02f);
                    materials[i].SetFloat("_c3", Mathf.Floor(vf3) + n1 * 0.02f);
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
                script3._c1 = Mathf.Floor(vf1);
            }
            if (acti2 == 0)
            {
                scene3.SetActive(false);
                if (acti == 0)
                {
                    scene1.SetActive(true);
                    scene2.SetActive(false);
                }
                else
                {
                    scene1.SetActive(false);
                    scene2.SetActive(true);
                }
            }
            else
            {
                scene1.SetActive(false);
                scene2.SetActive(false);
                scene3.SetActive(true);
            }
             
            if(acti3 == 0)
            {
                script2.phase2 = false;
            }
            else
            {
                script2.phase2 = true;
              
            }
            if (acti4 == 0)
            {
                script2.phase3 = false;
            }
            else
            {
                script2.phase3 = true;              
            }
          
                lightt.vl1 = vf8;
            // script1._c4 = vf4;
            //script1._c1 = vf1;
            //script1._c1 = vf1;
            //script1._c1 = vf1;
            if(selecspeed == 0)
            {
                script2.speed1 = 1 + speed * 10;
            }
            else
            {
                script2.speed1 = speed;
            }

        }
       
        void OnDisable()
		{
			
			_oscIn.UnmapFloat(In_ChangingValue1);
		}
   
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
        void In_Trigger5(float value)
        {
           
            v5 += Mathf.Abs(value) ;
      
        }
        void In_Trigger6(float value)
        {
            
            v6 += Mathf.Abs(value) ;
         
        }
        void In_Trigger7(float value)
        {
        
            v7 += Mathf.Abs(value);
        
        }
        void In_Trigger8(float value)
        {

            v8 += Mathf.Abs(value);

        }
        void In_Trigger9(float value)
        {

            materials[2].SetFloat("_fondu", value);

        }
        void In_Trigger10(float value)
        {

            materials[2].SetFloat("_bluractivation", value);
            script2._bluractivation= value;

        }
        void In_Trigger11(float value)
        {

            materials[2].SetFloat("_step0to1", value);

        }
        void In_Trigger12(float value)
        {

            materials[2].SetFloat("_step1to2", value);
            script2._step1to2= value;

        }
        void In_Trigger13(float value)
        {

            materials[2].SetFloat("_powermodification", value);

        }
        void In_Trigger14(float value)
        {

            materials[2].SetFloat("_step2invert", value);

        }
        void In_Trigger15(float value)
        {

            materials[2].SetFloat("_dither", value);

        }
        void In_Trigger16(float value)
        {

            materials[2].SetFloat("_final", value);
           

        }
        void In_Trigger17(float value)
        {
            acti = value;
        }
       
        void In_Trigger18(float value)
        {
           
            scene2.GetComponent<InfraredDetectionFrame>().SecondPhase = value;
            
            

        }
        void In_Trigger19(float value)
        {

            scene2.GetComponent<InfraredDetectionFrame>().TroisiemePhase = value;

        }
        void In_Trigger20(float value)
        {

           acti2 = value;

        }
        void In_Trigger21(float value)
        {

            acti3 = value;

        }
        void In_Trigger22(float value)
        {

            acti4 = value;

        }
        void In_Trigger23(float value)
        {

            script2.solo = value;

        }
        void In_Trigger24(float value)
        {

            selecspeed = value;

        }
        void In_Trigger25(float value)
        {

            speed = value;

        }
        void In_Trigger26(float value)
        {

            script2.speed2 =   value*100;

        }
        void In_Trigger27(float value)
        {

            script2.speed3 = value * 1000;
            

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