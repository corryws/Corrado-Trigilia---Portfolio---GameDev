using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    /*QUESTO SCRIPT E' ASSEGNATO AI GAMEOBJECT(SPAWNPOINT) DELLE VARIE STANZE
    E SONO QUELLI CHE PERMETTONO LO SPAWN DELLE STANZE RANDOMICHE*/
    GameObject singleroom;
    RoomTemplateScript templatesroom;Floor floor;
    [Header("0 bot 1 top 2 left 3 right")]
    public int openingDirection;/*openingDirection can be: 1 ---> Bottom door B | 2 ---> top  door T | 3 ---> left  door L |4 ---> right door R*/

    void Start()
    {
        templatesroom = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();
        floor         = GameObject.FindWithTag("Rooms").GetComponent<Floor>();
        Random.InitState(floor.seed);
        Invoke("Spawn",0.1f);
    }

//QUESTA FUNZIONE AVVIA LO SPAWN SOLO SE L'OGGETO NON HA SPAWNATO
    public void Spawn(){ AssignArrayRoom(openingDirection);}

    void AssignArrayRoom(int direction)
    {
        GameObject[] ArrayRoom = new GameObject[0];
        int maxrandoomroom=0;
        if(direction == 0)     {ArrayRoom = templatesroom.bottomRooms;maxrandoomroom=6;}
        else if(direction == 1){ArrayRoom = templatesroom.topRooms;   maxrandoomroom=6;}
        else if(direction == 2){ArrayRoom = templatesroom.leftRooms;  maxrandoomroom=7;}
        else if(direction == 3){ArrayRoom = templatesroom.rightRooms; maxrandoomroom=7;}
        Random.InitState(floor.seed);
        DirectionSpawn(Random.Range(0,maxrandoomroom),ArrayRoom,this.transform.parent.transform.parent.gameObject.GetComponent<RoomInfo>().ID_ROOM);   
    }

//FUNZIONE CHE SPAWNA LE STANZE IN BASE ALLA DIREZIONE DELLO SPAWNPOINT
    void DirectionSpawn(int randomRoom,GameObject[] ArrayRoom,int id)
    {
        if(floor.RoomFloor>0)
        {
            if(id >= 1 && id <= (floor.Nroom-3))
            {
                if(randomRoom >= 5 || floor.RoomFloor <= (floor.Nroom/2)) SetRoomToInstantiate(id,ArrayRoom,0);
                else SetRoomToInstantiate(id,ArrayRoom,randomRoom);

            }else SetRoomToInstantiate(id,ArrayRoom,0); // delete--->if(id >= (floor.Nroom-2)) 
            
            floor.RoomFloor--;

        }else{floor.Nroom+=1; SetRoomToInstantiate(id,ArrayRoom,0);}
    }
//FUNZIONE CHE SETTA INFO E GRAFICA DI TUTTE LE STANZE DOPO LA PRIMA 
    void SetRoomToInstantiate(int id,GameObject[] ArrayRoom,int index)
    {
        singleroom = Instantiate(ArrayRoom[index],transform.position,ArrayRoom[index].transform.rotation);
        templatesroom.InstantiateMiniMapIcon(singleroom);
        
        if(!PlayerPrefs.HasKey("IsSaved"))
        {
            singleroom.GetComponent<RoomInfo>().FollowedByRoomID    = id;
            singleroom.GetComponent<RoomInfo>().FollowedByDirection = openingDirection;
            singleroom.GetComponent<RoomInfo>().isclose = true;
        
            if(index == 0)
            {
                singleroom.GetComponent<RoomInfo>().issingledoor = true; 
                if(floor.RoomFloor == (floor.Nroom/2)) singleroom.GetComponent<RoomInfo>().Isshooproom = true;
                if(floor.RoomFloor == 1) singleroom.GetComponent<RoomInfo>().isbossroom  = true;
                if(!singleroom.GetComponent<RoomInfo>().isbossroom && !singleroom.GetComponent<RoomInfo>().Isshooproom)    
                templatesroom.allSingleDoor.Add(singleroom);
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){if(other.CompareTag("SingleRoom")) Destroy(this.gameObject);}
}
