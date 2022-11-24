using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingResolution : MonoBehaviour
{
    Resolution[] resolution;
    public Text resolutiontxt;

    // Start is called before the first frame update
    void Awake()
    {
        if(PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight") && PlayerPrefs.HasKey("ScreenQuality"))
        {
             Screen.SetResolution(PlayerPrefs.GetInt("ScreenWidth"),PlayerPrefs.GetInt("ScreenHeight") ,true);
             QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("ScreenQuality"));
        }else 
        {
            Screen.SetResolution(1280,1024,true);
            QualitySettings.SetQualityLevel(2);
        }
        string fullscreensign = "";
        if(Screen.fullScreen == true)fullscreensign = "FULLSCREENMODE = ON";else fullscreensign = "FULLSCREENMODE = OFF";
        resolutiontxt.text = Screen.width + " X " + Screen.height + "  " + fullscreensign + " " + Screen.fullScreen + " Quality.: " + QualitySettings.GetQualityLevel();
        
        StartCoroutine(nextscene());
    }

    IEnumerator nextscene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
