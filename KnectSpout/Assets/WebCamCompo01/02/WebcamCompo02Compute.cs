using UnityEngine;
using System.Collections;

public class WebcamCompo02Compute : MonoBehaviour
{
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;
    public Material material;
    int handle_main;
    private WebCamTexture webcamTexture;

    public int desiredWidth = 1920;
    public int desiredHeight = 1080;
    public float speed1 = 10;
    public float speed2 = 10;
    public float speed3 = 10;
    public float[] floatArray1 = new float[3];
    int handle_t3;
    ComputeBuffer t3Buffer2;
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, 1));
        A = new RenderTexture(512, 1024, 0);
        A.enableRandomWrite = true;
        A.Create();
        //A.filterMode = FilterMode.Point;
        B = new RenderTexture(512, 1024, 0);
        B.enableRandomWrite = true;
        B.Create();
        //B.filterMode = FilterMode.Point;
        handle_main = compute_shader.FindKernel("CSMain");
        handle_t3 = compute_shader.FindKernel("CSMain_t3");

        webcamTexture = new WebCamTexture(); //   "USB2.0 PC CAMERA"  "USB2.0 HD UVC WebCam"
        webcamTexture.requestedWidth = desiredWidth;
        webcamTexture.requestedHeight = desiredHeight;
        webcamTexture.Play();



        t3Buffer2 = new ComputeBuffer(3, sizeof(float));
        //material.SetTexture("_MainTex2", webcamTexture2);
    }
    float fract(float t) { return t - Mathf.Floor(t); }
    float rd(float t) { float f = Mathf.Floor(t); return fract(Mathf.Sin(Vector2.Dot(new Vector2(f, f), new Vector2(54.56f, 54.56f))) * 7845.236f); }
    void Update()
    {
      
            compute_shader.SetTexture(handle_main, "reader2", webcamTexture);
            compute_shader.SetTexture(handle_t3, "reader", webcamTexture);
            material.SetTexture("_MainTex2", webcamTexture);
        
      
        compute_shader.SetTexture(handle_main, "reader", A);

        compute_shader.SetFloat("_time", Time.frameCount);
        compute_shader.SetFloat("_speed1", speed1);
        compute_shader.SetFloat("_speed2", speed2);
        compute_shader.SetFloat("_speed3", speed3);
        compute_shader.SetFloat("_resx", 512);
        compute_shader.SetFloat("_resy", 1024);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);



        material.SetTexture("_MainTex", B);
        // material.SetTexture("_MainTex2", _InfraredManager.GetInfraredTexture());
        material.SetFloat("_frame", Time.frameCount);
        material.SetFloat("_speed1", speed1);
        material.SetFloat("_speed2", speed2);
        material.SetFloat("_speed3", speed3);
       // material.SetFloat("_f1", f1);


        compute_shader.SetBuffer(handle_t3, "t3Buffer2", t3Buffer2);
        compute_shader.Dispatch(handle_t3, 4, 1, 1);

        float[] t3Data2 = new float[3]; ;
        t3Buffer2.GetData(t3Data2, 0, 0, 3);
        floatArray1 = t3Data2;
    }
}