using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutoIngresa : MonoBehaviour {

    public Image image1, image2;
	// Use this for initialization
	void Start () {
        StartCoroutine("animate");
	}
	
    IEnumerator animate()
    {
        yield return new WaitForSeconds(.75f);
        image1.gameObject.SetActive(!image1.IsActive());
        image2.gameObject.SetActive(!image2.IsActive());
        StartCoroutine("animate");
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
