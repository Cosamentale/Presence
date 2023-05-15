using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatordefaultbuffer : MonoBehaviour
{
    public float scriptTime;
    public float ActivationTime;
    public float DesactivationTime;
    public CameraSetup script;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Time.time > scriptTime)
        {
            script.enabled = true;
        }
        if (Time.time > ActivationTime)
        {
            GetComponent<defaultbuffer>().enabled = true;
        }
        if (Time.time > DesactivationTime)
        {
            GetComponent<defaultbuffer>().enabled = false;
        }
    }
}
