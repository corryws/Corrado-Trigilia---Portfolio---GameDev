using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuButtonSetting : MonoBehaviour
{
    public Animator titlescreenanim;
    public GameObject blackscreen,MainButton,titleimage,optionscreen,scorescreen,creditscreen,diffcoultyscreen,background_theme_01;
    public GameObject[] SceneButton;
    public AudioMixer mainMixer,effectMixer;

    public Dropdown resolutionDropdown,difficoultyDropdown;
    public Text score_text,description_text;
    Resolution[] resolutions;
    int iteraction;

    void Awake()
    {
        SetDropDownResolution();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && MainButton.activeInHierarchy)SwitchOption();
        else if(Input.GetKeyDown("right") || Input.GetKeyDown("d") && MainButton.activeInHierarchy) OnClickArrowRight();
        else if(Input.GetKeyDown("left") || Input.GetKeyDown("a") && MainButton.activeInHierarchy) OnClickArrowLeft();
        if(SceneButton[0].name == "ContinueButton")SceneButton[0].GetComponent<Button>().interactable = PlayerPrefs.HasKey("IsSaved");
    }

    void SwitchOption()
    {
        switch(SceneButton[0].name)
        {
            case "NewGameButton":
                OnNewGame();
            break;
            case "ContinueButton"://da fare
                if(SceneButton[0].GetComponent<Button>().interactable) OnContinue();
            break;
            case "OptionButton":
                OnOption();
            break;
            case "ScoreButton":
                OnScore();
            break;
            case "CreditButton":
                OnCredit();
            break;
            case "ExitButton":
                OnExit();
            break;
        }
    }

public void OnNewGame(){diffcoultyscreen.SetActive(true);MainButton.SetActive(false);OnValueChangeChoiceMode();}
public void OnContinue(){SetGameObject();titlescreenanim.SetBool("play",true);}
public void OnOption(){optionscreen.SetActive(true);MainButton.SetActive(false);}
public void OnExit(){Application.Quit();}
public void OnCredit(){creditscreen.SetActive(true); MainButton.SetActive(false);}
public void OnScore()
    {
        if(GameManager.score_manager.scoresaved[0] != 0)
         GameManager.ScoreDataSave(GameManager.score_manager.actualscore,GameManager.score_manager.scoresaved);
        scorescreen.SetActive(true);
        MainButton.SetActive(false);
        if(GameManager.instance!=null)
        {
            GameManager.score_manager.scoresaved = GameManager.LoadScore(GameManager.score_manager.scoresaved);
            GameManager.score_manager.ArrayOrder();
            SetScoreText(GameManager.score_manager.scoresaved);
        }
        
    }

public void OnCloseButtonOption(){scorescreen.SetActive(false); optionscreen.SetActive(false); 
                                      creditscreen.SetActive(false); diffcoultyscreen.SetActive(false);
                                      MainButton.SetActive(true);}
    
public void OnStartButton(){diffcoultyscreen.SetActive(false);GameManager.score_manager.actualscore=0;SetGameObject();PlayerPrefs.DeleteAll();titlescreenanim.SetBool("play",true);}
public void OnValueChangeChoiceMode()
 {
    GameManager.gamemode = difficoultyDropdown.value;
    if(difficoultyDropdown.value == 0)description_text.text = "*Una semplice run a 3 vite col giusto bilanciamento";
    else description_text.text = "*Una run più complessa a 2 vite con un doppio score ma anche doppio del danno!";
 }

public void SetQuality(int qualityIndex){QualitySettings.SetQualityLevel(qualityIndex);}
//public void SetFullScreen(bool isFullScreen){Screen.fullScreen = isFullScreen;}
void SetDropDownResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options= new List<string>();
        int currentResolutionIndex = 0;
        int rwidth=0,rheight=0;
        for(int i=0;i<3;i++)
        {
            if(i==0){rwidth = 1024; rheight = 768;}
            else if(i==1){rwidth = 1280; rheight = 1024;}
            else if(i==2){rwidth = 1600; rheight = 900;}

            resolutions[i].width = rwidth; 
            resolutions[i].height = rheight;
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            currentResolutionIndex = 1;  
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }

public void SetMainVolume(float volume){mainMixer.SetFloat("volume",volume);}
public void SetEffectVolume(float volume){effectMixer.SetFloat("volume",volume);}

void SetScoreText(int[] allscore)
    {
        string positioncolor;
        for(int i=0;i<allscore.Length;i++) 
        {
            if(i == 0)score_text.text += " "+" "+GetPositionString(i,"#ffdf00",allscore[i]);
            else if(i == 1)score_text.text += " "+" "+GetPositionString(i,"#c4cace",allscore[i]);
            else if(i == 2)score_text.text += " "+" "+GetPositionString(i,"#996515",allscore[i]);
            else if(i == 9)score_text.text += GetPositionString(i,"#000000",allscore[i]);
            else score_text.text += " "+" "+GetPositionString(i,"#000000",allscore[i]);
        }
    }

    string GetPositionString(int i,string pos,int actualscore)
    {
        string startcolor = "<color="+pos+">",endcolor="</color>";
         return (startcolor + (i+1) + "# SCORE " + (actualscore) + endcolor+"\n");
    }

void SetGameObject()
    {
        blackscreen.SetActive(false);
        MainButton.SetActive(false);
        titleimage.SetActive(false);
        background_theme_01.SetActive(false);
        this.enabled = false;
    }

void ReverseArray()
 {
    int cmd = SceneButton.Length-1;
    for(int i=0;i<SceneButton.Length-2;i++)
    {
        GameObject substitutebutton = SceneButton[cmd];
        SceneButton[cmd] = SceneButton[i];
        SceneButton[i] = substitutebutton;
        cmd--;

        SceneButton[0].SetActive(true);
        SceneButton[i+1].SetActive(false);
    }
 }

void SetArrayActive()
 {
    for(int i=0;i<SceneButton.Length-1;i++)
    {
        SceneButton[0].SetActive(true);
        SceneButton[i+1].SetActive(false);
    }
 }

void SwitchMod()
 {
    for(int i=0;i<SceneButton.Length-1;i++)
        {     
            GameObject substitutebutton = SceneButton[i];
            SceneButton[i] = SceneButton[i+1];
            SceneButton[i+1] = substitutebutton;
            SetArrayActive();
        }
 }

public void OnClickArrowLeft(){
    if(!Input.GetKeyDown(KeyCode.Return))
    {
        if(iteraction!=0)
        {
            iteraction=0;
            ReverseArray();
            SetArrayActive();
        }else SwitchMod();
    }
 }

public void OnClickArrowRight(){
    if(!Input.GetKeyDown(KeyCode.Return))
    {
        if(iteraction!=1)
        {
            iteraction=1;
            ReverseArray();
            SetArrayActive();
        }else SwitchMod();
    }
    
 }

 }
