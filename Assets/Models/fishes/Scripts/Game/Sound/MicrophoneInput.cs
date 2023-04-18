using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    public Text txt;
    public float sensitivity = 50.0f;
    public float loudness = 0.0f;
    public float frequency = 0.0f;
    public int samplerate = 11024;
    public AudioSource aud;

    void Start()
    {
        if(Microphone.devices.Length > 0)
        {
            AudioSettings.outputSampleRate = samplerate;
            aud = GetComponent<AudioSource>();

            aud.clip = Microphone.Start("Built-in Microphone", true, 10, samplerate);
            aud.loop = true; // Set the AudioClip to loop
            aud.mute = false; // Mute the sound, we don't want the player to hear it

            while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
            aud.Play(); // Play the audio source!
        } else
        {
            gameObject.SetActive(false);
            Debug.Log("no microphone found");
        }
    }

    void Update()
    {
        loudness = GetAveragedVolume() * sensitivity;
        frequency = GetFundamentalFrequency();
    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        aud.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }

    float kprev = 0;
    float GetFundamentalFrequency()
    {
        float fundamentalFrequency = 0.0f;
        float[] data = new float[8192];
        aud.GetSpectrumData(data, 0, FFTWindow.BlackmanHarris);
        
        float s = 0.0f;
        int i = 0;
        float k = 0;

        for (int j = 1; j < 8192; j++)
        {
            if (s < data[j])
            {
                s = data[j];
                i = j;
                
            } 
        }
        for (int j = 1; j < 8192; j++)
        {
            if (data[j] >= s - 0.005f && data[j] <= s + 0.005f)
            {
                k++;
            }
        }
        
        fundamentalFrequency = i * samplerate / 8192/2;
        if(fundamentalFrequency == 300)
        {
            /*if(k < 2)//if (Mathf.Abs(k - kprev) < 5)
            {
                txt.text = "freq " + fundamentalFrequency + ", K:" + k + ", air";
            }
            else
            {
                txt.text = "freq " + fundamentalFrequency + ", K:" + k + ", water";
            }*/
            txt.text = "freq " + fundamentalFrequency + ", K:" + k + ", water";
            kprev = k;
        } else
        {
            txt.text = "freq " + fundamentalFrequency;
        }
        
        //txt.text = "freq "+fundamentalFrequency + ", K:"+k;
        return fundamentalFrequency;
    }
}

/*public class MicScript : MonoBehaviour {

    // Use this for initialization
    AudioSource aud;
    public Text txt;

    void Start () {
        /*if (Microphone.devices.Length > 0)
        {*/
/*aud = GetComponent<AudioSource>();
aud.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
aud.loop = true;
while (!(Microphone.GetPosition(null) > 0)) { }
//aud.Play();

//}
}

/*public double frequency = 440;
public double gain = 0.05;

private double increment;
private double phase;
private double sampling_frequency = 48000;

void OnAudioFilterRead(float[] data, int channels)
{
// update increment in case frequency has changed
increment = frequency * 2 * Math.PI / sampling_frequency;
for (var i = 0; i < data.Length; i = i + channels)
{
    phase = phase + increment;
    // this is where we copy audio data to make them “available” to Unity
    data[i] = (float)(gain * Math.Sin(phase));
    // if we have stereo, we copy the mono data to each channel
    if (channels == 2) data[i + 1] = data[i];
    if (phase > 2 * Math.PI) phase = 0;
}
}
}*/
