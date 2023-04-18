using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfigScript : MonoBehaviour {

    public Text dificultadTxt;
    public Text tiempoTxt;
    public static int dificultad = 0;
    public static int tiempo = 60;
    public Vector3 target = Vector3.zero;

    public Slider difSlider, timeSlider;

    // Use this for initialization
    void Start () {
        if (ConfigScript.dificultad == 0)
        {
            difSlider.value = 0;
        } else if(ConfigScript.dificultad == 1)
        {
            difSlider.value = 1;
        } else
        {
            difSlider.value = 2;
        }

        if (ConfigScript.tiempo == 60)
        {
            timeSlider.value = 0;
        }
        else if (ConfigScript.tiempo == 90)
        {
            timeSlider.value = 1;
        }
        else
        {
            timeSlider.value = 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition += (target - transform.localPosition) / 5;
    }
    public void changeDiff(float f)
    {
        switch (f + "")
        {
            case "0":
                ConfigScript.dificultad = 0;
                dificultadTxt.text = "Fácil";
                break;
            case "1":
                ConfigScript.dificultad = 1;
                dificultadTxt.text = "Medio";
                break;
            case "2":
                ConfigScript.dificultad = 2;
                dificultadTxt.text = "Difícil";
                break;
        }
    }
    public void changeTime(float f)
    {
        switch (f + "")
        {
            case "0":
                ConfigScript.tiempo = 60;
                tiempoTxt.text = ConfigScript.tiempo + " segundos";
                break;
            case "1":
                ConfigScript.tiempo = 90;
                tiempoTxt.text = ConfigScript.tiempo + " segundos";
                break;
            case "2":
                ConfigScript.tiempo = 120;
                tiempoTxt.text = ConfigScript.tiempo + " segundos";
                break;
        }
    }
    public void changeTarget(int x)
    {
        target.x = x;
    }

}
