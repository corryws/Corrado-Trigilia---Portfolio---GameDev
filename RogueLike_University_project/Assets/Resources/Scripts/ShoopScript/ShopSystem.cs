using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    UIGameplay UIManagement;
    RoomTemplateScript templatesroom;
    Item item;Character player;
    public GameObject block_item,block_descrizione,cursor;
    public int cursorpoint,item_id;
    public int [] costArray,listItem;
    public bool[] purchasedItem;
    string [] descriptionArray;
    float nextmove = 0f,nextpurchase = 0f;
    
    void Awake(){UIManagement  = GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>();
                 templatesroom = GameObject.FindWithTag("Rooms")       .GetComponent<RoomTemplateScript>();
                 player       = GameObject.FindWithTag("Player")      .GetComponent<Character>();
                 cursor.transform.position   = block_item.transform.GetChild(block_item.transform.childCount-1).transform.position;
                 cursor.transform.localScale = block_item.transform.GetChild(block_item.transform.childCount-1).transform.localScale;}
    void FixedUpdate()
    {
        MoveCursor();
        SetItemPurchased();
        ConfirmItem();
    }

public void RandomItem()
    {
        descriptionArray = new string[block_item.transform.childCount-1];
        listItem = new int[block_item.transform.childCount-1]; 
        costArray = new int[block_item.transform.childCount-1]; 
        purchasedItem = new bool[block_item.transform.childCount-1];
        //int[] item_dropped={0,1,2,3,4},item_stats={6,7,10,11},activable_item={8,9,12};
        for(int i=0;i<block_item.transform.childCount-1;i++)
        {
            do{
                item_id = Random.Range(0,13);
            }while(item_id == 2 || item_id == 5);

            listItem[i] = item_id;
        }
        SetDuplicate();
        for(int i=0;i<block_item.transform.childCount-1;i++)
        {
            item = Resources.Load<Item>("ScriptableObject/Items/item_" + listItem[i]);
            block_item.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = item.ShopItemSprite;
            block_item.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = item.name.ToUpper();
            block_item.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = (""+(item.cost)).ToUpper();
            costArray[i] = item.cost;
            descriptionArray[i] = item.description.ToUpper();
        }
    }

void SetDuplicate()
{
    for(int i=0;i<listItem.Length;i++)
        {
            for(int j=i+1;j<listItem.Length;j++)
            {
                if(listItem[i] == listItem[j])
                {
                    if(listItem[i] == 0 && listItem[j] == 0)listItem[j] = listItem[i] = 12;
                    do{
                        listItem[j]  = Random.Range(0,listItem[j]);
                    }while(listItem[j] == 2 || listItem[j] == 5);
                }
            }
        }
}

public void SetItemPurchased()
{
    for(int i=0;i<block_item.transform.childCount-1;i++)
    {
        if(purchasedItem[i] == true)
        {
            block_item.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "--";
            block_item.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = ("---");
        }
    }
}

    void MoveCursor()
    {  
        if(Time.time > nextmove)
        {
            if(Input.GetKey("w") || Input.GetKey("up") && cursorpoint >= 1) cursorpoint--;
            if(Input.GetKey("s") || Input.GetKey("down") && cursorpoint < block_item.transform.childCount-1) cursorpoint++;
            
            if(cursorpoint < 0) cursorpoint = block_item.transform.childCount-1;
            else if(cursorpoint > block_item.transform.childCount-1)cursorpoint = 0;
            
            cursor.transform.position = block_item.transform.GetChild(cursorpoint).transform.position;
            cursor.GetComponent<RectTransform>().sizeDelta 
            = new Vector2(block_item.transform.GetChild(cursorpoint)
            .GetComponent<RectTransform>().rect.width,block_item.transform.GetChild(cursorpoint)
            .GetComponent<RectTransform>().rect.height);

            if(cursorpoint < block_item.transform.childCount-1)
            block_descrizione.transform.GetChild(0).GetComponent<Text>().text  = descriptionArray[cursorpoint];
            else block_descrizione.transform.GetChild(0).GetComponent<Text>().text  = "VAI GIA' VIA? BIP BIP?";

            nextmove = Time.time + 0.1f;
        }
    }


    public void ConfirmItem()
    {
        if(Time.time > nextpurchase)
        {
            if(Input.GetKeyDown("q"))
            {
                if(cursorpoint == 4)
                {
                    Invoke("ExitShop",0.1f);
                }else GetPurchase();
            }
            nextpurchase = Time.time + 0.01f;
        }
    }

    void ExitShop()
    {
        UIManagement.ShopEnable(false);
        templatesroom.EnablePlayer(true);
        GameObject.FindWithTag("Player").GetComponent<Character>().insideshop = false;
        GameObject.FindWithTag("Mercante").GetComponent<ShopTrigger>().enabled = true;
    }
    void GetPurchase(){Purchase(cursorpoint);}
    void Purchase(int arrayid)
    {
        if(player.coin >= costArray[arrayid] && purchasedItem[arrayid]==false) 
         Resources.Load<ItemSetting>("Prefabs/GameplayPrefab/Item").purchaseFunction(listItem[arrayid],arrayid,this);
    }
}
