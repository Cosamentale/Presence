using UnityEngine;

public class RunInBackground : MonoBehaviour
{
    void Awake()
    {
        // Ensure the application keeps running in the background
        Application.runInBackground = true;
    }
}
