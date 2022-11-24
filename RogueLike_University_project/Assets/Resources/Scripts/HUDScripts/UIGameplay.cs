using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameplay : MonoBehaviour
{
    /*SCRIPT CHE PERMETTE LA GESTIONE DI TUTTO L'HUD IN PARTITA*/
    Character player;Floor floor;RoomTemplateScript roomtemplate;CameraFollowPlayer Maincamera;
    public SoundManager sound;

    [Header("VARIABILI UI PLAYER")]
    public Image[] heartImage; public Image UIheart_child,activable_image;
    public Button escbutton;
    public Sprite life,halflife,container;
    public GameObject UIheart,LoadScreen,HPBossBar,UIShoop,UIStatList,UIMiniMap,
                      ActivableLoadBar,ScoreBorder,Activablesign,PauseScreen,BossScreen,DeathScreen;
    public GameObject[] enemytopause;
    public Text bosskey_text,key_text,bombs_text,
                coint_text,floor_id_text,
                strenght_text,speed_text,
                dps_text,range_text,
                score_text,timer_text,
                timersecond,yourscore;

    public int countheart;
    public bool timeractive,escaneble,activebossscreen,skippclicked;

    [HideInInspector]
    public float secondsCount,FixedSecondCounter;
    public int minuteCount,hourCount,TimerCounterSecond;
    bool tabenable;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
        floor = GameObject.FindWithTag("Rooms").GetComponent<Floor>();
        roomtemplate = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();
        Maincamera = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollowPlayer>();

        heartImage = new Image[player.container];
        timeractive = true;
        EnableActivableLoadBar();
    }

    void Update()
    {
        SetHeart();
        SetPlayerUI();
        SetIconActivable();
        SetSignActivable();
        if(timeractive) StartTimer();
        if(roomtemplate.generatefloorcomplete && !BossScreen.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                tabenable = !tabenable;
                StatListEnable(tabenable);
                MiniMapEnable(tabenable);
            }

            if(Input.GetKeyDown(KeyCode.Escape) && !UIShoop.activeInHierarchy) PauseScreenEnable(true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && DeathScreen.activeInHierarchy) OnClosebutton();

        if(GameManager.instance!= null && TimerCounterSecond>0)ScoreBorder.transform.GetChild(3).gameObject.SetActive(false);
        else if(GameManager.instance!= null && TimerCounterSecond<=0)ScoreBorder.transform.GetChild(3).gameObject.SetActive(true);
        else if(GameManager.instance == null)ScoreBorder.transform.GetChild(3).gameObject.SetActive(true);

        BossScreen.SetActive(activebossscreen);
    }

    public void SetEscButton(RoomInfo inforoom)
    {
        if(!inforoom.isclose)escbutton.interactable = true;
        else escbutton.interactable = false;
    }

    public void LoadScreenEnable (bool enable)
    {
        AudioSource soundsource = GameObject.FindWithTag("Sounds").GetComponent<AudioSource>();
        soundsource.Stop();
        LoadScreen.SetActive(enable);
        if(!enable)sound.ChangeAudioSource("Audio/OST/Floor/floor"+floor.floorid+"ost");
    }
    public void DeathScreenEnable (bool enable){PlayerPrefs.DeleteAll();DeathScreen.SetActive(enable);}
    public void StatListEnable   (bool enable){UIStatList.SetActive(enable);}
    public void MiniMapEnable    (bool enable){UIMiniMap.SetActive(enable);}
    public void ShopEnable       (bool enable){UIShoop   .SetActive(enable);}
    
    public IEnumerator BossScreenEnable(GameObject currentroom) 
    {  
        activebossscreen = true;

        float tmpspeed = player.GetComponent<Character>().speed;
        float tmpshotspeed = player.GetComponent<Character>().shootspeed;

        BossScreen.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(SetImageBossScreen(floor.floorid));
        BossScreen.transform.GetChild(2).gameObject.GetComponent<Text>().text = SetTextBossScreen(floor.floorid);

        player.GetComponent<Character>().speed = 0;
        player.GetComponent<Character>().shootspeed = 0;
        player.GetComponent<CharacterShoot>().enabled = false;

        yield return new WaitForSeconds(1f);

        player.GetComponent<Character>().speed = tmpspeed;
        player.GetComponent<Character>().shootspeed = tmpshotspeed;
        player.GetComponent<CharacterShoot>().enabled = true;
        activebossscreen = false;
        currentroom.gameObject.GetComponent<EnemySpawn>().enabled = true;
    }

    string SetImageBossScreen(int floorid)
    {
        string path = "";
        if(floorid == 0) path = "Sprites/EnemySprites/BossSprite/Fire_Boss_Stationary_Animation";
        if(floorid == 1) path = "Sprites/EnemySprites/BossSprite/Aqua_Boss_Stationary_Animation";
        if(floorid == 2) path = "Sprites/EnemySprites/BossSprite/Green_Boss_Stationary_Animation";
        if(floorid == 3) path = "Sprites/EnemySprites/BossSprite/Orange_Boss_Stationary_Animation";
        if(floorid == 4) path = "Sprites/EnemySprites/BossSprite/White_Boss_Attack_Animation";
        if(floorid == 5) path = "Sprites/EnemySprites/BossSprite/Violet_Boss_Attack_Animation";
        return path;
    }

    string SetTextBossScreen(int floorid)
    {
        string vstext = "";
        if(floorid == 0) vstext = "Manush Vs TRIVELLATOR";
        if(floorid == 1) vstext = "Manush Vs LASORGRE";
        if(floorid == 2) vstext = "Manush Vs SNAIKO";
        if(floorid == 3) vstext = "Manush Vs GLADIATOR";
        if(floorid == 4) vstext = "Manush Vs MONARCH";
        if(floorid == 5) vstext = "Manush Vs BUGGYCLOWN";
        return vstext;
    }

    public void PauseScreenEnable(bool enable)
    {
        PauseScreen.SetActive(enable);
        GetPause(PauseScreen.activeInHierarchy);
    }

    public void SetEnemyToPauseArray(){enemytopause = GameObject.FindGameObjectsWithTag("Enemy");}

    public void GetPause(bool enable)
    {
        GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        GameObject.FindWithTag("Player").GetComponent<Character>().enabled = !enable;
        
        foreach(GameObject singleenemy in enemytopause) if(singleenemy != null) singleenemy.SetActive(!enable);

        GameObject[] bullet = GameObject.FindGameObjectsWithTag("Bullet");
        foreach(GameObject singlebullet in bullet) Destroy(singlebullet);
    }

    public void OnResumeButton(){PauseScreenEnable(false);}
    public void OnExitSaveGameButton()
    {   
        
        GameObject[] room = GameObject.FindGameObjectsWithTag("SingleRoom");
        foreach(GameObject singleroom in room) 
        {
            if(singleroom.GetComponent<RoomInfo>().isbonusroom)
               singleroom.GetComponent<RoomInfo>().bosskeyspawned = 
               singleroom.GetComponent<RoomInfo>().bosskeytaked;

            if(singleroom.GetComponent<RoomInfo>().Isshooproom)
               singleroom.GetComponent<RoomInfo>().shopspawned = false;

            if(singleroom.GetComponent<RoomInfo>().isbossroom)
               singleroom.GetComponent<RoomInfo>().stairspawned = 
               singleroom.GetComponent<RoomInfo>().defeatedboss;
        }
        player.SavePlayer();
        floor.SaveFloor();
        Maincamera.SaveMainCamera();
        SaveTimer();
        OnClosebutton();
    }
    public void OnClosebutton(){SceneManager.LoadScene(1);}

    public void ScoreBorderEnable(bool enable)
    {
        ScoreBorder.SetActive(enable);
        StartCoroutine(DecrementScore(TimerCounterSecond));
    }

    IEnumerator DecrementScore(int value)
    {
            for(int i=0;i<value;i++)
            {
                if(GameManager.score_manager.actualscore<=0)GameManager.score_manager.actualscore=0;
                else 
                {
                    if(skippclicked)break;
                    ScoreBorder.gameObject.GetComponent<AudioSource>().Play();
                    GameManager.score_manager.actualscore-=1;
                    yield return new WaitForSeconds(0.00001f);
                }
                TimerCounterSecond-=1;
               yield return new WaitForSeconds(0.05f);
            }
        ScoreBorder.gameObject.GetComponent<AudioSource>().Stop();
        GameManager.score_manager.ScoreArrayInsert();
    }

    public void OnSkipButton()
    {
        skippclicked = true;
        GameManager.score_manager.actualscore -= TimerCounterSecond;
        if(GameManager.score_manager.actualscore<=0)GameManager.score_manager.actualscore=0;
        ScoreBorder.gameObject.GetComponent<AudioSource>().Stop();
        GameManager.score_manager.ScoreArrayInsert();
    }

