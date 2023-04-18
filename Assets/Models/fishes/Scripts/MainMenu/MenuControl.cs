using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour {

    // Use this for initialization
    public Button PlayButton, QuitButton;
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PlayButton.onClick.AddListener(() => changeScene(1));
        QuitButton.onClick.AddListener(() => exit());

       // Sensor.Activate(Sensor.Type.Pressure);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeScene( int id)
    {
        SceneManager.LoadSceneAsync(id);
    }
    public void exit()
    {
        Application.Quit();
    }
}
