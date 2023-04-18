using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {
    public List<GameObject> images;
    int i = 0, j = 0;
    GameObject prev;

    int[] seq = { 6,8,6,8,6,8,6,8,0,1,2,1,0,1,2,1,3,4,5,4,3,4,5,4,3,7,9,7,9,7,9,7,9 };
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void playAnimation()
    {
        StartCoroutine("animate");
    }
    public void stopAnimation()
    {
        StopCoroutine("animate");
        prev.SetActive(false);
        i = 0; j = 0;
    }
    IEnumerator animate()
    {
        if (prev != null)
            prev.SetActive(false);
        j = i % 32;
        images[seq[j]].SetActive(true);
        prev = images[seq[j]];
        i++;
        yield return new WaitForSeconds(.5f);
        StartCoroutine("animate");
    }
}
