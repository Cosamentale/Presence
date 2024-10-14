using RenderHeads.Media.AVProLiveCamera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoOsc : MonoBehaviour
{
    public Material mat;
    public WebcamCompo01Compute script2;
    public InfraredDetectionFrameLine script3;
    public float speed;
    void Start()
    {
        
    }

   
    void Update()
    {
        float c1 = Time.time * speed;
        float c2 = Time.time * speed + 457f;
        float c3 = Time.time * speed + 332f;
        mat.SetFloat("_c1", c1);
        mat.SetFloat("_c2", c2);
        mat.SetFloat("_c3", c3);
        mat.SetFloat("_c4", Time.time* speed + 56.8f);
        script2._c1 = c1;
        script2._c2 = c2;
        script2._c3 = c3;
        script3._c1 = c1;
    }
}
