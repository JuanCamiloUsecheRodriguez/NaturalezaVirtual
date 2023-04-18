using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioWaveForm : MonoBehaviour {

    // Use this for initialization
    public GameObject mic;
    public static Texture2D audioWaveform(AudioClip aud, int width, int height, Color color)
    {

        int step = Mathf.CeilToInt((aud.samples * aud.channels) / width);
        float[] samples = new float[aud.samples * aud.channels];

        //workaround to prevent the error in the function getData
        //when Audio Importer loadType is "compressed in memory"
        /*string path = AssetDatabase.GetAssetPath(aud);
        AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
        AudioImporterLoadType audioLoadTypeBackup = audioImporter.loadType;
        audioImporter.loadType = AudioImporterLoadType.StreamFromDisc;
        AssetDatabase.ImportAsset(path);*/

        //getData after the loadType changed
        aud.GetData(samples, 0);

        //restore the loadType (end of workaround)
        //audioImporter.loadType = audioLoadTypeBackup;
       // AssetDatabase.ImportAsset(path);

        Texture2D img = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color[] xy = new Color[width * height];
        for (int x = 0; x < width * height; x++)
        {
            xy[x] = new Color(0, 0, 0, 0);
        }

        img.SetPixels(xy);

        int i = 0;
        while (i < width)
        {
            int barHeight = Mathf.CeilToInt(Mathf.Clamp(Mathf.Abs(samples[i * step]) * height, 0, height));
            int add = samples[i * step] > 0 ? 1 : -1;
            for (int j = 0; j < barHeight; j++)
            {
                img.SetPixel(i, Mathf.FloorToInt(height / 2) - (Mathf.FloorToInt(barHeight / 2) * add) + (j * add), color);
            }
            ++i;

        }

        img.Apply();
        return img;
    }
    public Image image;
    void Start () {
       image = GetComponent<Image>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if(mic.GetComponent<AudioSource>().clip != null)
        image.sprite = Sprite.Create(AudioWaveForm.audioWaveform(mic.GetComponent<AudioSource>().clip, 400, 400, Color.black), new Rect(0, 0, 400, 400), new Vector2(.5f, .5f));
    }
}
