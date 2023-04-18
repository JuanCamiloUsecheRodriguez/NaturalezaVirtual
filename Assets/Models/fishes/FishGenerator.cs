using UnityEngine;
using System.Collections;

public class FishGenerator : MonoBehaviour {

    // Use this for initialization
    public int maxFishes = 35;
    public GameObject[] prefabs;
    public GameObject container;
    public bool slowStart = false;

    private int fishes = 0;

	void Start () {
        /*if (slowStart)
        {
              StartCoroutine(spawnRandomFish(2f));
          } else {
              for (int i = 0; i < maxFishes; i++)
              {
                  generateFish(Random.Range(0, prefabs.Length), Random.Range(-20,20) * Vector3.right + Random.Range(-20, 20) * Vector3.forward + Vector3.up * 11f);
              }
            slowStartFunction();
        }   */
        
	}
    public void slowStartFunction()
    {
        StartCoroutine(spawnRandomFish(1f));
    }
    IEnumerator spawnRandomFish(float seconds)
    {
        if(fishes < maxFishes)
        {
            if (Random.Range(0,100) < 95 - 5 * ConfigScript.dificultad)
            {
                generateFish(Random.Range(0, prefabs.Length-3), Random.Range(-20, 20) * Vector3.right + Random.Range(-20, 20) * Vector3.forward + Vector3.up * 12f);
            } else
            {
                generateFish(Random.Range(prefabs.Length - 3, prefabs.Length), Random.Range(-20, 20) * Vector3.right + Random.Range(-20, 20) * Vector3.forward + Vector3.up * 12f);
            }
            
            yield return new WaitForSeconds(seconds);
            StartCoroutine(spawnRandomFish(seconds));
        }        
    }
	
    public void generateFish(int type, Vector3 position)
    {
        var fish = Instantiate(prefabs[type]);
        fish.transform.parent = container.transform;
        fish.transform.localPosition = position;
        fishes++;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
