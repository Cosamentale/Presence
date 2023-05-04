using UnityEngine;
using System.Collections;

public class InfraredSourceblock : MonoBehaviour
{
    public GameObject InfraredSourceManager;
    private InfraredSourceManager _InfraredManager;
    public ComputeShader compute_shader;
    RenderTexture A;
    RenderTexture B;

    public Material material;

    int handle_main;


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

    }

    void Update()
    {
        if (InfraredSourceManager == null)
        {
            return;
        }

        _InfraredManager = InfraredSourceManager.GetComponent<InfraredSourceManager>();
        if (_InfraredManager == null)
        {
            return;
        }

        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", _InfraredManager.GetInfraredTexture());
        compute_shader.SetFloat("_time", Time.frameCount);
        compute_shader.SetFloat("_resx", 512);
        compute_shader.SetFloat("_resy", 1024);
        compute_shader.SetTexture(handle_main, "writer", B);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);
        compute_shader.SetTexture(handle_main, "reader", B);
        compute_shader.SetTexture(handle_main, "writer", A);
        compute_shader.Dispatch(handle_main, B.width / 8, B.height / 8, 1);

      

        material.SetTexture("_MainTex", B);
        material.SetTexture("_MainTex2", _InfraredManager.GetInfraredTexture());
        material.SetFloat("_frame", Time.frameCount);
    }
}