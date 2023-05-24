using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool launch;
    public GameObject detector;
    public GameObject scene1;
    public GameObject scene2;
    public GameObject scene3;
    public GameObject scene4;
    public GameObject scene5;
    public Material matScene1;
    public Material matScene2;
    public Material matscene3;
    public Material matscene4;
    public Material matscene5;
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
    void Start()
    {

        activate = 0;
        activationScene02 = 0;
        launch = false;
        matScene1.SetFloat("_fondu",0);
        matScene1.SetFloat("_step0to1", 0);
        matScene1.SetFloat("_step1to2", 0);
        matScene1.SetFloat("_powermodification", 0);
        matScene1.SetFloat("_step2invert",0);
        matScene1.SetFloat("_dither", 0);
        matScene1.SetFloat("_final", 0);
        scene3.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
        scene3.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
        matscene3.SetFloat("_dither",0);
        launchtime = Time.time;
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
            if (timer < fin)
            {
                if (timer > launchface)
                {
                    bs1 = 0;
                    scene5.SetActive(true);
                    scene4.SetActive(false);
                    if (timer > apparition) { matscene5.SetFloat("_float1", Mathf.Clamp01(timer - apparition)); }
                    if (timer > imgfond) { matscene5.SetFloat("_float2", Mathf.Clamp01(timer - imgfond)); }
                    if (timer > decoupe) { matscene5.SetFloat("_float3", 1); ts1 = 1; }
                    if (timer > decoupefinal) { script4.final =1; }
                    if (timer > effetfinal) { matscene5.SetFloat("_float4", Mathf.Clamp01(timer - effetfinal)); }
                }
                else { 
                if (timer > launchcompo)
                {
                    scene4.SetActive(true);
                    scene2.SetActive(false);
                        bs1 = 1;
                    } 
                else {
                        if (timer > retourline)
                        { ds1 = 0; compo2 = 0; scene3.SetActive(false); scene2.SetActive(true); }
                        else {
                            if (timer > switchToDetec)
                            {
                                ds1 = 1;
                                matscene3.SetFloat("_dither", Mathf.Clamp01((timer - switchToDetec - 4) * 0.3f));
                                scene1.SetActive(false);
                                scene2.SetActive(false);
                                scene3.SetActive(true);
                                activationScene02 = 1;
                                if (timer > phase01Scene02)
                                {
                                    scene3.GetComponent<InfraredDetectionFrame>().SecondPhase = 1;
                                }
                                if (timer > phase02Scene02)
                                {
                                    scene3.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 1;
                                }
                            }
                            else
                            {
                                ds1 = 0;
                                if (timer > scene03)
                                {
                                    scene1.SetActive(true);
                                    scene2.SetActive(false);
                                    matScene1.SetFloat("_final", 1); compo2 = 1;
                                }
                                else
                                {

                                    if (timer > line)
                                    {
                                        scene1.SetActive(false);
                                        scene2.SetActive(true);
                                    }
                                    else
                                    {
                                        scene1.SetActive(true);


                                        if (timer < scene02)
                                        {
                                            matScene1.SetFloat("_fondu", Mathf.Clamp01(timer / tempsFondu));
                                            if (timer > timingBlur)
                                            {
                                                matScene1.SetFloat("_bluractivation", Mathf.Clamp01(timer - timingBlur));
                                            }
                                            if (timer > timingMergeScene01)
                                            {
                                                matScene1.SetFloat("_step1to2", Mathf.Clamp01(timer - timingMergeScene01));
                                            }
                                            if (timer > timingDecoupeScene01)
                                            {
                                                matScene1.SetFloat("_step0to1", 1); compo1 = 1;
                                            }
                                            if (timer > timingPowerScene01)
                                            {
                                                matScene1.SetFloat("_powermodification", Mathf.Clamp01(timer - timingPowerScene01));
                                            }
                                            if (timer > timingInvertScene01)
                                            {
                                                matScene1.SetFloat("_step2invert", Mathf.Clamp01(timer - timingInvertScene01));
                                            }
                                            if (timer > timingDitherScene01)
                                            {
                                                matScene1.SetFloat("_dither", Mathf.Clamp01(timer - timingDitherScene01));
                                            }
                                        }
                                        else
                                        {
                                            matScene1.SetFloat("_step0to1", 0); compo1 = 0;
                                            matScene1.SetFloat("_powermodification", 0);
                                            if (timer > timingInvertScene02)
                                            {
                                                matScene1.SetFloat("_step2invert", 1 - Mathf.Clamp01(timer - timingInvertScene02));
                                                matScene1.SetFloat("_step1to2", 1 - Mathf.Clamp01(timer - timingInvertScene02));
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
         
            if (timer > fin + 1)
            {
                activationScene02 = 0;
                scene3.SetActive(false);
                scene2.SetActive(false);
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
                matscene5.SetFloat("_float1", 0);
                matscene5.SetFloat("_float2", 0);
                matscene5.SetFloat("_float3",0); 
                matscene5.SetFloat("_float4", 0);
                matscene3.SetFloat("_dither", 0);
                scene3.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
                scene3.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
                detector.SetActive(true);
                dmx = 0;
                timer = 0;
                activate = 0;
                launch = false;
                canReactivateLaunch = false;
                 compo1=0;
                 compo2 = 0;
                 ds1 = 0;
                 bs1 = 0;
                 ts1 = 0;
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
