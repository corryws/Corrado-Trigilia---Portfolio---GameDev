using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public GameObject Player,MainCamera,RoomTemplate,UiSetter;

    void Awake()
    {
        if(PlayerPrefs.HasKey("IsSaved"))LoadGameSet();
        else NewGameSet();
    }

    void NewGameSet()
    {
        MainCamera.SetActive(true);
        RoomTemplate.SetActive(true);
        UiSetter.SetActive(true);
        Player.SetActive(true);

        if(GameManager.gamemode == 1)//HARD MODE
        {
            Player.GetComponent<Character>().life = Player.GetComponent<Character>().container = 2;
            Player.GetComponent<Character>().bombs = 1;
        }
    }

    void LoadGameSet()
    {
        MainCamera.SetActive(true);
        RoomTemplate.SetActive(true);
        UiSetter.SetActive(true);
        Player.SetActive(true);

        MainCamera  = GameManager.LoadCameraInfo(MainCamera);
        RoomTemplate = GameManager.LoadFloorInfo(RoomTemplate);
        Player = GameManager.LoadPlayerInfo(Player);
        GameManager.LoadHudTimer(UiSetter);
    }
}
