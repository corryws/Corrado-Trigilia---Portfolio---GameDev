using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    /*QUESTO SCRIPT GESTISCE TUTTE LE INFORMAZIONI DI UN DETERMINATO PIANO
    -GENERA DI NUOVO IL DUNGEON SE NON VIENE GENERATO CORRETTAMENTE
    -CHE PIANO E' ES. 1,2,3 ECC...
    -QUANTE STANZE HA
    -UN MASSIMO E UN MINIMO DI STANZE IN BASE AL PIANO*/
    public int seed;
    public int floorid,Nroom,MaxRoomFloor,MinRoomFloor,RoomFloor;
    bool reset;
    public bool finderror;
    UIGameplay hudview;
    RoomTemplateScript templatesroom;

    void Start()
    {
        templatesroom = this.gameObject.GetComponent<RoomTemplateScript>();
        reset = true; 
        GenerateFloor();
        //Invoke("GenerateFloor",0.1f);
    }

//SAVE DATA FLOOR
public void SaveFloor(){GameManager.FloorDataSave(this.gameObject);}

//INCREMENT FLOOR ID AND RESET DUNGEON
    public void NextFloor()
    {
        if(floorid < 5)
        {
            templatesroom.generatefloorcomplete = false;
            floorid++;
            reset = true; 
            resetDungeon();
        }else{
            hudview = GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>();
            hudview.timeractive = false;
            hudview.ScoreBorderEnable(true);
        }
        
    }
//FUNZIONE CHE CONTROLLA SE LA GENERAZIONE DI TUTTO IL PIANO è CORRETTA
//ALTRIMENTI LA RIGENERI
    public void CheckFloor()
    {
        GameObject[] checkroom = GameObject.FindGameObjectsWithTag("SingleRoom");
        if(GameObject.FindWithTag("SpawnPoint")!=null)finderror = true;//INCORRECT != null
        else
        {
            if(Nroom != this.GetComponent<RoomTemplateScript>().rooms.Count-1)finderror = true;//INCORRECT != count
            else if(RoomFloor < 0)finderror = true;//INTANTIATE MORE ROOME OF NROOM
            else
            {
                for(int i=0;i<checkroom.Length;i++)
                {
                    for(int j=i+1;j<checkroom.Length;j++)
                      if(checkroom[i].transform.position == checkroom[j].transform.position) finderror = true;//COLLISION DETECTED NON CORRECT 
                }
            }
        }
        if(finderror)resetDungeon();
        else templatesroom.EnableGamePlay(!finderror);
    }

//QUESTA FUNZIONA RESETTA IL DUNGEON CIOE' CANCELLA TUTTO QUELLO CHE CI STA AL SUO INTERNO
    public void resetDungeon()
    {
        finderror = false;
        reset = true;
        GameObject[] deleteroom   = GameObject.FindGameObjectsWithTag("SingleRoom");
        GameObject[] deleteitem   = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] deletestatue = GameObject.FindGameObjectsWithTag("Statua");
        foreach(GameObject singleroom in deleteroom) Destroy(singleroom);
        foreach(GameObject singleitem in deleteitem) Destroy(singleitem); 
        foreach(GameObject singlestatua in deletestatue)Destroy(singlestatua); 

        for(int i=0;i<templatesroom.minimap.transform.GetChild(0).transform.childCount;i++)
        Destroy(templatesroom.minimap.transform.GetChild(0).transform.GetChild(i).gameObject);

        Destroy(GameObject.FindGameObjectWithTag("Stairs")); 
        Destroy(GameObject.FindGameObjectWithTag("Mercante")); 
        templatesroom.rooms = new List<GameObject>(0);
        templatesroom.allMiniRooms = new List<GameObject>(0);
        templatesroom.allSingleDoor = new List<GameObject>(0);
        GenerateFloor();
    }

//QUESTA FUNZIONA GENERA UNA STANZA DA CUI PARTE LA GENERAZIONE DEL DUNGEON
    public void GenerateFloor()
    {
        if(reset)
        {
            reset = false;
            templatesroom.EnableGamePlay(false);
            RoomFloor = 0;Nroom=0;
            //GENERA SEED RANDOM
            if(!PlayerPrefs.HasKey("IsSaved")) seed = Random.Range(0,99999); 
            Random.InitState(seed);
            Nroom = RoomFloor = Random.Range(MinRoomFloor,MaxRoomFloor+1);
            templatesroom.SpawnEntryRoom(Random.Range(4,templatesroom.AllRoom.Length-1));
            Invoke("CheckFloor",0.5f);
        }
    }
}
