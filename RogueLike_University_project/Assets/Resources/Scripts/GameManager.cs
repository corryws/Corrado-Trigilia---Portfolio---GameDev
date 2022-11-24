using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Score score_manager;
    public static int gamemode;

void Awake(){
       if(instance == null){
           instance = this;
           DontDestroyOnLoad(transform.root.gameObject);
           score_manager = GameObject.Find("ScoreManager").GetComponent<Score>();
           Destroy(GameObject.Find("ScoreManager"));
       }else{
           Destroy(transform.root.gameObject);
           return;
       }
    }

//funzione salva player
 public static void PlayerDataSave(GameObject player)
    {
        PlayerPrefs.SetInt("IsSaved",1);

        Character character= player.GetComponent<Character>();
        string infoplayer  = JsonUtility.ToJson(player.GetComponent<Character>()); 
        PlayerPrefs.SetString("InfoPlayer",infoplayer);
        PlayerPrefs.SetFloat ("PlayerX"   ,player.transform.position.x);
        PlayerPrefs.SetFloat ("PlayerY"   ,player.transform.position.y);
    }

//funzione salva floor
 public static void FloorDataSave(GameObject floorobj)
    {
        Floor floor = floorobj.GetComponent<Floor>();
        string infofloor = JsonUtility.ToJson(floorobj.GetComponent<Floor>());
        PlayerPrefs.SetString("InfoFloor",infofloor);

        for(int i=0;i<floorobj.GetComponent<RoomTemplateScript>().rooms.Count;i++)
        {
            string inforoom_string = JsonUtility.ToJson(floorobj.GetComponent<RoomTemplateScript>().rooms[i].GetComponent<RoomInfo>());
            string enemyspawn      = JsonUtility.ToJson(floorobj.GetComponent<RoomTemplateScript>().rooms[i].GetComponent<EnemySpawn>());
            for(int j=0;j<4;j++)
            {
                string infodoor = JsonUtility.ToJson(
                floorobj.GetComponent<RoomTemplateScript>().rooms[i].transform.GetChild(2).
                gameObject.transform.GetChild(j).GetComponent<DoorTrigger>());
                
                PlayerPrefs.SetString("RoomDoor"+i+"Info"+j, infodoor); 
            }
            PlayerPrefs.SetString("RoomInfo"  +i, inforoom_string); 
            PlayerPrefs.SetString("enemyspawn"+i, enemyspawn);
        }
    }

//funzione salva score
 public static void ScoreDataSave(int actualscore,int[] scoresaved)
 {
     PlayerPrefs.SetInt("ActualScore",actualscore);
     if(scoresaved.Length != 0)
     {
        for(int i=0;i<scoresaved.Length;i++)
        {
            PlayerPrefs.SetInt("ScoreSaved"+i,scoresaved[i]);
        }
     }   
 } 
//funzione salva timer
 public static void TimeDataSave(float secondsCount,int minuteCount,int hourCount)
    {
        PlayerPrefs.SetFloat("secondsCount",secondsCount);
        PlayerPrefs.SetInt  ("minuteCount" ,minuteCount);
        PlayerPrefs.SetInt  ("hourCount"   ,hourCount);
    }
//funzione salva Camera
 public static void MainCameraDataSave(GameObject camera)
 {
    CameraFollowPlayer camerafollow = camera.GetComponent<CameraFollowPlayer>();
    string infocamerafollow = JsonUtility.ToJson(camera.GetComponent<CameraFollowPlayer>()); 
    PlayerPrefs.SetString("InfoCamera",infocamerafollow);
 }


//funzione carica hud timer
    public static void LoadHudTimer(GameObject UiSetter)
    {
        UIGameplay hudview = UiSetter.GetComponent<UIGameplay>();
        hudview.secondsCount = PlayerPrefs.GetFloat("secondsCount");
        hudview.minuteCount = PlayerPrefs.GetInt("minuteCount");
        hudview.hourCount = PlayerPrefs.GetInt("hourCount");
    }

//funzione carica FloorInfo
    public static GameObject LoadFloorInfo(GameObject floor)
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("InfoFloor"),floor.GetComponent<Floor>());
        return floor;
    }

//funzione carica PlayerInfo
    public static GameObject LoadPlayerInfo(GameObject player)
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("InfoPlayer"),player.GetComponent<Character>());
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("PlayerX"),PlayerPrefs.GetFloat("PlayerY"));
        return player;
    }

//funzione carica CameraInfo
    public static GameObject LoadCameraInfo(GameObject camera)
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("InfoCamera"),camera.GetComponent<CameraFollowPlayer>());
        return camera;
    }

//funzione carica ScoreArray
    public static int[] LoadScore(int[] scoresaved)
    {
        for(int i=0;i<scoresaved.Length;i++)
        {
            scoresaved[i] = PlayerPrefs.GetInt("ScoreSaved"+i);
        }
        return scoresaved;
    }

//funzione carica RoomInfo
    public static List<GameObject> LoadRoomScript(List<GameObject> rooms)
    {
        for(int i=0;i<rooms.Count;i++)
        {
            for(int j=0;j<4;j++)
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("RoomDoor"+i+"Info"+j),rooms[i].transform.GetChild(2).
                gameObject.transform.GetChild(j).GetComponent<DoorTrigger>()); 
            }
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("RoomInfo"+i),rooms[i].GetComponent<RoomInfo>());
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("enemyspawn"+i),rooms[i].GetComponent<EnemySpawn>());
        }
        return rooms;
    }


     void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("ScreenWidth",Screen.width);
        PlayerPrefs.SetInt("ScreenHeight",Screen.height);
        PlayerPrefs.SetInt("ScreenQuality",QualitySettings.GetQualityLevel());
        Debug.Log(Screen.fullScreen);
    }
}
