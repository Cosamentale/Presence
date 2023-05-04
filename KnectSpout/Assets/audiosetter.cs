using UnityEngine;

public class audiosetter : MonoBehaviour
{
    public int sampleRate = 44100;
    public int bufferSize = 1024;

    void Awake()
    {
        AudioSettings.outputSampleRate = sampleRate;
       // AudioSettings.bufferSize = bufferSize;
    }
}