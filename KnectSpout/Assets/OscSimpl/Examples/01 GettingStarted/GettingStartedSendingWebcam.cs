using UnityEngine;

namespace OscSimpl.Examples
{
	public class GettingStartedSendingWebcam : MonoBehaviour
	{
		[SerializeField] OscOut _oscOut;

		OscMessage _message2; // Cached message.
        
        private int Nbr_portOut;
		public string address1 = "/f1";
		public string address2 = "/f2";
        public string address3 = "/f3";
        public string address4 = "/f4";
        public string address5 = "/f5";
        public string address6 = "/f6";
        public string address7 = "/d3";
        public string address8 = "/d4";
        public float speed = 0;
        public float speed2 = 0;
        private string LocalIPTarget;

        public InfraredDetectionFrame script1;
        public Timer script2;

        void Start()
		{
            LocalIPTarget = _oscOut.remoteIpAddress;
            Nbr_portOut = _oscOut.port;
           // LocalIPTarget = "192.168.1.25";
            // Ensure that we have a OscOut component.
            if ( !_oscOut ) _oscOut = gameObject.AddComponent<OscOut>();

			// Prepare for sending messages locally on this device on port 7000.
			_oscOut.Open(Nbr_portOut, LocalIPTarget);

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

            //_oscOut.Send(address1, infra.float1*10);
            float tt1 = 0; float tt2 = 0; float tt3 = 0; float tt4 = 0;
            // if (fract(Time.time * speed) < 0.5)
            if (rd(Time.time*speed*script2.dmx)<0.5)
            {
                tt1 = 0;
            }
            else
            {
                tt1 = 255 ;
            }
            if (rd(Time.time * speed * script2.dmx+234) < 0.5)
            {
                tt2 = 0;
            }
            else
            {
                tt2 = 255;
            }
            if (rd(Time.time * speed2 * script2.dmx+945) < 0.5)
            {
                tt3 = 0;
            }
            else
            {
                tt3 = 255;
            }
            if (rd(Time.time * speed2 * script2.dmx+785) < 0.5)
            {
                tt4 = 0;
            }
            else
            {
                tt4 = 255;
            } 
            _oscOut.Send(address5, tt1);
            _oscOut.Send(address6, tt2);
            _oscOut.Send(address7, tt3);
            _oscOut.Send(address8, tt4);     
            _oscOut.Send(address1, script1.floatArray1[2]*10);
            _oscOut.Send(address4, script2.activate);
            _oscOut.Send(address3, script2.activationScene02*10);
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