using OscSimpl.Examples;
using UnityEngine;

public class PrefabManageryolo : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    private GameObject currentPrefabInstance;
    public bool isPrefabActive;
    public SedingOscSpout oscScript;

    void Update()
    {
        if (isPrefabActive && currentPrefabInstance == null)
        {
            currentPrefabInstance = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);

            if (oscScript != null)
            {
                VisualizerYolo yoloScript = currentPrefabInstance.GetComponent<VisualizerYolo>();
                if (yoloScript != null) oscScript.yolo = yoloScript;
            }
        }
        else if (!isPrefabActive && currentPrefabInstance != null)
        {
            Destroy(currentPrefabInstance);
        }
    }
}
