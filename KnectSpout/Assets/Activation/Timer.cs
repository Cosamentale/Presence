using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool launch;
    public GameObject detector;
    public GameObject scene1;
    public GameObject scene2;
    public Material matScene1;
    private float timer = 0f;
    public float tempsFondu;
    public float timingMergeScene01;
    public float timingDecoupeScene01;
    public float timingPowerScene01;
    public float timingInvertScene01;
    public float timingDitherScene01;
    public float scene02;
    public float timingPowerScene02;
    public float timingInvertScene02;
    public float timingDitherScene02;
    public float timingFinalScene02;
    public float switchToDetec;
    public float phase01Scene02;
    public float phase02Scene02;
    public float fin;
    public bool canReactivateLaunch;
    public float timeSinceDeactivation;
    public float activate;
    public float activationScene02;
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
        scene2.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
        scene2.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
        
    }

    void Update()
    {
        if(Mathf.Floor(Time.time)>1)
        {
            detector.SetActive(true);
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
            detector.SetActive(false);
            timer += Time.deltaTime;
            activate = 10;
            timeSinceDeactivation = 0;
            if (timer < fin)
            {
                scene1.SetActive(true);
                if (scene1.activeSelf)
                {
                    matScene1.SetFloat("_fondu", Mathf.Clamp01(timer / tempsFondu));
                    if (timer < scene02)
                    {

                        matScene1.SetFloat("_step1to2", Mathf.Clamp01(timer - timingMergeScene01));
                        if (timer > timingDecoupeScene01)
                        {
                            matScene1.SetFloat("_step0to1", 1);
                        }
                        if (timer > timingPowerScene01)
                        {
                            matScene1.SetFloat("_powermodification",1);
                        }
                        if (timer > timingInvertScene01)
                        {
                            matScene1.SetFloat("_step2invert",1);
                          
                        }
                        if (timer > timingDitherScene01)
                        {
                            matScene1.SetFloat("_dither", 1);
                        }
                    }
                    else
                    {
                        matScene1.SetFloat("_step0to1", 0);
                        if (timer > timingPowerScene02)
                        {
                            matScene1.SetFloat("_powermodification", 1);
                        }
                        if (timer > timingInvertScene02)
                        {
                            matScene1.SetFloat("_step2invert", 1);
                        }
                        if (timer > timingDitherScene02)
                        {
                            matScene1.SetFloat("_dither", 1);
                        }
                        if (timer > timingFinalScene02)
                        {
                            matScene1.SetFloat("_final", 1);
                        }
                    }
                }
                if (timer > switchToDetec)
                {
                    scene1.SetActive(false);
                    scene2.SetActive(true);
                    activationScene02 = 1;
                    if (scene2.activeSelf)
                    {
                        if (timer > phase01Scene02)
                        {
                            scene2.GetComponent<InfraredDetectionFrame>().SecondPhase = 1;
                        }
                        if (timer > phase02Scene02)
                        {
                            scene2.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 1;
                        }
                    }
                }

            }
          /*  else
            {
                activationScene02 = 0;
                scene2.SetActive(false);
                    matScene1.SetFloat("_fondu", 0);
                    matScene1.SetFloat("_step0to1", 0);
                    matScene1.SetFloat("_step1to2", 0);
                    matScene1.SetFloat("_powermodification", 0);
                    matScene1.SetFloat("_step2invert", 0);
                    matScene1.SetFloat("_dither", 0);
                    matScene1.SetFloat("_final", 0);
                    scene2.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
                    scene2.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
               
            }  */
            if (timer > fin + 1)
            {
                activationScene02 = 0;
                scene2.SetActive(false);
                scene1.SetActive(false);
                matScene1.SetFloat("_fondu", 0);
                matScene1.SetFloat("_step0to1", 0);
                matScene1.SetFloat("_step1to2", 0);
                matScene1.SetFloat("_powermodification", 0);
                matScene1.SetFloat("_step2invert", 0);
                matScene1.SetFloat("_dither", 0);
                matScene1.SetFloat("_final", 0);
                scene2.GetComponent<InfraredDetectionFrame>().SecondPhase = 0;
                scene2.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 0;
                detector.SetActive(true);
                
                timer = 0;
                activate = 0;
                launch = false;
                canReactivateLaunch = false;
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
/*
  if (launch)
{
detector.SetActive(false);
timer += Time.deltaTime;
activate = 10;
if (timer < fin)
{
  scene1.SetActive(true);
  if (scene1.activeSelf)
  {
      matScene1.SetFloat("_fondu", Mathf.Clamp01(timer / tempsFondu));
      if (timer < scene02)
      {

          matScene1.SetFloat("_step1to2", Mathf.Clamp01(timer - timingMergeScene01));
          if (timer > timingDecoupeScene01)
          {
              matScene1.SetFloat("_step0to1", 1);
          }
          matScene1.SetFloat("_powermodification", Mathf.Clamp01(timer - timingPowerScene01));
          matScene1.SetFloat("_step2invert", Mathf.Clamp01(timer - timingInvertScene01));
          matScene1.SetFloat("_dither", Mathf.Clamp01(timer - timingDitherScene01));
      }
      else
      {
          matScene1.SetFloat("_step0to1", 0);
          matScene1.SetFloat("_powermodification", Mathf.Clamp01(timer - timingPowerScene02));
          matScene1.SetFloat("_step2invert", Mathf.Clamp01(timer - timingInvertScene02));
          matScene1.SetFloat("_dither", Mathf.Clamp01(timer - timingDitherScene02));
          if (timer > timingFinalScene02)
          {
              matScene1.SetFloat("_final", 1);
          }
      }
  }
  if (timer > switchToDetec)
  {
      scene1.SetActive(false);
      scene2.SetActive(true);
      activationScene02 = 1;
      if (scene2.activeSelf)
      {
          if (timer > phase01Scene02)
          {
              scene2.GetComponent<InfraredDetectionFrame>().SecondPhase = 1;
          }
          if (timer > phase02Scene02)
          {
              scene2.GetComponent<InfraredDetectionFrame>().TroisiemePhase = 1;
          }
      }
  }

}
*/