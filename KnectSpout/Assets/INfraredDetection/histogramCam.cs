using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class histogramCam : MonoBehaviour
{
    public Texture tex;
    public Material mat;
    public Material histogramMat;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tex = mat.GetTexture("_MainTex");
        histogramMat.SetTexture("_MainTex", tex);
    }
}
