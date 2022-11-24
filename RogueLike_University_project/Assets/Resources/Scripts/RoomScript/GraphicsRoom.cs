using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsRoom : MonoBehaviour
{
    /*TRAMITE QUESTO SCRIPT GESTISCO LA GRAFICA DELLE VARIE STANZA,IN PARTICOLARE IN BASE AL PIANO CAMBIO GLI SPRITE MURO E PAVIMENTO*/
    GameObject Wall,Door;Floor floor;RoomTemplateScript templatesroom;
    void Awake()
    {
        templatesroom = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();
        floor         = GameObject.FindWithTag("Rooms").GetComponent<Floor>(); 
        
        for(int i=0;i<this.gameObject.transform.childCount;i++)
        {Door=SetAwakeObject(i,"Doors",Door);Wall=SetAwakeObject(i,"Walls",Wall);}
    }
    
//FUNZIONE CHE SETTA I GAMEOBJECT NELL'AWAKE
     GameObject SetAwakeObject(int i,string Tag,GameObject var)
    {if(this.gameObject.transform.GetChild(i).CompareTag(Tag))var
    =this.gameObject.transform.GetChild(i).gameObject; return var;}
    

//FUNZIONI CHE SETTANO LA GRAFICA DEI WALL
     public void SetWallGraphic()
    {
        /*layer 8-top_wall/bot_wall | 9-right_wall/left_wall | 10-top_angle |11-bot_angle*/
        SetGround();
        for(int i=0;i<Wall.transform.childCount;i++)
        {SetWall(8,i,floor.floorid,"/Wall_1");SetWall(9,i,floor.floorid,"/Wall_2");
        SetWall(10,i,floor.floorid,"/Wall_0");SetWall(11,i,floor.floorid,"/Wall_3");}
    }

    void SetGround()
    {
        string groundpath = "";
        if(this.gameObject.GetComponent<RoomInfo>().Isshooproom)groundpath = "Tileset/shoop_tileset/Ground";
        else if(this.gameObject.GetComponent<RoomInfo>().isbonusroom)groundpath = "Tileset/bonus_tileset/Ground";
        else groundpath = "Tileset/floor_"+floor.floorid+"/Ground";
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(groundpath);
    }

    void SetWall(int layer,int i,int floorid,string WallPath)
    {
        string wallpath = "";
        if(Wall.transform.GetChild(i).gameObject.layer == layer)
        {
            if(this.gameObject.GetComponent<RoomInfo>().Isshooproom)wallpath = "Tileset/shoop_tileset"+WallPath;
            else if(this.gameObject.GetComponent<RoomInfo>().isbonusroom)wallpath = "Tileset/bonus_tileset"+WallPath;
            else wallpath = "Tileset/floor_"+floorid+WallPath; 
            Wall.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite =Resources.Load<Sprite>(wallpath);
        }
        
    }
//FUNZIONE CHE SETTA LA GRAFICA DELLE PORTE BOSS E NON
    public void SetGraphicDoor(){for(int i=0;i<Door.transform.childCount;i++)SetDoor(i,this.gameObject.GetComponent<RoomInfo>().isclose);}

    public void SetDoor(int i,bool enable)
    {
        string DoorPath = "",path = "";
        if (enable == true) DoorPath = "/Door_Close";
        else                DoorPath = "/Door_Open";

        if(!Door.transform.GetChild(i).GetComponent<DoorTrigger>().isdoorboss && 
           !Door.transform.GetChild(i).GetComponent<DoorTrigger>().isshoopdoor)
        {
            if(this.gameObject.GetComponent<RoomInfo>().isbossroom)
                 path = "Tileset/general_floor"+DoorPath+"_Boss_1";

            else if(this.gameObject.GetComponent<RoomInfo>().Isshooproom)
                 path = "Tileset/shoop_tileset"         +DoorPath;

            else if(this.gameObject.GetComponent<RoomInfo>().isbonusroom) 
                path = "Tileset/bonus_tileset"         +DoorPath;

            else if(Door.transform.GetChild(i).GetComponent<DoorTrigger>().shopopen) 
                 path = "Tileset/shoop_tileset/Door_Open";
            
            else if(Door.transform.GetChild(i).GetComponent<DoorTrigger>().bossopen) 
                 path = "Tileset/general_floor/Door_Open_Boss_1";

            else path = "Tileset/floor_"+floor.floorid  +DoorPath;

            Door.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite    = Resources.Load<Sprite>(path);
            Door.transform.GetChild(i).GetComponent<Collider2D>().enabled = enable;
            Door.transform.GetChild(i).GetComponent<Animator>      ().enabled   = false;
        }
    }


    //SETTA LE PORTE ADIACENTI ALLE STANZA BOSS E DELLO SHOP
    public void SetBossOrShoopDoor(int ID_ROOM,int FollowedID,bool isbossroom,bool isshooproom,bool isbonusroom)
    {
        Transform FollowedTemplateDoor = templatesroom.rooms[FollowedID]
                                         .gameObject.GetComponent<GraphicsRoom>()
                                         .Door.transform.GetChild(GetCorrectChildID
                                         (this.gameObject.GetComponent<RoomInfo>().FollowedByDirection));
         
        if(ID_ROOM != 1)
        {
            if(isbossroom)
            {
                FollowedTemplateDoor.GetComponent<DoorTrigger>().isdoorboss = true;
                FollowedTemplateDoor.GetComponent<Animator>().      enabled = true;
            }
            if(isshooproom)
            {
                FollowedTemplateDoor.GetComponent<DoorTrigger>().isshoopdoor = true;
                FollowedTemplateDoor.GetComponent<Animator>().      enabled  = false;

                FollowedTemplateDoor.GetComponent<SpriteRenderer>().sprite 
                = Resources.Load<Sprite>("Tileset/shoop_tileset/Door_Close");
            }
                FollowedTemplateDoor.GetComponent<Collider2D>().isTrigger    = false;
        }

        if(FollowedTemplateDoor.GetComponent<DoorTrigger>().isshoopdoor &&
           FollowedTemplateDoor.GetComponent<DoorTrigger>().shopopen)
           {
               FollowedTemplateDoor.GetComponent<DoorTrigger>().isshoopdoor = false;
               FollowedTemplateDoor.GetComponent<Animator>().      enabled  = false;
               FollowedTemplateDoor.GetComponent<SpriteRenderer>().sprite 
                = Resources.Load<Sprite>("Tileset/shoop_tileset/Door_Open");
           }

        if(FollowedTemplateDoor.GetComponent<DoorTrigger>().isdoorboss &&
           FollowedTemplateDoor.GetComponent<DoorTrigger>().bossopen)
           {
               FollowedTemplateDoor.GetComponent<DoorTrigger>().isdoorboss = false;
               FollowedTemplateDoor.GetComponent<Animator>().      enabled  = false;
           }
        
       
    }

   public  int GetCorrectChildID(int ThisFollowedByDirection)
    {   
        int correctchildid = 0;
        if(ThisFollowedByDirection >= 3)     correctchildid = 2;
        else if(ThisFollowedByDirection == 2)correctchildid = 3;
        else if(ThisFollowedByDirection == 1)correctchildid = 0;
        else if(ThisFollowedByDirection == 0)correctchildid = 1;
        return correctchildid;
    }
//-----------------------------------
}
