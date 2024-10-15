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
    public float[] floatArray1 = new float[4];
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
    public float _p3;
    public float _p4;
    public bool phase2;
    public bool phase3;
    public float timePhase31 = 0;
    public float timePhase32 = 0;
    public bool hasValidated = false;
    public bool hasValidated2 = false;
    public bool hasValidated3 = false;
    public float _bluractivation;
    public float _step1to2;
    public float tic;
    public float solo;
    public float tsolo;
    public float _reg1;
    public float _reg2;
    public float activationtime;
    public float test;
    public float test2;
    public float lhv;
    public float final;
    public float bande;
    public float ts2;
    public PoseNetCompo posenet;
    private float smoothDampVelocity;
    private float smoothDampVelocity2;
    private float smoothingTime =1f;
    public float dp;
    public float dp2;
    //public float reactive = 0;
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

        t3Buffer2 = new ComputeBuffer(4, sizeof(float));
        compute_shader.SetFloat("_resx", imgresx);
        compute_shader.SetFloat("_resy", imgresy);
        compute_shader.SetFloat("_active", 1);
        material.SetFloat("_resy", imgresy);
        activationtime = Time.frameCount;
        solo = 0;
        tsolo = 0;
        timePhase31 = 0;
        timePhase32 = 0;
        compute_shader.SetFloat("_active", 0);
    }
     float fract(float t) { return t - Mathf.Floor(t); }
    float rd(float t) { float f = Mathf.Floor(t); return   fract(Mathf.Sin(Vector2.Dot(new Vector2(f, f), new Vector2(54.56f, 54.56f))) * 7845.236f); }
    float sm (float a, float b, float x){float t = Mathf.Clamp01((x - a) / (b - a));
    return t * t * (3 - 2 * t);}
    float no(float t) { return Mathf.Lerp(rd(t), rd(t + 1), sm(0.4f, 0.6f, fract(t))); }

    void Update()
    {
        tex1 = mat1.GetTexture("_MainTex");
        tex2 = mat2.GetTexture("_MainTex");
        tex3 = mat3.GetTexture("_MainTex");

        //float tt =  no(Time.time*0.25f)*3000+fract((Time.frameCount - activationtime)/1000)*1000;//test * 60;//
        float tt = Time.frameCount - activationtime;//test * 60;//
        //float pp1 = Mathf.Lerp(posenet.pos1[5].y, posenet.pos1[0].y+Mathf.Abs(posenet.pos1[5].y- posenet.pos1[0].y), Mathf.Sin(Time.time) * 0.5f + 0.5f);
        dp = Mathf.SmoothDamp(dp, posenet.pos1[0].y, ref smoothDampVelocity, smoothingTime);
        //float pp2 = Mathf.Lerp(posenet.pos1[5].x, posenet.pos1[6].x, Mathf.Sin(Time.time) * 0.5f + 0.5f);
        dp2 = Mathf.SmoothDamp(dp2, posenet.pos1[0].x, ref smoothDampVelocity2, smoothingTime);

        if (tt < 500) { ts2 = 0; }
        else { ts2 = 10; }
        float sp = speed3;
        float ff1 = 0;
        if (final < 0.5)
        {
             ff1 = Mathf.Floor(tt / speed1) * 10 / imgresy * sp ;
        }
        else { 
        speed1 =0.01f+0.7f*no(Time.time * 0.125f + 785);
        ff1 = no(Time.time * 0.125f) * 70 + fract(Mathf.Floor(tt / speed1) * 10 / imgresy * sp/10)*10;
        }
        if (fract(ff1)< 0.02f) { bande = 10; }
        else { bande = 0; }
        //float vi1 = 1.5f;
        float ff = ff1;
        //if (ff1 < 10/vi1) { ff =ff1 * vi1; } else { ff = ff1; }        
        if (fract(tt / speed2) > 0.05f){tic = 10;}
        else{tic = 0;}
        float state = 0;
        if (solo == 1 ) { tsolo += 1*sp; }
        else { tsolo = 0; }
        float td = 1f / 3f;
        float dt = 2f / 3f;           
        float t2 =0; float t3 = 0 ; float t4 = 0;

        if (ff < 21){
            phase2 = false;
            phase3 = false;
            float tt4 = ff / 9;
            if (fract(ff / 6) > 0.5f) { lhv = 0; } else { lhv = 1; }
            if (tt4 < 1){
                t2 = fract(ff / 4);
                t3 = fract(ff / 3);
                t4 = fract(tt4);}
            else{
                t2 = fract(ff / 12);
                t3 = fract(tt4 + dt);
                t4 = fract(ff / 18 - td);
                if (fract(ff / 3) < td) { solo = 0; }
                else { solo = 1; }}}
        else {
            if (ff < 37) {                
                phase2 = true;
                phase3 = false;
                if (fract(ff / 24) < 0.5f) { if (fract(ff / 6) < 0.5f) { lhv = 0; } else { lhv = 1; } }
            else { if (fract(ff / 6) < 0.5f) { lhv = 1; } else { lhv = 0; } }
            if (ff > 26){
                    t2 = fract((ff + 1) / 6);
                t3 = fract((ff + 1) / 12+ 5);
                if (fract((ff + 1) / 3) < td){solo = 0;}
                else { solo = 1; }}
            else{ solo = 0;
                if (ff < 25){
                    t2 = fract(ff / 2);
                    t3 = fract(ff / 4+ 0.25f);}
                else{
                    if (fract(ff / 8) < 0.5f){t2 = fract(ff / 8+ 1.5f);}
                    else { t2 = fract(ff / 4); }
                    t3 = fract(ff / 2);}}}
            else
            {
                phase3 = true;
                solo = 0;
            }
        }
       
        if (phase3 == true)
        {
            hasValidated2 = false;
            if (!hasValidated ){
                compute_shader.SetFloat("_active",0);
                hasValidated = true;}
            else{ compute_shader.SetFloat("_active", 1); }
            material.SetFloat("_phase3", 1);
            compute_shader.SetFloat("_phase3", 1);
            _p1 = 0.4f;_p2 = 0.7f;_p3 = 0.9f;_p4 = 0.1f;
            float ts = 0;
            float bmd = Mathf.Floor((timePhase31 + timePhase32+ts) / speed1) / imgresy*10;
            float ba = 1;
         
            if (bmd >= 6)
            {
                if (fract(ff / 12) > 0.5f) { lhv = 0; } else { lhv = 1; }
                ba = 3;
                if (fract(ff / 6) < td) { solo = 0; ts = 0; }
                else { solo = 1; ts += 1 * sp ; bmd = bmd - 1; }
            }
            test = ba;
            float md2 = bmd /2 ;
            float md = bmd /4 ;
            float md3 = fract(bmd /16+0.5f);
            if (md2 < 0.5){material.SetFloat("_phase3d", 0);}else{material.SetFloat("_phase3d", 1);}
            if (fract(md+0.5f) < 0.5) { material.SetFloat("_phase3st2", 1);compute_shader.SetFloat("_phase3st2", 1); }
            else { material.SetFloat("_phase3st2", 0); compute_shader.SetFloat("_phase3st2", 0); }
            if (md3 < 0.25f) { material.SetFloat("_phase3st3", 1); compute_shader.SetFloat("_phase3st3", 1); }
            else { material.SetFloat("_phase3st3", 0);compute_shader.SetFloat("_phase3st3", 0); }
            if (md3 < 0.25f) { material.SetVector("_poca", new Vector4(0.3f, 0.6f,0.7f, 1));
            compute_shader.SetVector("_poca", new Vector4(0.3f, 0.6f, 0.7f, 1));}
            if (md3 > 0.25f && t2 <= 0.5f) { material.SetVector("_poca", new Vector4(0, 0.3f, 0.4f, 0.7f));
                compute_shader.SetVector("_poca", new Vector4(0, 0.3f, 0.4f, 0.7f));}
            if (md3 > 0.5f && t2 <= 0.75f) { material.SetVector("_poca", new Vector4(0.2f, 0.5f, 0.6f, 0.9f));
                compute_shader.SetVector("_poca", new Vector4(0.2f, 0.5f, 0.6f, 0.9f));}
            if (md3 > 0.75f && t2 <= 1) { material.SetVector("_poca", new Vector4(0.1f, 0.4f, 0.5f, 0.8f));
                compute_shader.SetVector("_poca", new Vector4(0.1f, 0.4f, 0.5f, 0.8f));}
        if (fract(md2) < 0.5){
                material.SetFloat("_phase3st", 1);
                compute_shader.SetFloat("_phase3st", 1);
                timePhase31 +=  (1 - solo) * sp ;
                compute_shader.SetFloat("_timePhase31", timePhase31);
                material.SetFloat("_timePhase31", timePhase31);}
            else {
                material.SetFloat("_phase3st", 0);
                compute_shader.SetFloat("_phase3st", 0);
                timePhase32 +=  (1 - solo) * sp ;
                compute_shader.SetFloat("_timePhase32", timePhase32);
                material.SetFloat("_timePhase32", timePhase32);}
           
        }
        else
        {
            hasValidated = false;
            timePhase31 = 0;
            timePhase32 = 0;
            material.SetFloat("_phase3", 0);
            compute_shader.SetFloat("_phase3", 0);

            if (phase2 == true){
                if (!hasValidated2){compute_shader.SetFloat("_active",0);hasValidated2 = true;}
                else{compute_shader.SetFloat("_active", 1);}
                material.SetFloat("_phase2", 1);
                compute_shader.SetFloat("_phase2", 1);
                if (t3 < 0.5f){
                    material.SetFloat("_phase2v", 1);
                    compute_shader.SetFloat("_phase2v", 1);
                                  
                    if (t2 < 0.5f) {
                        material.SetFloat("_phase2st", 0);
                        compute_shader.SetFloat("_phase2st", 0);
                        _p2 = dp2;
                        _p4 = 1f - dp2;
                        _p1 = 0.2f;
                        _p3 = 0.7f;
                    }
                    else{
                        material.SetFloat("_phase2st", 1);
                        compute_shader.SetFloat("_phase2st", 1);
                        _p2 = 0.5f;
                        _p4 = 0.9f;
                        _p1 = dp2;
                        _p3 = 1f - dp2;
                    }
                }
                else{                    
                    material.SetFloat("_phase2v", 0);
                    compute_shader.SetFloat("_phase2v", 0);
                    if (t2 < 0.5f){
                        material.SetFloat("_phase2st", 1);
                        compute_shader.SetFloat("_phase2st", 1);
                        _p1 = 0.375f;
                        _p2 = dp;
                    }
                    else{
                        material.SetFloat("_phase2st", 0);
                        compute_shader.SetFloat("_phase2st", 0);
                        _p1 = 0.125f;_p2 = dp;}
                }
            }
            else
            {
                if (tt > 1) { compute_shader.SetFloat("_active", 1); } else { compute_shader.SetFloat("_active", 0); }
                hasValidated = false; hasValidated2 = false;
                material.SetFloat("_phase2", 0);
                compute_shader.SetFloat("_phase2", 0);
                material.SetFloat("_phase2v", 0);
                compute_shader.SetFloat("_phase2v", 0);
                if (t3 < dt){state = 0;
                    if (t2 < 0.25f){_p1 = 0.125f;_p2 = dp;}
                    if (t2 > 0.25f && t2 <= 0.5f){_p1 = 0.625f;_p2 = dp; }
                    if (t2 > 0.5f && t2 <= 0.75f){_p1 = 0.875f; _p2 = dp; }
                    if (t2 > 0.75f && t2 <= 1){_p1 = 0.375f;_p2 = dp; }}
                else{state = 1; _p2 = dp;
                    if (t4 <= td) { _p1 = 0.5f; }
                    if (t4 > td && t4 <= dt) { _p1 = 0.5f / 3f; }
                    if (t4 > dt && t4 <= 1) { _p1 = 2.5f / 3f; }
                }
            }
        }
        compute_shader.SetFloat("_reg1", _reg1);
        compute_shader.SetFloat("_reg2", _reg2);
        compute_shader.SetTexture(handle_main2, "reader4", D);
        compute_shader.SetTexture(handle_main2, "reader", tex1);
        compute_shader.SetTexture(handle_main2, "reader2", tex2);
        compute_shader.SetTexture(handle_main2, "reader3", tex3);
        compute_shader.SetTexture(handle_main2, "reader5", texn);
        compute_shader.SetFloat("_c1", _c1);
        compute_shader.SetFloat("_c2", _c2);
        compute_shader.SetFloat("_c3", _c3);
        compute_shader.SetTexture(handle_main2, "writer", C);
        compute_shader.Dispatch(handle_main2, C.width / 8, C.height / 8, 1);
        compute_shader.SetTexture(handle_main2, "reader4", C);
        compute_shader.SetTexture(handle_main2, "writer", D);
        compute_shader.Dispatch(handle_main2, C.width / 8, C.height / 8, 1);

        compute_shader.SetTexture(handle_main, "reader", A);
        compute_shader.SetTexture(handle_main, "reader2", C);
        compute_shader.SetFloat("_time", tt);
        compute_shader.SetFloat("_sp", sp);
        compute_shader.SetFloat("_ff", ff);
        compute_shader.SetFloat("_time2", Time.time);
        compute_shader.SetFloat("_p1", _p1);
        compute_shader.SetFloat("_p2", _p2);
        compute_shader.SetFloat("_p3", _p3);      
        compute_shader.SetFloat("_p4", _p4);
        compute_shader.SetFloat("_bluractivation", _bluractivation);
        compute_shader.SetFloat("_step1to2", _step1to2);
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
        material.SetFloat("_frame", tt);
        material.SetFloat("_speed1", speed1);
        material.SetFloat("_speed2", speed2);
        material.SetFloat("_speed3", speed3);
        material.SetFloat("_sp",sp);
        material.SetFloat("_ff", ff);
        material.SetFloat("_p1", _p1);
        material.SetFloat("_p2", _p2);
        material.SetFloat("_p3", _p3);
        material.SetFloat("_p4", _p4);
        material.SetFloat("_state", state);
        material.SetFloat("_solo", solo);
        material.SetFloat("_tsolo", tsolo);
        material.SetFloat("_lhv", lhv);
        compute_shader.SetFloat("_lhv", lhv);
        compute_shader.SetFloat("_tsolo", tsolo);
        compute_shader.SetFloat("_solo", solo);
        compute_shader.SetFloat("_state", state);
        compute_shader.SetTexture(handle_t3, "reader", C);
        compute_shader.SetTexture(handle_t3, "reader2", B);
        compute_shader.SetBuffer(handle_t3, "t3Buffer2", t3Buffer2);
        compute_shader.Dispatch(handle_t3, 5, 1, 1);
        float[] t3Data2 = new float[4]; ;
        t3Buffer2.GetData(t3Data2, 0, 0, 4);
        floatArray1 = t3Data2;
        posenet.Tex = C;
    }

    private void OnEnable()
    {
        activationtime = Time.frameCount;
        solo = 0;
        tsolo = 0;
        timePhase31 = 0;
        timePhase32 = 0;
        compute_shader.SetFloat("_active", 0);
    }
}