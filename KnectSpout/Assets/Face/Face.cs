using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltraFace;
public class Face : MonoBehaviour
{
    public Material mat;
    public Texture tex;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public Visualizer face;
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    int handle_main;
    public Material material;
    //public Material material2;
    //public Vector4[] Data;
    public float c1;
    public float c2;
    public float c3;
    public float c4;
    public float lastIncrementTime;
    public float value;
    public float final;
    float fract(float t) { return t - Mathf.Floor(t); }
    float rd(float t) { float f = Mathf.Floor(t*12); return fract(Mathf.Sin(Vector2.Dot(new Vector2(f, f), new Vector2(54.56f, 54.56f))) * 7845.236f); }
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
        //float r1 = rd(c1 + 65);
        float r2 = rd(c1 + 123);
        float r3 = rd(c2);
        //float r4 = rd(c3);
        float v1 = 0;
        if (Mathf.Floor(c3) > value)
        {
            value = Mathf.Floor(c3);
            lastIncrementTime = Time.time;
            v1 = 0;
        }


        if (Time.time - lastIncrementTime > 0.2f)
        {

            v1 = 1;
        }
        /*tex = mat.GetTexture("_MainTex");
        tex2 = mat2.GetTexture("_MainTex");
        tex3 = mat3.GetTexture("_MainTex"); */
        if (r2 < 0.75)
        {
            if (r2 < 0.5)           
            {
                if (r2 < 0.25)
                {
                    tex = mat.GetTexture("_MainTex");
                }
                else
                {
                    tex = mat2.GetTexture("_MainTex");
                }
            }
            else
            {
                tex = mat3.GetTexture("_MainTex");
            }           
        }
        else {
               
            tex = mat4.GetTexture("_MainTex");
        }
      

        face._source = tex;
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", tex);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetFloat("_c2", c2);
        compute_shader.SetFloat("_r3", r3);
        compute_shader.SetFloat("_r4", v1);
        compute_shader.SetFloat("_final", final);
        compute_shader.SetVectorArray("_data", face.Data);
        compute_shader.SetFloat("_resx", 1920);
        compute_shader.SetFloat("_resy", 1080);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        material.SetFloat("_r3", r3);
        material.SetFloat("_r4", v1);
        material.SetFloat("_c4", c4);
        material.SetTexture("_Texture", B);
        material.SetVectorArray("_data", face.Data);
        material.SetTexture("_cam", tex);
        //material2.SetVector("_data", face.Data[0]);
        //material2.SetTexture("_cam", tex);
    }
}
