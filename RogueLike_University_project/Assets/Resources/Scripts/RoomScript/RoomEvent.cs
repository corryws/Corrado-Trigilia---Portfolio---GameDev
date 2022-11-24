using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEvent : MonoBehaviour
{
    /*QUESTO SCRIPT GESTISCE TUTTI GLI EVENTI DI UNA DETERMINATA STANZA*/
    Floor floor;
    GameObject Mercante;
    RoomTemplateScript templatesroom;
    SoundManager sound;
    void Awake()
    {
        floor = GameObject.FindWithTag("Rooms").GetComponent<Floor>();
        templatesroom = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();
        sound = GameObject.FindWithTag("Sounds").GetComponent<SoundManager>();
        this.gameObject.GetComponent<EnemySpawn>().enabled = false;
    }

    void Update()
    {
        if(this.gameObject.GetComponent<RoomInfo>().isclose)
        {
            for(int i=0;i<this.gameObject.transform.GetChild(2).transform.childCount;i++)
            {
                if(this.gameObject.transform.GetChild(2).transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    this.gameObject.transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<Collider2D>().enabled = true;
                    this.gameObject.transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<Collider2D>().isTrigger = false;
                }
                 
            }
        }
    }
    
    //EVENTO TRIGGER ENTER
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SingleRoom"))Destroy(other.gameObject);

        if(other.CompareTag("Player")) 
        {
            GameObject[] deletebullet = GameObject.FindGameObjectsWithTag("Bullet");
            foreach(GameObject singlebullet in deletebullet) Destroy(singlebullet);
            ChangeMiniMapColor(Color.white);
            other.GetComponent<Character>().ResetUseActivableBool();

            if(this.gameObject.GetComponent<RoomInfo>().isclose && GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>().generatefloorcomplete)
            {
                if(this.gameObject.GetComponent<RoomInfo>().isbossroom) 
                {
                    StartCoroutine(GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>().BossScreenEnable(this.gameObject));
                    sound.ChangeAudioSource("Audio/OST/Boss/boss"+floor.floorid+"ost");
                }else this.gameObject.GetComponent<EnemySpawn>().enabled = true;
            }
              
            if(this.gameObject.GetComponent<RoomInfo>().Isshooproom)
            {
                SpawnMercante(); 
                sound.ChangeAudioSource("Audio/HUDSound/Shop_Room");
            } 

            if(this.gameObject.GetComponent<RoomInfo>().isbonusroom)sound.ChangeAudioSource("Audio/HUDSound/Shop_Room");
        }
    }

    void OnTriggerStay2D (Collider2D other)
    {
        if(other.CompareTag("SingleRoom"))Destroy(other.gameObject);
        if(other.CompareTag("Player")) 
        {
             if(this.gameObject.GetComponent<RoomInfo>().isbonusroom 
             && !this.gameObject.GetComponent<RoomInfo>().bosskeytaked
             && !this.gameObject.GetComponent<RoomInfo>().bosskeyspawned) 
             {this.gameObject.GetComponent<RoomInfo>().bosskeyspawned = true;BonusRoomSpawnItem();}

             if(this.gameObject.GetComponent<RoomInfo>().Isshooproom 
             && !this.gameObject.GetComponent<RoomInfo>().shopspawned) 
             {this.gameObject.GetComponent<RoomInfo>().shopspawned = true;SpawnMercante();}

             if(this.gameObject.GetComponent<RoomInfo>().isbossroom 
             && this.gameObject.GetComponent<RoomInfo>().stairspawned) 
             {this.gameObject.GetComponent<RoomInfo>().stairspawned = false;SpawnStair();}

            GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>().SetEscButton(this.gameObject.GetComponent<RoomInfo>());
        }
    }

void OnTriggerExit2D(Collider2D other)
{
    if(other.CompareTag("Player"))
    {
        for(int i=0;i<this.transform.childCount;i++)
        {
            if(this.transform.GetChild(i).CompareTag("Item"))ChangeMiniMapColor(Color.red);
            else ChangeMiniMapColor(Color.green);
        }
        
        if(this.gameObject.GetComponent<RoomInfo>().isbonusroom 
           || this.gameObject.GetComponent<RoomInfo>().Isshooproom
           || this.gameObject.GetComponent<RoomInfo>().isbossroom)
           {
               sound.ChangeAudioSource("Audio/OST/Floor/floor"+floor.floorid+"ost");
           }
    } 
}
void ChangeMiniMapColor(Color color){
    templatesroom.allMiniRooms[this.gameObject.GetComponent<RoomInfo>().CurrentMiniMapIcon].GetComponent<Image>().color = color;}

//EVENTO SPAWN MERCANTE
    void SpawnMercante()
    {
        if(Mercante == null){
            Mercante = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Mercante");
            Instantiate(Mercante,this.transform.position,Quaternion.identity);
            templatesroom.shopsystem.GetComponent<ShopSystem>().RandomItem();
        }   
    }

    //BONUS ROOM
    void BonusRoomSpawnItem()
    {
        GameObject item = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Item");
        GameObject spawneditem = Instantiate(item,this.transform.position,Quaternion.identity);
        spawneditem.GetComponent<ItemSetting>().SetItem(3);
        spawneditem.transform.SetParent(this.transform,true);
    }

    void SpawnStair()
    {
        GameObject StairsPrefab = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Stairs");
        Instantiate(StairsPrefab,this.transform.position,Quaternion.identity);
    }

    public void SetBossKeyTake(){this.gameObject.GetComponent<RoomInfo>().bosskeytaked = true;}
    

    //EVENTO CHECK BOSS OR SHOOP DOOR ADIACENTE
    public void CheckBossOrShoopBool()
    {
        int ID_ROOM = this.gameObject.GetComponent<RoomInfo>().ID_ROOM;
        int FollowedByRoomID = this.gameObject.GetComponent<RoomInfo>().FollowedByRoomID;
        bool isbossroom = this.gameObject.GetComponent<RoomInfo>().isbossroom;
        bool Isshooproom = this.gameObject.GetComponent<RoomInfo>().Isshooproom;
        bool isbonusroom = this.gameObject.GetComponent<RoomInfo>().isbonusroom;
        this.GetComponent<GraphicsRoom>().SetBossOrShoopDoor(ID_ROOM,FollowedByRoomID-1,isbossroom,Isshooproom,isbonusroom);
    }

    public void SetBossKeyIcon()
    {
        if(this.gameObject.GetComponent<RoomInfo>().isbossroom)
        templatesroom.allMiniRooms[this.gameObject.GetComponent<RoomInfo>().CurrentMiniMapIcon].GetComponent<Image>().sprite 
        = Resources.Load<Sprite>("HUD/MiniMapIconBoss");

        if(this.gameObject.GetComponent<RoomInfo>().Isshooproom)
        templatesroom.allMiniRooms[this.gameObject.GetComponent<RoomInfo>().CurrentMiniMapIcon].GetComponent<Image>().sprite 
        = Resources.Load<Sprite>("HUD/MiniMapIconShop");

        if(this.gameObject.GetComponent<RoomInfo>().isbonusroom)
        templatesroom.allMiniRooms[this.gameObject.GetComponent<RoomInfo>().CurrentMiniMapIcon].GetComponent<Image>().sprite 
        = Resources.Load<Sprite>("HUD/MiniMapIconBonus");
    }
}
