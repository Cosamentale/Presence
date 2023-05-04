
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class sonsmaster : MonoBehaviour
{


    [Header("Volume / Frequency")]
    [Range(0.0f, 20.0f)]
    public float masterVolume = 0.5f;
    [Range(0.0f, 4)]
    public float modif = 0.5f;
    public float smoothingTime = 0.1f; // time over which to average modif values
    private Queue<float> modifQueue = new Queue<float>(); // queue to store last N modif values
    private float smoothedModif = 1.0f; // smoothed value of modif
    float mainFrequencyPreviousValue;
    private System.Random RandomNumber = new System.Random();
    //public Component obj;
    private double sampleRate;

    private double dataLen;
    double chunkTime;
    double dspTimeStep;
    double currentDspTime;
    //public InfraredSourceCompute arr;
    public InfraredSourceDessin arr;
    void Awake()
    {

        sampleRate = AudioSettings.outputSampleRate;
    }
    void Update()
    {
        // add current modif value to queue
        modifQueue.Enqueue(modif);
        if (modifQueue.Count > 64)
        {
            modifQueue.Dequeue();
        }

        // compute moving average of modif values
        float sum = 0.0f;
        foreach (float m in modifQueue)
        {
            sum += m;
        }
        smoothedModif = sum / modifQueue.Count;
    }

    float fract(float s) { return s - Mathf.Floor(s); }
    float rd(float s) { return fract(Mathf.Sin(Vector2.Dot(new Vector2(Mathf.Floor(s), 0.0f), new Vector2(12.654f, 0.0f))) * 4032.326f); }
    float no(float s) { return Mathf.Lerp(rd(s), rd(s + 1.0f), Mathf.SmoothStep(0.0f, 1.0f, fract(s))); }
    float hash(float x) { return fract(Mathf.Sin(x) * 897612.531f); }

    float inst2(float t)
    {
      float r = 0;
        float r2 = 0;
        for (int i = 0; i < arr.taille; i++)
        {
           // r += (no(t*((i+1)*arr.floatArray1[i]*modif+30))-0.5f);
            r += (no(t * ((i + 1) * 100 * modif+30)) - 0.5f) * arr.floatArray1[i];
            r2 += (fract(t * ((i + 1) * 100 * modif + 30)) - 0.5f) * arr.floatArray1[i];
        }       
        return (r*0.8f+r2*0.2f)/ arr.taille;  
    }
    void OnAudioFilterRead(float[] data, int channels)
    {

        currentDspTime = AudioSettings.dspTime;
        dataLen = data.Length / channels;
        chunkTime = dataLen / sampleRate;
        dspTimeStep = chunkTime / dataLen;

        double preciseDspTime;
        for (int i = 0; i < dataLen; i++)
        {
            preciseDspTime = currentDspTime + i * dspTimeStep;

            float x = masterVolume * inst2((float)preciseDspTime);
            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] = x;
            }
        }

    }

}
