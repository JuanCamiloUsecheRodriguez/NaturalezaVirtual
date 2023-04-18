using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressureScript : MonoBehaviour
{

    // Use this for initialization
    public Text txt, txt2;
    public float currentPressure;
    public float averagePressure;
    public float deltaPressure, previousPressure;
    //public Sensor.Information sensor;
    public float threshold = 3;

    public bool inWater = false;
    public bool changed = false;

    public float timer = 0;
    void Awake()
    {
        
    }
    void Start()
    {
       // Sensor.Activate(Sensor.Type.Pressure);
       // sensor = Sensor.Get((Sensor.Type) 6);
        
        
        StartCoroutine(beginCoroutine());
    }
    IEnumerator beginCoroutine()
    {
        yield return new WaitForSeconds(1f);
        //currentPressure = sensor.values.x;
        averagePressure = currentPressure;
        StartCoroutine(calcAveragePressure());
    }
    IEnumerator calcAveragePressure()
    {
        
       // deltaPressure = sensor.values.x - currentPressure;
       // currentPressure = sensor.values.x;

        if (deltaPressure >= 0.2f)
        {
            inWater = true;
            changed = true;
        }
        else if (deltaPressure <= -0.2f)
        {
            inWater = false;
            changed = true;
        }

        yield return new WaitForSeconds(.2f);

        
        StartCoroutine(calcAveragePressure());
        
    }

    // Update is called once per frame

    void Update()
    {
        /*if(timer >= 3)
        {
            if (sensor.values.x >= averagePressure + threshold)
            {
                txt.text = "p:" + sensor.values + " water";
            }
            else
            {
                txt.text = "p:" + sensor.values + " air";
            }
        }*/
        /*txt.text = "p:" + sensor.values.x+", d:"+deltaPressure;
        if (inWater)
        {
            txt2.text = "water";
        } else
        {
            txt2.text = "air";
        }*/

    }
}
