using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool launch;
    public GameObject detector;
    public GameObject scene1;
    public GameObject scene2;
    public GameObject scene2b;
    public GameObject scene3;
    public GameObject scene4;
    public GameObject scene5;
    public Material matScene1;
    public Material matScene2;
    public Material matscene3;
    public Material matscene4;
    public Material matscene5;
    public Material matscene52;
    private float timer = 0f;
    public float tempsFondu; 
    public float timingBlur;
    public float timingMergeScene01;
    public float timingDecoupeScene01;
    public float timingPowerScene01;
    public float timingInvertScene01;
    public float timingDitherScene01;
    public float scene02;
    public float timingInvertScene02;
    public float line;
    public float scene03;
    public float switchToDetec;
    public float phase01Scene02;
    public float phase02Scene02;
    public float retourline;
    public float launchcompo;
    public float launchface;
    public float apparition;
    public float imgfond;
    public float decoupe;
    public float decoupefinal;
    public float effetfinal;
    public float fin;
    public float compo1;
    public float compo2;
    public float ds1;
    public float bs1;
    public float ts1;
    public bool canReactivateLaunch;
    public float timeSinceDeactivation;
    public float activate;
    public float activationScene02;
    public float dmx; 
    public float launchtime;
    public Face script4;
    private float ttimingMergeScene01 ;
    private float ttimingDecoupeScene01 ;
    private float ttimingPowerScene01 ;
    private float ttimingInvertScene01;
    private float ttimingDitherScene01;
    private float tscene02;
    private float ttimingInvertScene02 ;
    private float tline ;
    private float tscene03 ;
    private float tswitchToDetec ;
    private float tphase01Scene02;
    private float tphase02Scene02;
    private float tretourline;
    private float tlaunchcompo ;
    private float tlaunchface;
    private float tapparition ;
    private float timgfond;
    private float tdecoupe;
    private float tdecoupefinal ;
    private float teffetfinal ;
    private float tfin ;
    public GameObject canvas;
    void Start()
    {

        activate = 0;
        activationScene02 = 0;
        launch = false;
        matScene1.SetFloat("_bluractivation",0);
        matScene1.SetFloat("_fondu",0);
        matScene1.SetFloat("_step0to1", 0);
        matScene1.SetFloat("_step1to2", 0);
        matScene1.SetFloat("_powermodification", 0);
        matScene1.SetFloat("_step2invert",0);
        matScene1.SetFloat("_dither", 0);
        matScene1.SetFloat("_final", 0);
        canvas.SetActive(true);
        scene3.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
        scene3.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
        matscene3.SetFloat("_dither",0);
        matscene5.SetFloat("_float1", 0);
        matscene5.SetFloat("_float2", 0);
        matscene52.SetFloat("_float2", 0);
        matscene5.SetFloat("_float3", 0);
        matscene5.SetFloat("_float4", 0);
        matscene3.SetFloat("_dither", 0);

        launchtime = Time.time;
        compo1 = 0;
        compo2 = 0;
        ds1 = 0;
        bs1 = 0;
        ts1 = 0;
        script4.final = 0;
          ttimingMergeScene01 = timingBlur+ timingMergeScene01;
          ttimingDecoupeScene01 = ttimingMergeScene01+ timingDecoupeScene01;
          ttimingPowerScene01= ttimingDecoupeScene01+ timingPowerScene01;
          ttimingInvertScene01= ttimingPowerScene01+ timingInvertScene01;
          ttimingDitherScene01= ttimingInvertScene01+ timingDitherScene01;
          tscene02= ttimingDitherScene01+ scene02;
          ttimingInvertScene02= tscene02+ timingInvertScene02;
          tline= ttimingInvertScene02+ line;
          tscene03= tline+ scene03;
          tswitchToDetec= tscene03+ switchToDetec;
          tphase01Scene02= tswitchToDetec+ phase01Scene02;
          tphase02Scene02= tphase01Scene02+ phase02Scene02;
          tretourline= tphase02Scene02+ retourline;
          tlaunchcompo= tretourline+ launchcompo;
          tlaunchface= tlaunchcompo+ launchface;
          tapparition= tlaunchface+ apparition;
          timgfond= tapparition+ imgfond;
          tdecoupe= timgfond+ decoupe;
          tdecoupefinal= tdecoupe+ decoupefinal;
          teffetfinal= tdecoupefinal+ effetfinal;
          tfin= teffetfinal+ fin;
}

    void Update()
    {
        float tt = Time.time - launchtime;
        if(Mathf.Floor(tt) >1)
        {
            detector.SetActive(true);
            dmx = 0;
        }
        if (detector.GetComponent<Activation>().floatArray1[2] == 1)
        {
            if (canReactivateLaunch)
            {
                launch = true;
            }
          
        }       
       
        if (launch)
        {
            canReactivateLaunch = false;
            detector.SetActive(false);
            timer += Time.deltaTime;
            activate = 10;
            timeSinceDeactivation = 0;
            if (timer < tfin)
            {
                if (timer > tlaunchface)
                {
                    bs1 = 0;
                    scene5.SetActive(true);
                    scene4.SetActive(false);
                    if (timer > tapparition) { matscene5.SetFloat("_float1", Mathf.Clamp01(timer - tapparition)); }
                    if (timer > timgfond) { matscene5.SetFloat("_float2", Mathf.Clamp01(timer - timgfond));
                        matscene52.SetFloat("_float2", Mathf.Clamp01(timer - timgfond));
                    }
                    if (timer > tdecoupe) { matscene5.SetFloat("_float3", 1); ts1 = 1; }
                    if (timer > tdecoupefinal) { script4.final =1; }
                    if (timer > teffetfinal) { matscene5.SetFloat("_float4", Mathf.Clamp01(timer - teffetfinal)); }
                }
                else { 
                if (timer > tlaunchcompo)
                {
                    scene4.SetActive(true);
                    scene2.SetActive(false);
                        scene2b.SetActive(false);
                        bs1 = 1;
                    } 
                else {
                        if (timer > tretourline)
                        { ds1 = 0; compo2 = 0; scene3.SetActive(false); scene2b.SetActive(true); }
                        else {
                            if (timer > tswitchToDetec)
                            {
                                ds1 = 1;
                                matscene3.SetFloat("_dither", Mathf.Clamp01((timer - tswitchToDetec - 4) * 0.3f));
                                scene1.SetActive(false);
                                scene2.SetActive(false);
                                scene2b.SetActive(false);
                                scene3.SetActive(true);
                                activationScene02 = 1;
                                if (timer > tphase01Scene02)
                                {
                                    scene3.GetComponent<InfraredDetectionFrame>().SecondPhase = 1;
                                }
                                if (timer > tphase02Scene02)
                                {
                                    scene3.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 1;
                                }
                            }
                            else
                            {
                                ds1 = 0;
                                if (timer > tscene03)
                                {
                                    scene1.SetActive(true);
                                    scene2.SetActive(false);
                                    scene2b.SetActive(false);
                                    matScene1.SetFloat("_final", 1); compo2 = 1;
                                    canvas.SetActive(false);
                                }
                                else
                                {

                                    if (timer > tline)
                                    {
                                        scene1.SetActive(false);
                                        scene2.SetActive(true);
                                    }
                                    else
                                    {
                                        scene1.SetActive(true);


                                        if (timer < tscene02)
                                        {
                                            matScene1.SetFloat("_fondu", Mathf.Clamp01(timer / tempsFondu));
                                            if (timer > timingBlur)
                                            {
                                                matScene1.SetFloat("_bluractivation", Mathf.Clamp01(timer - timingBlur));
                                            }
                                            if (timer > ttimingMergeScene01)
                                            {
                                                matScene1.SetFloat("_step1to2", Mathf.Clamp01(timer - ttimingMergeScene01));
                                            }
                                            if (timer > ttimingDecoupeScene01)
                                            {
                                                matScene1.SetFloat("_step0to1", 1); compo1 = 1;
                                            }
                                            if (timer > ttimingPowerScene01)
                                            {
                                                matScene1.SetFloat("_powermodification", Mathf.Clamp01(timer - ttimingPowerScene01));
                                            }
                                            if (timer > ttimingInvertScene01)
                                            {
                                                matScene1.SetFloat("_step2invert", Mathf.Clamp01(timer - ttimingInvertScene01));
                                            }
                                            if (timer > ttimingDitherScene01)
                                            {
                                                matScene1.SetFloat("_dither", Mathf.Clamp01(timer - ttimingDitherScene01));
                                            }
                                        }
                                        else
                                        {
                                            matScene1.SetFloat("_step0to1", 0); compo1 = 0;
                                            matScene1.SetFloat("_powermodification", 0);
                                            if (timer > ttimingInvertScene02)
                                            {
                                                matScene1.SetFloat("_step2invert", 1 - Mathf.Clamp01(timer - ttimingInvertScene02));
                                                matScene1.SetFloat("_step1to2", 1 - Mathf.Clamp01(timer - ttimingInvertScene02));
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
         
            if (timer > tfin + 1)
            {
                activationScene02 = 0;
                scene3.SetActive(false);
                scene2.SetActive(false);
                scene2b.SetActive(false);
                scene1.SetActive(false);
                scene5.SetActive(false);
                scene4.SetActive(false);
                matScene1.SetFloat("_fondu", 0);
                matScene1.SetFloat("_step0to1", 0);
                matScene1.SetFloat("_step1to2", 0);
                matScene1.SetFloat("_powermodification", 0);
                matScene1.SetFloat("_step2invert", 0);
                matScene1.SetFloat("_dither", 0);
                matscene3.SetFloat("_dither", 0);
                matScene1.SetFloat("_final", 0);
                canvas.SetActive(true);
                matscene5.SetFloat("_float1", 0);
                matscene5.SetFloat("_float2", 0);
                matscene52.SetFloat("_float2", 0);
                matscene5.SetFloat("_float3",0); 
                matscene5.SetFloat("_float4", 0);
                matscene3.SetFloat("_dither", 0);
                script4.final = 0;
                scene3.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
                scene3.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
                detector.SetActive(true);
                timer = 0;
                activate = 0;
                launch = false;
                canReactivateLaunch = false;
                 compo1=0;
                 compo2 = 0;
                 ds1 = 0;
                 bs1 = 0;
                 ts1 = 0;
                RestartScene();
            }
            void RestartScene()
            {
                // Reload the currently active scene
                // Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                UnityEngine.SceneManagement.SceneManager.LoadScene("sceneinstallQuai");
                //  Application.LoadLevel("scene03");
            }
        }
        else
        {
            timeSinceDeactivation += Time.deltaTime;
            if (timeSinceDeactivation >= 10)
            {
                canReactivateLaunch = true;
              
            }
            else
            {
                canReactivateLaunch = false;
            }   
        }
    }
}
