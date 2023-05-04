using UnityEngine;
using System.Collections;

public class InfraredSourceDetection : MonoBehaviour
{

    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    //ComputeBuffer t3Buffer;
    ComputeBuffer t3Buffer2;
    public Material material;
    public float blur;
    public float[] floatArray1 = new float[3];
    int handle_main;
    int handle_t3;
    /*private WebCamTexture webcamTexture;
    public int desiredWidth = 1280;
    public int desiredHeight = 720;   */
    public Material mat;
    public Texture tex;
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        A = new RenderTexture(64, 64, 0);
        A.enableRandomWrite = true;
        A.Create();
        B = new RenderTexture(64, 64, 0);
        B.enableRandomWrite = true;
        B.Create();
       // t3Buffer = new ComputeBuffer(1, sizeof(float));
        t3Buffer2 = new ComputeBuffer(3, sizeof(float));
        handle_main = compute_shader.FindKernel("CSMain");
        handle_t3 = compute_shader.FindKernel("CSMain_t3");
       /* webcamTexture = new WebCamTexture("HD Pro Webcam C920", desiredWidth, desiredHeight, 30);
       // webcamTexture.requestedWidth = desiredWidth;
       // webcamTexture.requestedHeight = desiredHeight;
        webcamTexture.Play();  */

    }

    void Update()
    {

        tex = mat.GetTexture("_MainTex");

        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", tex);
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
        material.SetTexture("_MainTex", B);
        material.SetTexture("_MainTex2", tex);

        float[] t3Data2 =  new float[3]; ;
        t3Buffer2.GetData(t3Data2, 0, 0, 3);
        floatArray1 = t3Data2;

        material.SetFloat("_float1", floatArray1[0]);
        material.SetFloat("_float2", floatArray1[1]);
        material.SetFloat("_float3", floatArray1[2]);
    }
}