//FUNZIONE CHE SETTA LA UI DEL PLAYER
    void SetPlayerUI()
    {
        bosskey_text.text  = "x"      + player.bosskey;
        bombs_text.text    = "x"      + player.bombs;
        key_text.text      = "x"      + player.key;
        coint_text.text    = "x"      + player.coin;

        strenght_text.text = ""       + player.strenght;
        speed_text.text    = ""       + player.speed;
        dps_text.text      = ""       + player.dps;
        range_text.text    = ""       + player.shootspeed;
        floor_id_text.text = "FLOOR " + floor.floorid;
        timersecond.text = "TIMER SECOND "+TimerCounterSecond;
        if(GameManager.instance!= null) 
        {
            score_text.text = "SCORE "+GameManager.score_manager.actualscore;
            yourscore.text = "YOUR SCORE "+GameManager.score_manager.actualscore;
        }
    }
//-----------------------------------------------
//FUNZIONI CHE SETTA LA UI LOAD BAR DEI BOSS
    public void EnableHPBossBar(int MinVaL,int MaxVal,bool enable)
    {
        HPBossBar.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/EnemySprites/BossTesta/Head"+floor.floorid);
        HPBossBar.SetActive(enable);
        HPBossBar.GetComponent<Slider>().minValue = MinVaL;
        HPBossBar.GetComponent<Slider>().value = HPBossBar.GetComponent<Slider>().maxValue = MaxVal;
    }

     public void DamageHPBossBar(int damage){HPBossBar.GetComponent<Slider>().value -= damage;}
