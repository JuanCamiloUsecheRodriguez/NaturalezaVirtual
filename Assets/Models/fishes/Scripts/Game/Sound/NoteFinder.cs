using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoteFinder : MonoBehaviour
{
    public GameObject audioInputObject;
    public float threshold = 1.0f;
    MicrophoneInput micIn;
    public Text txt;
    // Use this for initialization
    void Start()
    {
        if (audioInputObject == null)
            audioInputObject = GameObject.Find("MicMonitor");
        micIn = (MicrophoneInput)audioInputObject.GetComponent("MicrophoneInput");
    }

    // Update is called once per frame
    void Update()
    {
        int f = (int)micIn.frequency; // Get the frequency from our MicrophoneInput script

        //txt.text = "wl: " + (340.0f / f) + ", " + (1600.0f / f);
        /*if(Mathf.Round(f * 1.13333333333f) == 340)
        {
            txt.text = "AIR";
        } else
        {
            txt.text = "WATER";
        }*/

        /*if (f >= 261 && f <= 262) // Compare the frequency to known value, take possible rounding error in to account
        {
            txt.text = "Middle-C played!";
        }
        else
        {
            txt.text = "Play another note...";
        }*/
    }
}