using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    float a = 0;
    Vector3 speed = Vector3.zero; 
	void Update () {
        speed += Vector3.right * Mathf.Sin(a) * .001f;
        speed *= .97f;
        transform.position += speed;
        a+=.025f;
	}
}
