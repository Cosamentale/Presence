using UnityEngine;

namespace OscSimpl.Examples
{
	public class GettingStartedSendinglum : MonoBehaviour
	{
		[SerializeField] OscOut _oscOut;

		OscMessage _message2; // Cached message.
        
        private int Nbr_portOut;
		public string address1 = "/f1";
		public string address2 = "/f2";
        public string address3 = "/f3";
        //public string address4 = "/activate";
        public string address5 = "/f5";
        public string address6 = "/f6";
        public string address7 = "/d3";
        public string address8 = "/d4";
        public string address9 = "/d5";
        public string address10 = "/ecriture";
        public string address11 = "/compo1";
        public string address12 = "/compo2";
        /*public string address13 = "/ds1";
        public string address14 = "/ds2";
        public string address15 = "/bs1";
        public string address16 = "/bs2";
        public string address17 = "/ts1";
        public string address18 = "/ts2"; */
        //public string adresse12 = "/bande";
        //public string address8 = "/d4";

        public float speed = 0;
        public float speed2 = 0;
        private string LocalIPTarget;

        public InfraredDetectionFrame script1;
        public Timer script2;
        public WebcamCompo01Compute script3;
        public Receiver script4;
        void Start()
		{
            LocalIPTarget = _oscOut.remoteIpAddress;
            Nbr_portOut = _oscOut.port;
           // LocalIPTarget = "192.168.1.25";
            // Ensure that we have a OscOut component.
            if ( !_oscOut ) _oscOut = gameObject.AddComponent<OscOut>();

			// Prepare for sending messages locally on this device on port 7000.
			_oscOut.Open(Nbr_portOut, LocalIPTarget);
            //_oscOut.Send(address15, 10);
            //_oscOut.Send(address16, 10);
            //_oscOut.Send(address14, 10);
            //_oscOut.Send(address13, 10);
            _oscOut.Send(address12, 10);
            _oscOut.Send(address11, 10);
            //_oscOut.Send(address17, 10);
            //_oscOut.Send(address18, 10);
            _oscOut.Send(address5, 0);
            _oscOut.Send(address6, 0);
            _oscOut.Send(address7, 0);
            _oscOut.Send(address8, 0);
            _oscOut.Send(address1, 0);
            // ... or, alternatively target remote devices with a IP Address.
            //oscOut.Open( 7000, "192.168.1.101" );

            // If you want to send a single value then you can use this one-liner.
            //_oscOut.Send( address1, 0.5f );

            // If you want to send a message with multiple values, then you
            // need to create a message, add your values and send it.
            // Always cache the messages you create, so that you can reuse them.
            //_message2 = new OscMessage( address2 );
            //_message2.Add( Time.frameCount ).Add( Time.time ).Add( Random.value );
            //_oscOut.Send( _message2 );
            // _oscOut.Send(address2, 0.6f);
        }

         float fract(float t) { return t - Mathf.Floor(t); }
        float rd(float x) { float fx = Mathf.Floor(x); return Mathf.Sin(Vector2.Dot(new Vector2(fx,fx), new Vector2(54.56f, 54.56f))) * 7845.236f; }
        void Update()
		{

            if (script4.acti2 == 2)
            {
                //_oscOut.Send(address15, 10);
               // _oscOut.Send(address16, 10);
                //_oscOut.Send(address14, 10);
               // _oscOut.Send(address13, 10);
                _oscOut.Send(address12, 10);
                _oscOut.Send(address11, 10);

              //  if (script4.factivate2 > 0.95) { _oscOut.Send(address4, 10); //_oscOut.Send(address18, 10); _oscOut.Send(address17, 10); }
               // else { _oscOut.Send(address4, 0);
                    /*if (script4.ts1 > 0.5) { _oscOut.Send(address17, 0); }
                    else { _oscOut.Send(address17, 10); }
                    if (script4.ts2 > 0.5) { _oscOut.Send(address18, 0); }
                    else { _oscOut.Send(address18, 10); } */
               // }
                _oscOut.Send(address5, 0);
                _oscOut.Send(address6, 0);
                _oscOut.Send(address7, 0);
                _oscOut.Send(address8, 0);
                _oscOut.Send(address1, 0);
            }

                if (script4.acti2 ==1)
            {
                _oscOut.Send(address5, script3.floatArray1[0]);
                _oscOut.Send(address6, script3.floatArray1[1]);
                _oscOut.Send(address7, script3.floatArray1[2]);
                _oscOut.Send(address8, script3.floatArray1[3]);
                _oscOut.Send(address10, (1 - script3.solo) * 10);
                _oscOut.Send(address9, script3.bande );
                //_oscOut.Send(address15, 0);
                //_oscOut.Send(address16, 10-script3.ts2);
               // _oscOut.Send(address17, 10);
               // _oscOut.Send(address18, 10);
               // _oscOut.Send(address14, 10);
               // _oscOut.Send(address13, 10);
                _oscOut.Send(address12, 10);
                _oscOut.Send(address11, 10);
                _oscOut.Send(address1, 0);
                //_oscOut.Send(address7, script3.tic);

            }
            
            // _oscOut.Send(address9, script3.solo*10);
            // _oscOut.Send(address10,(1- script3.solo) * 10);
            if (script4.acti2 == 0)
            {
                float va1;
                if (script1.floatArray1[2] < 0.01f) { va1 = 0; }
                else { va1 = 10; }
                _oscOut.Send(address1, va1);
                if (script4.compo1 > 0.5) { _oscOut.Send(address11, 0); }
                else { _oscOut.Send(address11, 10); }
                if(script4.compo2 > 0.5) { _oscOut.Send(address12, 0); }
                else { _oscOut.Send(address12, 10); }
               // if (script4.acti  * (1 - script4.acti3) > 0.5) { _oscOut.Send(address13, 0); }
               // else { _oscOut.Send(address13, 10); }
               // if (script4.ds2 > 0.5) { _oscOut.Send(address14, 0); }
               // else { _oscOut.Send(address14, 10); }
               // if(script4.factivate > 0.05f) { _oscOut.Send(address4, 0); }
               // else { _oscOut.Send(address4, 10); }
               // _oscOut.Send(address15, 10);
                //_oscOut.Send(address16, 10);
                //_oscOut.Send(address17, 10);
                //_oscOut.Send(address18, 10);
                _oscOut.Send(address5, 0);
                _oscOut.Send(address6, 0);
                _oscOut.Send(address7, 0);
                _oscOut.Send(address8, 0);
                //_oscOut.Send(address15, 0);
                // _oscOut.Send(address4, script2.activate);
                // _oscOut.Send(address3, script2.activationScene02 * 10);
            }
           
            // _oscOut.Send(address2, infra.floatArray1[1] * 10);
            // _oscOut.Send(address3, infra.floatArray1[2] * 10);
            /*_oscOut.Send(address5, infra.floatArray1[3]);
            _oscOut.Send(address6, infra.floatArray1[4]);  */
            // We update the content of message2 and send it again.
            //   _message2.Set( 0, Time.frameCount );
            //	_message2.Set( 1, Time.time );
            //	_message2.Set( 2, Random.value );
            //	_oscOut.Send( _message2 );
         
        }
	}
}