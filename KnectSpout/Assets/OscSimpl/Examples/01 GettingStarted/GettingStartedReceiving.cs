/*
	Created by Carl Emil Carlsen.
	Copyright 2016-2018 Sixth Sensor.
	All rights reserved.
	http://sixthsensor.dk
*/

using UnityEngine;

namespace OscSimpl.Examples
{
	public class GettingStartedReceiving : MonoBehaviour
	{
		[SerializeField] OscIn _oscIn;

		public string address1 = "/f7/f1";
		public string address2 = "/1/1";
        float _incomingFloat;
        private int Nbr_portIn;
        public Material mat;
        void Start()
		{
            Nbr_portIn = _oscIn.port;
			// Ensure that we have a OscIn component and start receiving on port 7000.
			if( !_oscIn ) _oscIn = gameObject.AddComponent<OscIn>();
			_oscIn.Open( Nbr_portIn);

        }


		void OnEnable()
		{
            // You can "map" messages to methods in two ways:

            // 1) For messages with a single argument, route the value using the type specific map methods.
            /////// EVENEMENT MAPPING_oscIn.MapFloat( address1, Event1 );
            _oscIn.MapFloat(address1, Event1);
            // 2) For messages with multiple arguments, route the message using the Map method.
            //_oscIn.Map( address2, OnCusto );
        }


		void OnDisable()
		{
			// If you want to stop receiving messages you have to "unmap".
			//_oscIn.UnmapFloat( OnTest1 );
			//_oscIn.Unmap( OnTest2 );
		}
        /*void Test1(OscMessage incomingMessage)
        {
            if (incomingMessage.TryGet(0, out _incomingFloat))
            {
                Debug.Log("ConditionOk");
            }    */
          //  Debug.Log("Received f7/f1" + "\n");
            
        

        void Event1( float value )
		{
            mat.SetFloat("Volume Accumulation 1", value); 
		}


	}
}