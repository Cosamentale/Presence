using UnityEngine;
using System.Collections;
using TMPro;
using System;
public class InfraredDetectionFrame : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    RenderTexture C;
    RenderTexture D;
    //ComputeBuffer t3Buffer;
    ComputeBuffer t3Buffer2;
    public Material material;
    public float blur;
    public float[] floatArray1 = new float[3];
    int handle_main;
    int handle_main2;
    int handle_t3;
    public Material mat;
    public Texture tex;
    public Material mat2;
    public Texture tex2;
    public Material mat3;
    public Texture tex3;
    public float _taille1;
    public float _taille2;
    public float floatA;
    private bool isFloatATriggered = false;
    private float previousfloat;
    private float lastIncreaseTime = -1f;
    public TextMeshPro textMesh;
    private string date;
    public float SecondPhase;
    public float TroisiemePhase;
    public float _c1;
    public float value = 0f;
    private float lastIncrementTime = 0f;
    public float v1 = 0f;
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        A = new RenderTexture(64, 64, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(64, 64, 0);
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
        t3Buffer2 = new ComputeBuffer(3, sizeof(float));
        handle_main = compute_shader.FindKernel("CSMain");
        handle_t3 = compute_shader.FindKernel("CSMain_t3");
        handle_main2 = compute_shader.FindKernel("CSMain2");
    }

    void Update()
    {
        
        if (Mathf.Floor(_c1)> value)
        {
            value = Mathf.Floor(_c1);
            lastIncrementTime = Time.time;
            v1 = 0;
        }

        
        if (Time.time - lastIncrementTime > 0.1f)
        {
         
            v1 = 1;
        }

        tex = mat.GetTexture("_MainTex");
        tex2 = mat2.GetTexture("_MainTex");
        tex3 = mat3.GetTexture("_MainTex");
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2",D);
        compute_shader.SetFloat("_blur", blur);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetFloat("_resx", 64);
        compute_shader.SetFloat("_resy", 64);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

        // generate t3

        compute_shader.SetTexture(handle_t3, "reader", B);
        //compute_shader.SetBuffer(handle_t3, "t3Buffer", t3Buffer);
       // compute_shader.Dispatch(handle_t3, 1, 1, 1);
        compute_shader.SetBuffer(handle_t3, "t3Buffer2", t3Buffer2);
        compute_shader.Dispatch(handle_t3, 4, 1, 1);

        // set material texture
      //  material.SetTexture("_MainTex", B);
       // material.SetTexture("_MainTex2", _InfraredManager.GetInfraredTexture());

        float[] t3Data2 =  new float[3]; ;
        t3Buffer2.GetData(t3Data2, 0, 0, 3);
        floatArray1 = t3Data2;

        material.SetFloat("_float1", floatArray1[0]);
        material.SetFloat("_float2", floatArray1[1]);
        material.SetFloat("_float3", floatArray1[2]);
    

        if (floatArray1[2] != previousfloat)
        {
            previousfloat = floatArray1[2];
            if (Time.time - lastIncreaseTime >= 1f)
            {
                floatA += Mathf.Lerp(0, 1, isFloatATriggered ? 0 : 1);
                isFloatATriggered = !isFloatATriggered;
                lastIncreaseTime = Time.time;
                DateTime dt = DateTime.Now;
                 date = dt.ToString("MM / dd / yyyy   ddd   HH : mm : ss").ToUpper();
            }
        }

        compute_shader.SetTexture(handle_main2, "reader", C);
        compute_shader.SetTexture(handle_main2, "reader2", tex);
        compute_shader.SetTexture(handle_main2, "reader3", tex2);
        compute_shader.SetTexture(handle_main2, "reader4", tex3);
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetFloat("_floatA", floatA);
        compute_shader.SetFloat("_float1", floatArray1[0]);
        compute_shader.SetFloat("_float2", floatArray1[1]);
        compute_shader.SetFloat("_float3", floatArray1[2]);
        compute_shader.SetFloat("_taille1", _taille1);
        compute_shader.SetFloat("_taille2", _taille2);
        compute_shader.SetFloat("_resx", 1920);
        compute_shader.SetFloat("_resy", 1080);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetFloat("_value",v1);
        compute_shader.SetFloat("_secondPhase", SecondPhase );   //* (1 - TroisiemePhase)
        //compute_shader.SetFloat("_troisiemePhase", TroisiemePhase);
        compute_shader.SetTexture(handle_main2, "writer", D);
        compute_shader.Dispatch(handle_main2, D.width / 8, D.height / 8, 1);
        compute_shader.SetTexture(handle_main2, "reader", D);
        compute_shader.SetTexture(handle_main2, "writer", C);
        compute_shader.Dispatch(handle_main2, D.width / 8, D.height / 8, 1);
        
        material.SetTexture("_MainTex", D);
        material.SetTexture("_MainTex2", B);
        material.SetFloat("_taille2", _taille2);
        material.SetFloat("_floatA", floatA);
        material.SetFloat("_secondPhase", SecondPhase);
        material.SetFloat("_troisiemePhase", TroisiemePhase);
        textMesh.text = date;
    }
    private void OnDisable()
    {
        floatArray1[2] = 0;
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
    private void OnEnable()
    {
        // Check if buffer and texture are null, and if so, recreate them
        if (t3Buffer2 == null)
        {
            t3Buffer2 = new ComputeBuffer(4, sizeof(float));
        }

        if (A == null)
        {
            A = new RenderTexture(64, 64, 0);
            A.enableRandomWrite = true;
            A.Create();
        }
        if (B == null)
        {
            B = new RenderTexture(64, 64, 0);
            B.enableRandomWrite = true;
            B.Create();
        }
        if (C == null)
        {
            C = new RenderTexture(1920, 1080, 0);
            C.enableRandomWrite = true;
            C.Create();
        }
        if (D == null)
        {
            D = new RenderTexture(1920, 1080, 0);
            D.enableRandomWrite = true;
            D.Create();
        }
    }

    }