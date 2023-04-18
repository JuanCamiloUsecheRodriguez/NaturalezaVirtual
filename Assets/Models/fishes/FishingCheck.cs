using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FishingCheck : MonoBehaviour {

    public PressureScript ps;
    public FishGenerator fg;
    public GameObject IngresaTxt, SacaTxt, BuscaTxt, Plus5Txt, Minus5Txt, ScorePanel, FallasteTxt;
    public Text TimeTxt, ScoreTxt, ScorePanelText;
    public GameObject fish, net, tutorial;
    public int time = ConfigScript.tiempo;
    public int score = 0;

    public bool isTimeRunning = false;
    public Quaternion netDesiredRotation;

    public List<Fish2> fishes;
    // Use this for initialization
    void Start () {
        fishes = new List<Fish2>();
        
        netDesiredRotation = Quaternion.Euler(Vector3.left * 45);
        time = ConfigScript.tiempo;
    }
	public IEnumerator timer()
    {
        yield return new WaitForSeconds(1f);
        time--;
        if(time == 0)
        {
            StopAllCoroutines();
            ScorePanel.SetActive(true);
            time = 0;
            TimeTxt.text = time + "";
            ScorePanelText.text = score + "";
            BuscaTxt.SetActive(false);
            FallasteTxt.SetActive(false);
            IngresaTxt.SetActive(false);
            SacaTxt.SetActive(false);
            Plus5Txt.SetActive(false);
            Minus5Txt.SetActive(false);
        } else
        {
            if (ps.inWater)
            {
                StartCoroutine(timer());
            }
        }
    }
    // Update is called once per frame

    float d = 0, d2 = 0;
    int fishIndex = -1;
    /*void Update()
    {
        if(time > 0)
        {
            if (ps.changed)
            {
                if (ps.inWater)
                {

                } else
                {

                }
            }
        }

    }*/
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ps.changed = true;
            ps.inWater = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ps.changed = true;
            ps.inWater = false;
        }
        net.transform.rotation = Quaternion.Lerp(net.transform.rotation, netDesiredRotation, Time.deltaTime * 2.5f);
        if (time > 0)
        {
            
            if (ps.inWater)
            {
                TimeTxt.text = time + "";
            } else
            {
                TimeTxt.text = "| |";
                //netDesiredRotation = Quaternion.Euler(Vector3.left * 45);
            }
            
            ScoreTxt.text = score + "";
            if (ps.changed)
            {
                ps.changed = false;
                if (ps.inWater) // goes into water
                {
                    if (tutorial.activeSelf)
                    {
                        tutorial.GetComponent<TutoIngresa>().StopAllCoroutines();
                        tutorial.SetActive(false);
                    }
                    
                    //net.GetComponent<NetColliderScript>().showNormal();
                    netDesiredRotation = Quaternion.identity;
                    Plus5Txt.SetActive(false);
                    FallasteTxt.SetActive(false);
                    Minus5Txt.SetActive(false);
                    IngresaTxt.SetActive(false);
                    SacaTxt.SetActive(false);
                    BuscaTxt.SetActive(true);

                    fg.container.GetComponent<Control>().move = true;
                    if (!isTimeRunning)
                    {
                        StartCoroutine(timer());
                        isTimeRunning = true;
                    }

                    fg.slowStartFunction();
                    
                    if (fish != null)
                    {
                        fish.GetComponent<Fish2>().changeBehavior(fish.GetComponent<Fish2>().normalBehavior);
                        fish.GetComponent<Fish2>().speedUpFunction();
                        fishes.Remove(fish.GetComponent<Fish2>());
                        fish = null;
                    }
                }
                else // goes out of water
                {
                    fg.container.GetComponent<Control>().move = false;
                    
                    if (isTimeRunning)
                    {
                        StopCoroutine(timer());
                        isTimeRunning = false;
                    
                        BuscaTxt.SetActive(false);

                        TimeTxt.text = "| |";

                        fishIndex = -1;
                        for(int i = 0; i<fishes.Count; i++)
                        {
                            if(i == 0)
                            {
                                d = fishes[i].transform.position.x * fishes[i].transform.position.x + fishes[i].transform.position.z * fishes[i].transform.position.z;
                                fishIndex = i;
                            } else
                            {
                                d2 = fishes[i].transform.position.x * fishes[i].transform.position.x + fishes[i].transform.position.z * fishes[i].transform.position.z;
                                if (d2 < d)
                                {
                                    d = d2;
                                    fishIndex = i;
                                }
                            }
                        }

                        if(fishes.Count > 0)
                        {
                            fish = fishes[fishIndex].gameObject;
                        } else
                        {
                            fish = null;
                        }
                        if (fish != null)
                        {
                            if(fish.GetComponent<Fish2>().inNet) {//caught
                                fish.transform.position = Vector3.right * 0 + Vector3.forward * 0 + Vector3.up * 12;
                                
                                FallasteTxt.SetActive(false);
                                IngresaTxt.SetActive(false);
                                BuscaTxt.SetActive(false);
                                SacaTxt.SetActive(false);

                                fish.GetComponent<Fish2>().changeBehavior(Behaviors.JUMP);
                                score += fish.GetComponent<Fish2>().score;
                                if (fish.GetComponent<Fish2>().score < 0)
                                {
                                    //net.GetComponent<NetColliderScript>().showBroken();
                                    Minus5Txt.SetActive(true);
                                } else
                                {
                                    Plus5Txt.SetActive(true);
                                }
                                StartCoroutine(showIngresa());
                                netDesiredRotation = Quaternion.identity;
                            }
                            else // miss, probably will never work
                            {
                                fish = null;
                                FallasteTxt.SetActive(true);
                                IngresaTxt.SetActive(false);
                                BuscaTxt.SetActive(false);
                                SacaTxt.SetActive(false);
                                Plus5Txt.SetActive(false);
                                Minus5Txt.SetActive(false);
                                StartCoroutine(showIngresa());
                                netDesiredRotation = Quaternion.Euler(Vector3.left * 45);
                            }
                        } else // miss
                        {
                            fish = null;
                            FallasteTxt.SetActive(true);
                            IngresaTxt.SetActive(false);
                            BuscaTxt.SetActive(false);
                            SacaTxt.SetActive(false);
                            Plus5Txt.SetActive(false);
                            Minus5Txt.SetActive(false);
                            StartCoroutine(showIngresa());
                            netDesiredRotation = Quaternion.Euler(Vector3.left * 45);
                        }
                    }

                }
                //do gui stuff
            }
        }
        
	}
    public IEnumerator showIngresa()
    {
        yield return new WaitForSeconds(2f);
        IngresaTxt.SetActive(true);
        BuscaTxt.SetActive(false);
        SacaTxt.SetActive(false);
        FallasteTxt.SetActive(false);
        Plus5Txt.SetActive(false);
        Minus5Txt.SetActive(false);
    }
    public void goBackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
    public void notify(GameObject fish)
    {
        //this.fish = null;
        if(isTimeRunning && time > 0)
        {
            BuscaTxt.SetActive(false);
            FallasteTxt.SetActive(false);
            IngresaTxt.SetActive(false);
            SacaTxt.SetActive(true);
            Plus5Txt.SetActive(false);
            Minus5Txt.SetActive(false);
        //this.fish = fish;
        } 
    }
}
