using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltraFace;
public class Face : MonoBehaviour
{
    public Material mat;
    public Texture tex;
    public Visualizer face;
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    int handle_main;
    public Material material;
    //public Vector4[] Data;
    void Start()
    {
        A = new RenderTexture(1920, 1080, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(1920, 1080, 0);
        B.enableRandomWrite = true;
        B.Create();
        handle_main = compute_shader.FindKernel("CSMain");
    }
    void Update()
    {
        tex = mat.GetTexture("_MainTex");
        face._source = tex;
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", tex);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetVectorArray("_data", face.Data);
        compute_shader.SetFloat("_resx", 1920);
        compute_shader.SetFloat("_resy", 1080);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

        material.SetTexture("_Texture", B);
    }
}
