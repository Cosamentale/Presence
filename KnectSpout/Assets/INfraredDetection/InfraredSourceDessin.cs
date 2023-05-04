using UnityEngine;
using System.Collections;

public class InfraredSourceDessin : MonoBehaviour
{

    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    ComputeBuffer t3Buffer;
    ComputeBuffer t3Buffer2;
    public Material material;
    public float float1;
    public float reg01;
    public float reg02;
    public float reg03;
    public float blur;
    public int taille;
    public float[] floatArray1 = new float[5];
    int handle_main;
    int handle_t3;
    //private WebCamTexture webcamTexture;
    //public int desiredWidth = 1280;
    //public int desiredHeight = 720;
    public int renderWidth = 1920;
    public int renderHeight = 1080;
    public Material mat;
    public Texture tex;
    public float _c1;
    public float _c2;
    public float _c3;
    public float _c4;

    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        A = new RenderTexture(renderWidth, renderHeight, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(renderWidth, renderHeight, 0);
        B.enableRandomWrite = true;
        B.Create();
        t3Buffer = new ComputeBuffer(1, sizeof(float));
        t3Buffer2 = new ComputeBuffer(212, sizeof(float));
        handle_main = compute_shader.FindKernel("CSMain");
        handle_t3 = compute_shader.FindKernel("CSMain_t3");

       /* webcamTexture = new WebCamTexture("HD Pro Webcam C920");
        webcamTexture.requestedWidth = desiredWidth;
        webcamTexture.requestedHeight = desiredHeight;
        webcamTexture.Play(); */

        compute_shader.SetFloat("_resx", renderWidth);
        compute_shader.SetFloat("_resy", renderHeight);
    }

    void Update()
    {
        tex = mat.GetTexture("_MainTex");

        // generate A
        compute_shader.SetFloat("_time", Time.time);
        compute_shader.SetFloat("_reg01", reg01);
        compute_shader.SetFloat("_reg02", reg02);
        compute_shader.SetFloat("_reg03", reg03);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetFloat("_c2", _c2);
        compute_shader.SetFloat("_c3", _c3);
        compute_shader.SetFloat("_blur", blur);
        compute_shader.SetInt("_taille", taille);
        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2",tex);
        //compute_shader.SetFloat("_time", Time.frameCount);
        

        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

        // generate t3

        compute_shader.SetTexture(handle_t3, "reader", B);
        compute_shader.SetBuffer(handle_t3, "t3Buffer", t3Buffer);
       // compute_shader.Dispatch(handle_t3, 1, 1, 1);
        compute_shader.SetBuffer(handle_t3, "t3Buffer2", t3Buffer2);
        compute_shader.Dispatch(handle_t3, taille, 1, 1);

        // set material texture
        material.SetTexture("_MainTex", B);

        float[] t3Data = new float[1];
        t3Buffer.GetData(t3Data, 0, 0, 1);
        float1 = t3Data[0];
        float[] t3Data2 =  new float[taille]; ;
        t3Buffer2.GetData(t3Data2, 0, 0, taille);
        floatArray1 = t3Data2;

        material.SetFloat("_float1",float1);
    }
}