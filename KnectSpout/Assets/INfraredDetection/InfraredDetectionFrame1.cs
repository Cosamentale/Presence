using UnityEngine;
using System.Collections;
using System;
public class InfraredDetectionFrame1 : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    RenderTexture C;
    RenderTexture D;

    public Material material;
 
    int handle_main;
    int handle_main2;
    public Material mat;
    public Texture tex;
    public Material mat2;
    public Texture tex2;
    public Material mat3;
    public Texture tex3;
    public float _c1;

    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        A = new RenderTexture(64, 32, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(64, 32, 0);
        B.enableRandomWrite = true;
        B.Create();
        //B.filterMode = FilterMode.Point;
        C = new RenderTexture(1920, 1080, 0);
        C.enableRandomWrite = true;
        C.Create();
        D = new RenderTexture(1920, 1080, 0);
        D.enableRandomWrite = true;
        D.Create();
        // t3Buffer = new ComputeBuffer(1, sizeof(float));
      
        handle_main = compute_shader.FindKernel("CSMain");
     
        handle_main2 = compute_shader.FindKernel("CSMain2");
    }

    void Update()
    {
        
   

        tex = mat.GetTexture("_MainTex");
        tex2 = mat2.GetTexture("_MainTex");
        tex3 = mat3.GetTexture("_MainTex");
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2",D);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetFloat("_resx", 64);
        compute_shader.SetFloat("_resy", 32);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);


        compute_shader.SetTexture(handle_main2, "reader", C);
        compute_shader.SetTexture(handle_main2, "reader2", tex);
        compute_shader.SetTexture(handle_main2, "reader3", tex2);
        compute_shader.SetTexture(handle_main2, "reader4", tex3);
        compute_shader.SetTexture(handle_main2, "reader5", B);
        compute_shader.SetFloat("_time", Time.time);      
        compute_shader.SetFloat("_resx", 1920);
        compute_shader.SetFloat("_resy", 1080);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetTexture(handle_main2, "writer", D);
        compute_shader.Dispatch(handle_main2, D.width / 8, D.height / 8, 1);
        compute_shader.SetTexture(handle_main2, "reader", D);
        compute_shader.SetTexture(handle_main2, "writer", C);
        compute_shader.Dispatch(handle_main2, D.width / 8, D.height / 8, 1);
        
        material.SetTexture("_MainTex", D);
       // material.SetTexture("_MainTex2", B);

    }
    private void OnDisable()
    {
        
        CleanupResources();
    }



    private void CleanupResources()
    {
       

        // Destroy the RenderTexture
        if (A != null)
        {
            Destroy(A);
        }
        if (B != null)
        {
            Destroy(B);
        }
        if (C != null)
        {
            Destroy(C);
        }
        if (D != null)
        {
            Destroy(D);
        }
    }
}