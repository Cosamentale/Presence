using UnityEngine;


namespace OscSimpl.Examples
{
	public class GettingStartedSendingInstal2 : MonoBehaviour
	{
		[SerializeField] OscOut _oscOut;

		OscMessage _message2; // Cached message.
        
        private int Nbr_portOut;
	
        public string addressMax1 = "/l1";
        public string addressMax2 = "/l2";
        public string addressMax3 = "/l3";
        public string addressMax4 = "/l4";
        public contorlLightinstal lights;

        //public float speed = 0;
        //public float speed2 = 0;
        private string LocalIPTarget;

    
        void Start()
		{
            LocalIPTarget = _oscOut.remoteIpAddress;
            Nbr_portOut = _oscOut.port;
           // LocalIPTarget = "192.168.1.25";
            // Ensure that we have a OscOut component.
            if ( !_oscOut ) _oscOut = gameObject.AddComponent<OscOut>();

			// Prepare for sending messages locally on this device on port 7000.
			_oscOut.Open(Nbr_portOut, LocalIPTarget);
  

            _oscOut.Send(addressMax1, 0);
            _oscOut.Send(addressMax2, 0);
            _oscOut.Send(addressMax3, 0);
            _oscOut.Send(addressMax4, 0);
            /*_oscOut.Send(addressMax5, 0);
            _oscOut.Send(addressMax6, 0);
            _oscOut.Send(addressMax7, 0);
            _oscOut.Send(addressMax8, 0); */
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

        void Update()
		{
           
            _oscOut.Send(addressMax1, lights.l1 );
            _oscOut.Send(addressMax2, lights.l2 );
            _oscOut.Send(addressMax3, lights.l3 );
            _oscOut.Send(addressMax4, lights.l4 );
           /* _oscOut.Send(addressMax5, script4.Data[1].x );
            _oscOut.Send(addressMax6, script4.Data[1].y );
            _oscOut.Send(addressMax7, script4.Data[1].z );
            _oscOut.Send(addressMax8, script4.Data[1].w );    */

        }
	}
}