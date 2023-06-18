using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProLiveCamera;
using UnityEngine.SceneManagement;
public class Activatordefaultbuffer : MonoBehaviour
{
    public float scriptTime;
    public float OscTime;
    public float ActivationTime;
    public float DesactivationTime;
    public CameraSetup script;
    public GameObject OSC;
    public AVProLiveCamera cam1;
    public AVProLiveCamera cam2;
    public AVProLiveCamera cam3;
    public float restartTime;
    void Start()
    {
        restartTime = Time.time;
    }

    
    void Update()
    {
        if (Time.time- restartTime > scriptTime)
        {
            script.enabled = true;
        }
        if (Time.time- restartTime > OscTime)
        {
            OSC.SetActive(true);
            script.enabled = false;
            cam1._updateSettings = false;
            cam2._updateSettings = false;
            cam3._updateSettings = false;

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            RestartScene2();
        }
        /* if (Time.time > ActivationTime)
         {
             GetComponent<defaultbuffer>().enabled = true;
         }
         if (Time.time > DesactivationTime)
         {
             GetComponent<defaultbuffer>().enabled = false;
         }   */
    }
    void RestartScene()
    {
        // Reload the currently active scene
        // Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
         UnityEngine.SceneManagement.SceneManager.LoadScene("scene03");
      //  Application.LoadLevel("scene03");
    }
    void RestartScene2()
    {
        // Reload the currently active scene
        // Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene("sceneinstall");
        //  Application.LoadLevel("scene03");
    }
}
