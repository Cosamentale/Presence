using RenderHeads.Media.AVProLiveCamera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoOsc : MonoBehaviour
{
    public Material mat;
    public float speed;
    void Start()
    {
        
    }

   
    void Update()
    {

       mat.SetFloat("_c1", Time.time*speed);
        mat.SetFloat("_c2", Time.time* speed + 457f);
        mat.SetFloat("_c3", Time.time* speed + 332f);
        mat.SetFloat("_c4", Time.time* speed + 56.8f);
    }
}
