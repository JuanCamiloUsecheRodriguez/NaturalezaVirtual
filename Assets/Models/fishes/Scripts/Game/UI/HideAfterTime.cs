using UnityEngine;
using System.Collections;

public class HideAfterTime : MonoBehaviour {

    // Use this for initialization
    public int delay = 3;
	void Start () {
	    
	}
	void OnEnable()
    {
        StartCoroutine(HideAfterSeconds());
    }
    IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
