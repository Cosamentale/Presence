using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    public Material material;
    int handle_main;
   
   // public float activationTime;
    public defaultbuffer script;
    public float _float1;
    public float _float2;
    public float _float3;
    public float _float4;
    public float _float5;
    public float _float6;
  
    void Start()
    {
        A = new RenderTexture(1920, 1080, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(1920, 1080, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
       // activationTime = Time.time;
    }

    void Update()
    {
       
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", script.tex);
        compute_shader.SetTexture(handle_main, "reader3", script.tex2);
        compute_shader.SetTexture(handle_main, "reader4", script.tex3);
        compute_shader.SetTexture(handle_main, "reader5", script.B);
        compute_shader.SetFloat("_time", Time.time );
        compute_shader.SetFloat("_resx", 1920);
        compute_shader.SetFloat("_resy", 1080);
        compute_shader.SetFloat("_float1", _float1);
        compute_shader.SetFloat("_float2", _float2);
        compute_shader.SetFloat("_float3", _float3);
        compute_shader.SetFloat("_float4", _float4);
        compute_shader.SetFloat("_float5", _float5);
        compute_shader.SetFloat("_float6", _float6);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

        material.SetTexture("_MainTex", B);
        material.SetTexture("_cam", script.tex);
        material.SetTexture("_cam2", script.tex2);
        material.SetTexture("_cam3", script.tex3);
        material.SetTexture("_m", script.B);
        //material.SetTexture("_Tex", script.tex);
        //material.SetTexture("_tex2", tex2);
        //material.SetTexture("_tex3", tex3);
    }
}
