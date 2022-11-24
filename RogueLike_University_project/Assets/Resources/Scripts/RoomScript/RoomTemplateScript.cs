using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomTemplateScript : MonoBehaviour
{
    /*QUESTO SCRIPT E' QUELLO CHE GESTISCE TUTTE LE STANZE PRESENTI NEL GIOCO
    INOLTRE PERMETTE L'INIZIO DI ESSO SPAWNANDO UNA ROOM RANDOM TRA QUELLE PRESENTI
    E INDICA DOVE SI TROVA LA BOSSROOM(FINE STANZA)*/
    public CameraFollowPlayer Camerafollow;
    public UIGameplay hudview;
    public GameObject[] AllRoom,bottomRooms, topRooms, leftRooms, rightRooms;
    //inspector prefab variable
    public GameObject singleminimaproom,minimap,shopsystem;
    public List<GameObject> rooms,allMiniRooms,allSingleDoor;
    public bool generatefloorcomplete;

    public int counticon;

    void Awake(){Camerafollow = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollowPlayer>();}

//spawn entry function
 public void SpawnEntryRoom(int RandomRoom)
    {
        GameObject entryroom = Instantiate(AllRoom[RandomRoom],this.transform.position,Quaternion.identity);
        counticon=0; InstantiateMiniMapIcon(entryroom);
    }

//MiniMapFunction
 public void InstantiateMiniMapIcon(GameObject currentroom)
    {
        Vector3 pos = currentroom.transform.position;
        GameObject minimaproom = Instantiate(singleminimaproom,pos,Quaternion.identity);
        allMiniRooms.Add(minimaproom);
        currentroom.GetComponent<RoomInfo>().CurrentMiniMapIcon = allMiniRooms.Count-1;
        SetMiniMapIcon(pos);
    }

 public void SetMiniMapIcon(Vector3 pos)
    {
        //1f x corrisponde a 15 | 1f y corrisponde a 9
        allMiniRooms[counticon].transform.position = new Vector2(pos.x*2f,pos.y*3.5f);
        allMiniRooms[counticon].transform.SetParent(minimap.transform.GetChild(0).transform,false);
        allMiniRooms[counticon].GetComponent<Image>().color = Color.grey;
        counticon++;
    }

 public void SetXYMiniMapIcon(float xpos,float ypos)
 {
    foreach(GameObject singleminimap in allMiniRooms)
    singleminimap.transform.position = new Vector2(singleminimap.transform.position.x+(xpos),singleminimap.transform.position.y+(ypos));
 }
 public void InstantiateLoadingMiniMapIcon(GameObject minimaploaded){minimaploaded.transform.SetParent(minimap.transform.GetChild(0).transform,true);}
 public GameObject GetMiniMapContainer(){return minimap;}

//FUNZIONI DI ABILIAZIONE GAMEPLAY E NON
    public void EnableGamePlay(bool enable)
    {
        generatefloorcomplete = enable; EnablePlayer(enable);
        hudview = GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>(); hudview.LoadScreenEnable(!enable);

        if(generatefloorcomplete)
        {
            if(!PlayerPrefs.HasKey("IsSaved")) 
            {
                Camerafollow.minPos = Camerafollow.maxPos = new Vector2(0,0);
                 SettSingleDoorRoom();
            }else rooms = GameManager.LoadRoomScript(rooms);
            SettRoom();
        }
    }

    void SettSingleDoorRoom()
    {   
        int countfreesingledoor = 0;
        for(int i=0;i<allSingleDoor.Count;i++)
         if(!allSingleDoor[i].GetComponent<RoomInfo>().isbossroom && !allSingleDoor[i].GetComponent<RoomInfo>().Isshooproom) countfreesingledoor++;

        if(countfreesingledoor > 0)
        {
            for(int i=0;i<allSingleDoor.Count;i++)
            {
                if(!allSingleDoor[i].GetComponent<RoomInfo>().isbossroom && !allSingleDoor[i].GetComponent<RoomInfo>().Isshooproom)
                {
                    allSingleDoor[i].GetComponent<RoomInfo>().isbonusroom = true;break;
                }
            }
        }else{
            for(int i=0;i<rooms.Count;i++)
            {
                if(rooms[i].GetComponent<RoomInfo>().ID_ROOM != 1 && !rooms[i].GetComponent<RoomInfo>().isbossroom && !rooms[i].GetComponent<RoomInfo>().Isshooproom)
                {
                    rooms[i].GetComponent<RoomInfo>().isbonusroom = true;break;
                }
            }
        }
    }

    void SettRoom()
    {
        foreach(GameObject singleroom in rooms)
        { 
            if(singleroom.GetComponent<RoomInfo>().ID_ROOM == 1) singleroom.gameObject.GetComponent<RoomInfo>().isclose = false;
            if(singleroom.GetComponent<RoomInfo>().issingledoor) singleroom.GetComponent<RoomEvent>().CheckBossOrShoopBool();
            if(singleroom.GetComponent<RoomInfo>().issingledoor && singleroom.GetComponent<RoomInfo>().Isshooproom && singleroom.GetComponent<RoomInfo>().FollowedByRoomID == 1)
            {
                int countdoor = rooms[singleroom.GetComponent<RoomInfo>().FollowedByRoomID].transform.GetChild(2).transform.childCount;
                for(int i=0;i<countdoor;i++)
                {
                    if(rooms[singleroom.GetComponent<RoomInfo>().FollowedByRoomID-1].transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<DoorTrigger>().isshoopdoor)
                    rooms[singleroom.GetComponent<RoomInfo>().FollowedByRoomID-1].transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<Collider2D>().enabled = true;
                }
            }
            singleroom.GetComponent<GraphicsRoom>().SetWallGraphic();
            singleroom.GetComponent<GraphicsRoom>().SetGraphicDoor();
            singleroom.GetComponent<RoomEvent>().SetBossKeyIcon();
        }
    }

    public void EnablePlayer(bool enable)
    {
        GameObject.FindWithTag("Player").GetComponent<Character>().enabled    = enable;
    }
}
