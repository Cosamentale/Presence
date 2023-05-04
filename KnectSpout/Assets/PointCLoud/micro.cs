using UnityEngine;

public class micro : MonoBehaviour {

    public int microphoneIndex = 0;

    void Update()
    {
        string[] microphones = Microphone.devices;
        if (microphones.Length > 0 && microphoneIndex < microphones.Length)
        {
            string microphoneDevice = microphones[microphoneIndex];
            AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(microphoneDevice, true, 10, 44100);
        audioSource.Play();
        }
    }
}
