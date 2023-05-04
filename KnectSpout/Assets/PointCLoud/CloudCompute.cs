using UnityEngine;
using System.Collections;

public class CloudCompute : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    public Material material;
    public RenderTexture tex;
    int handle_main;
    public RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;
    public float r1;
    public float r2;
    public float r3;
    void Start()
    {
        A = new RenderTexture(1920, 1080, 0, rtFormat);
        A.enableRandomWrite = true;
        A.Create();
        //A.filterMode = FilterMode.Point;
        B = new RenderTexture(1920, 1080, 0, rtFormat);
        B.enableRandomWrite = true;
        B.Create();
        //B.filterMode = FilterMode.Point;
        handle_main = compute_shader.FindKernel("CSMain");
        compute_shader.SetFloat("_resx", 1920);
        compute_shader.SetFloat("_resy", 1080);
    }

    void Update()
    {

        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", tex);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetFloat("_r1", r1);
        compute_shader.SetFloat("_r2", r2);
        compute_shader.SetFloat("_r3", r3);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

        material.SetTexture("_MainTex", B);
    }
}