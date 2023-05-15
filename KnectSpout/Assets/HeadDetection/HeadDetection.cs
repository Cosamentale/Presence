using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDetection : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    public Material material;
    int handle_main;
    public Material mat;
    public Texture tex;
    public Material mat2;
    public Texture tex2;
    public Material mat3;
    public Texture tex3;
    public float activationTime;
    void Start()
    {
        A = new RenderTexture(128, 32, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(128, 32, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
        activationTime = Time.time;
    }

    void Update()
    {
        tex = mat.GetTexture("_MainTex");
        tex2 = mat2.GetTexture("_MainTex");
        tex3 = mat3.GetTexture("_MainTex");
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", tex);
        compute_shader.SetTexture(handle_main, "reader3", tex2);
        compute_shader.SetTexture(handle_main, "reader4", tex3);
        compute_shader.SetFloat("_time", Time.time-activationTime);
        compute_shader.SetFloat("_resx", 128);
        compute_shader.SetFloat("_resy", 32);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

        material.SetTexture("_MainTex", B);
        material.SetTexture("_tex", tex);
        material.SetTexture("_tex2", tex2);
        material.SetTexture("_tex3", tex3);
    }
}
