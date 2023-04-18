using UnityEngine;
using System.Collections;

public class ToonShaderScript : MonoBehaviour {

    // Use this for initialization
    Material mat;
	void Start () {
        mat = GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    float a = 0;
	void Update () {
        mat.SetFloat("_Outline", (.5f + Mathf.Sin(a)/2)/200);
        a += .25f;
    }
}
