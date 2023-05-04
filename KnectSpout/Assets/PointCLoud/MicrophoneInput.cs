using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneInput : MonoBehaviour
{
    public AudioMixerGroup outputMixerGroup = null;
    public float sensitivity = 1.0f;
    public int microphoneIndex = 0; 
    public float bassVolume = 0;
    public float middleVolume = 0;
    public float highVolume = 0;
    public float bassAccumulation = 0;
    public float middAccumulation = 0;
    public float highAccumulation = 0;
    private AudioSource _audioSource;
    private float[] _spectrumData;

    public Material mat;
    public Material mat2;
    void Start()
    {
        string[] microphones = Microphone.devices;
        if (microphones.Length > 0 && microphoneIndex < microphones.Length)
        {
            string microphoneDevice = microphones[microphoneIndex];
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = outputMixerGroup;
            _audioSource.loop = true;
            _audioSource.clip = Microphone.Start(microphoneDevice, true, 1, AudioSettings.outputSampleRate);
            //_audioSource.mute = true; // Mute the audio playback
           // _audioSource.volume = 0.0001f;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No microphone found or microphone index out of range.");
        }
        _spectrumData = new float[1024]; // Set the size of the spectrum data buffer
    }

    void Update()
    {
        float[] samples = new float[_audioSource.clip.samples];
        _audioSource.clip.GetData(samples, 0);
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        float average = sum / samples.Length;
        float normalizedVolume = Mathf.Clamp01(average * sensitivity);
        _audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);
        int bassRangeStart = 1;
        int bassRangeEnd = Mathf.FloorToInt(_spectrumData.Length * 0.2f);
        int middleRangeStart = bassRangeEnd + 1;
        int middleRangeEnd = Mathf.FloorToInt(_spectrumData.Length * 0.6f);
        int highRangeStart = middleRangeEnd + 1;
        int highRangeEnd = _spectrumData.Length - 1;
        float bassSum = 0f;
        for (int i = bassRangeStart; i <= bassRangeEnd; i++)
        {
            bassSum += _spectrumData[i];
        }
        bassVolume =bassSum * sensitivity;

        float middleSum = 0f;
        for (int i = middleRangeStart; i <= middleRangeEnd; i++)
        {
            middleSum += _spectrumData[i];
        }
        middleVolume = middleSum * sensitivity*10;

        float highSum = 0f;
        for (int i = highRangeStart; i <= highRangeEnd; i++)
        {
            highSum += _spectrumData[i];
        }
        highVolume = highSum * sensitivity*1000;
        bassAccumulation += bassVolume*0.1f;
        middAccumulation += middleVolume * 0.1f;
        highAccumulation += highVolume * 0.1f;
        mat.SetFloat("_b", bassVolume);
        mat.SetFloat("_m", middleVolume);
        mat.SetFloat("_h", highVolume);
        mat.SetFloat("_ba", bassAccumulation);
        mat.SetFloat("_ma", middAccumulation);
        mat.SetFloat("_ha", highAccumulation);
        mat2.SetFloat("_b", bassVolume);
        mat2.SetFloat("_m", middleVolume);
        mat2.SetFloat("_h", highVolume);
        mat2.SetFloat("_ba", bassAccumulation);
        mat2.SetFloat("_ma", middAccumulation);
        mat2.SetFloat("_ha", highAccumulation);
    }
}

