using UnityEngine;
using System.Collections;

public class WebcamCompo01Compute : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    RenderTexture C;
    RenderTexture D;
    public Material material;
    int handle_main;
    int handle_main2;
    //private WebCamTexture webcamTexture;
    // private WebCamTexture webcamTexture2;
    public int desiredWidth = 1920; 
    public int desiredHeight = 1080;
    public float speed1 = 10;
    public float speed2 = 10;
    public float speed3 = 10;
    public int imgresx = 512;
    public int imgresy = 1024;
    public float[] floatArray1 = new float[3];
    int handle_t3;
    ComputeBuffer t3Buffer2;
    public RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;
    public Material mat1;
    public Texture tex1;
    public Material mat2;
    public Texture tex2;
    public Material mat3;
    public Texture tex3;
    public Texture texn;
    public float ti;
    public float _c1;
    public float _c2;
    public float _c3;
    public float _p1;
    public float _p2;
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        A = new RenderTexture(imgresx, imgresy, 0);
        A.enableRandomWrite = true;
        A.Create();
        //A.filterMode = FilterMode.Point;
        B = new RenderTexture(imgresx, imgresy, 0);
        B.enableRandomWrite = true;
        B.Create();
        C = new RenderTexture(1920, 1080, 0, rtFormat);
        C.enableRandomWrite = true;
        C.Create();
        D = new RenderTexture(1920, 1080, 0, rtFormat);
        D.enableRandomWrite = true;
        D.Create();
        //B.filterMode = FilterMode.Point;
        handle_main = compute_shader.FindKernel("CSMain");
        handle_t3 = compute_shader.FindKernel("CSMain_t3");
        handle_main2 = compute_shader.FindKernel("CSMain2");

        t3Buffer2 = new ComputeBuffer(3, sizeof(float));
        //material.SetTexture("_MainTex2", webcamTexture2);
        compute_shader.SetFloat("_resx", imgresx);
        compute_shader.SetFloat("_resy", imgresy);

        

        material.SetFloat("_resy", imgresy);
    }
     float fract(float t) { return t - Mathf.Floor(t); }
    float rd(float t) { float f = Mathf.Floor(t); return   fract(Mathf.Sin(Vector2.Dot(new Vector2(f, f), new Vector2(54.56f, 54.56f))) * 7845.236f); }
    void Update()
    {
        tex1 = mat1.GetTexture("_MainTex");
        tex2 = mat2.GetTexture("_MainTex");
        tex3 = mat3.GetTexture("_MainTex");
        float ran = rd(Time.time);
        if (ran < 0.25f)
        {
            _p1 = 0.125f;
            _p2 = 0.625f;
        }
        if (ran >= 0.25f && ran <0.5f)
        {
            _p1 = 0.375f;
            _p2 = 0.75f;
        }
        if (ran >= 0.55f && ran < 0.75f)
        {
            _p1 = 0.625f;
            _p2 = 0.25f;
        }
        if (ran >= 0.75f && ran <= 1)
        {
            _p1 = 0.875f;
            _p2 = 0.375f;
        }
      

        compute_shader.SetTexture(handle_main2, "reader4", D);
        compute_shader.SetTexture(handle_main2, "reader", tex1);
        compute_shader.SetTexture(handle_main2, "reader2", tex2);
        compute_shader.SetTexture(handle_main2, "reader3", tex3);
        compute_shader.SetTexture(handle_main2, "reader5", texn);
        compute_shader.SetTexture(handle_main2, "writer", C);
        compute_shader.Dispatch(handle_main2, C.width / 8, C.height / 8, 1);
        compute_shader.SetTexture(handle_main2, "reader4", C);
        compute_shader.SetTexture(handle_main2, "writer", D);
        compute_shader.Dispatch(handle_main2, C.width / 8, C.height / 8, 1);

        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", C);
        compute_shader.SetFloat("_time", Time.frameCount);
        compute_shader.SetFloat("_time2", Time.time);
        compute_shader.SetFloat("_p1", _p1);
        compute_shader.SetFloat("_p2", _p2);
        compute_shader.SetFloat("_speed1", speed1);                
        compute_shader.SetFloat("_speed2", speed2);
        compute_shader.SetFloat("_speed3", speed3);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
     
        material.SetTexture("_MainTex", B);
        material.SetTexture("_MainTex2", D);
        material.SetFloat("_frame", Time.frameCount);
        material.SetFloat("_speed1", speed1);
        material.SetFloat("_speed2", speed2);
        material.SetFloat("_speed3", speed3);

       
     
        material.SetFloat("_p1", _p1);
        material.SetFloat("_p2", _p2);
        compute_shader.SetTexture(handle_t3, "reader", B);
        compute_shader.SetBuffer(handle_t3, "t3Buffer2", t3Buffer2);
        compute_shader.Dispatch(handle_t3, 4, 1, 1);

        float[] t3Data2 = new float[3]; ;
        t3Buffer2.GetData(t3Data2, 0, 0, 3);
        floatArray1 = t3Data2;
    }
}