//------------------------------------------------

//FUNZIONE CHE SETTA IL TEMPO
    public void StartTimer()
    {
        secondsCount += Time.deltaTime;
        FixedSecondCounter += Time.deltaTime;
        TimerCounterSecond = (int)FixedSecondCounter;
        timer_text.text = "TIMER "+hourCount.ToString("00") +":"+ minuteCount.ToString("00") +":"+((int)secondsCount).ToString("00");
        if(secondsCount >= 60){
            minuteCount++;
            secondsCount = 0;
            }else if(minuteCount >= 60){
            hourCount++;
            minuteCount = 0;
        }
    }

    void SaveTimer()
    {
        GameManager.TimeDataSave(secondsCount,minuteCount,hourCount);
    }
    
//------------------------------------------------
 
//FUNZIONI CHE SETTA LA UI  DELLE ATTIVABILI
    void EnableActivableLoadBar(){ActivableLoadBar.GetComponent<Slider>().minValue = 0;ActivableLoadBar.GetComponent<Slider>().maxValue = 4;}
    void SetIconActivable()
    {
        if(player.activable_laser == false && player.activable_sigaro == false && player.activable_mirino == false) 
        {
            activable_image.gameObject.SetActive(false);
        }else{
            if(player.activable_laser) activable_image.sprite = Resources.Load<Sprite>("Sprites/ShopSprites/Laser");
            else if(player.activable_sigaro) activable_image.sprite = Resources.Load<Sprite>("Sprites/ShopSprites/Smoke");
            else if(player.activable_mirino) activable_image.sprite = Resources.Load<Sprite>("Sprites/ShopSprites/Mirino");

            activable_image.gameObject.SetActive(true);
            
            if(ActivableLoadBar.GetComponent<Slider>().value < ActivableLoadBar.GetComponent<Slider>().maxValue)
                activable_image.color = new Color32(130,130,130,255);
            else activable_image.color = new Color32(255,255,255,255);
        }
    }

    void SetSignActivable(){if(player.use_laser || player.use_sigaro || player.use_mirino ||  GetActivableValue())Activablesign.SetActive(true); else Activablesign.SetActive(false);}
    public bool GetActivableValue(){return ActivableLoadBar.GetComponent<Slider>().value >= ActivableLoadBar.GetComponent<Slider>().maxValue;}
    public void IncrementActivableLoadBar(){if(player.activable_laser || player.activable_sigaro || player.activable_mirino) ActivableLoadBar.GetComponent<Slider>().value += 1;}
    public void ResetActivableLoadBar(){if(ActivableLoadBar.GetComponent<Slider>().value >= ActivableLoadBar.GetComponent<Slider>().maxValue){ActivableLoadBar.GetComponent<Slider>().value = 0;Activablesign.SetActive(false);}}


//------------------------------------------------
   
//FUNZIONE CHE SETTA IL MECCANISMO DEI CUORI
    public void SetHeart()
    {
        if(player.container != heartImage.Length)
          {
              GameObject[] deleteheart = GameObject.FindGameObjectsWithTag("Heart");
              foreach(GameObject singleheart in deleteheart) Destroy(singleheart); 
              heartImage = new Image[player.container];
              countheart = 0;
          } 

        for(int i=0;i<player.container;i++)
        {
            if(countheart < player.container)
            {
                if(countheart < 6)
                 heartImage[countheart] =  Instantiate(UIheart_child,new Vector2(UIheart.transform.position.x+i*30,
                 UIheart.transform.position.y),Quaternion.identity,UIheart.transform);
                else
                 heartImage[countheart] =  Instantiate(UIheart_child,new Vector2(UIheart.transform.position.x+(i-6)*30,
                 UIheart.transform.position.y-30),Quaternion.identity,UIheart.transform);
                
                countheart++;
            }else if(countheart >= player.container)
            {
                if(i<=(int)player.life) 
                {
                    if(i==(int)player.life)
                    {
                        if(player.life % 1 !=0)heartImage[i].sprite = halflife;
                        else heartImage[i].sprite = container;
                    }else heartImage[i].sprite = life;
                    
                }else heartImage[i].sprite = container;
                

                if(i < player.container)heartImage[i].enabled=true;
                else heartImage[i].enabled=false;
            }
        }    
    }
//-------------------------------------------------------------------   
}
