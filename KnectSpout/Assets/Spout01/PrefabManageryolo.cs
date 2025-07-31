using OscSimpl.Examples;
using UnityEngine;

public class PrefabManageryolo : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    private GameObject currentPrefabInstance;
    public bool isPrefabActive;
    public SedingOscSpout oscScript;
    public int nbmax;
    // Enum to select which yolo to assign
    public YoloTarget yoloTarget;

    public enum YoloTarget
    {
        Yolo1,
        Yolo2,
        Yolo3
    }

    void Update()
    {
        if (isPrefabActive && currentPrefabInstance == null)
        {
            currentPrefabInstance = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            if (oscScript != null)
            {
                VisualizerYolo yoloScript = currentPrefabInstance.GetComponent<VisualizerYolo>();
                if (yoloScript != null)
                {
                    switch (yoloTarget)
                    {
                        case YoloTarget.Yolo1:
                            oscScript.yolo1 = yoloScript;
                            break;
                        case YoloTarget.Yolo2:
                            oscScript.yolo2 = yoloScript;
                            break;
                        case YoloTarget.Yolo3:
                            oscScript.yolo3 = yoloScript;
                            break;
                    }
                }
            }
        }
        else if (!isPrefabActive && currentPrefabInstance != null)
        {
            Destroy(currentPrefabInstance);
        }

        if (currentPrefabInstance != null)
        {
            VisualizerYolo yoloScript = currentPrefabInstance.GetComponent<VisualizerYolo>();
            if (yoloScript != null)
            {
                yoloScript.nbmax = nbmax;
            }
        }
    }
}
