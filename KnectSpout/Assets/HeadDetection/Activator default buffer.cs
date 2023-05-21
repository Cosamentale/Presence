using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProLiveCamera;
public class Activatordefaultbuffer : MonoBehaviour
{
    public float scriptTime;
    public float OscTime;
    public float ActivationTime;
    public float DesactivationTime;
    public CameraSetup script;
    public GameObject OSC;
    public AVProLiveCamera cam1;
    public AVProLiveCamera cam2;
    public AVProLiveCamera cam3;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Time.time > scriptTime)
        {
            script.enabled = true;
        }
        if (Time.time > OscTime)
        {
            OSC.SetActive(true);
            script.enabled = false;
            cam1._updateSettings = false;
            cam2._updateSettings = false;
            cam3._updateSettings = false;

        }
        /* if (Time.time > ActivationTime)
         {
             GetComponent<defaultbuffer>().enabled = true;
         }
         if (Time.time > DesactivationTime)
         {
             GetComponent<defaultbuffer>().enabled = false;
         }   */
    }
}
