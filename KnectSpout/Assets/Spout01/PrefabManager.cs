using OscSimpl.Examples;
using UnityEngine;

public class PrefabManager : MonoBehaviour
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
                PoseNetSpout poseNetScript = currentPrefabInstance.GetComponent<PoseNetSpout>();
                if (poseNetScript != null) oscScript.script = poseNetScript;
            }
        }
        else if (!isPrefabActive && currentPrefabInstance != null)
        {
            Destroy(currentPrefabInstance);
        }
    }
}
