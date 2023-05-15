using UnityEngine;
using RenderHeads.Media.AVProLiveCamera;

public class CameraSetup : MonoBehaviour
{
 
    private void Start()
    {
       
        AVProLiveCameraDevice LiveCamera = AVProLiveCameraManager.Instance.GetDevice(0);

        for (int j = 0; j < LiveCamera.NumSettings; j++)
        {
            AVProLiveCameraSettingBase settingBase = LiveCamera.GetVideoSettingByIndex(j);
           
            settingBase.IsAutomatic = false;
            settingBase.SetDefault();
            // AVProLiveCameraSettingBase expo = LiveCamera.GetVideoSettingByIndex(10);



        }
        AVProLiveCameraDevice LiveCamera2 = AVProLiveCameraManager.Instance.GetDevice(1);

        for (int j = 0; j < LiveCamera2.NumSettings; j++)
        {
            AVProLiveCameraSettingBase settingBase = LiveCamera2.GetVideoSettingByIndex(j);
            
            settingBase.IsAutomatic = false;
            settingBase.SetDefault();
        }
        AVProLiveCameraDevice LiveCamera3 = AVProLiveCameraManager.Instance.GetDevice(2);

        for (int j = 0; j < LiveCamera3.NumSettings; j++)
        {
            AVProLiveCameraSettingBase settingBase = LiveCamera3.GetVideoSettingByIndex(j);
           
            settingBase.IsAutomatic = false;
            settingBase.SetDefault();
        }
    }
   
}