using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetting : MonoBehaviour
{
    Character player;
    Item item;
    //0 = bomb | 1 = key | 2 = coin |3 = containerheart | 4 = heart | 5 = bosskey 
    //6 = forza | 7 = grilletto | 11 = speed | 10 = raggio | 8 = laser | 9 = mirino | 12 = sigaro|
    int[] item_dropped={0,1,2,4};
    int[] item_stats={6,7,10,11};
    int[] activable_item={8,9,12};
    int item_id;
    public bool ispicked,activable_change;

    //void Awake(){SetItem(0);}

    public void SetItem(int type)
    {
        item_id = GetRandomItem(type);
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
        if(player.key>=5 && item_id == 1)item_id = 2;
        item = Resources.Load<Item>("ScriptableObject/Items/item_" + item_id);//item_itemid
        this.gameObject.GetComponent<SpriteRenderer>().sprite = item.ItemSprite;
        this.gameObject.GetComponent<AudioSource>().clip = item.pickclip;
    }

    int GetRandomItem(int type)
    {
        int item_returned=0,randomitem = 0;
        switch(type)
        {
            case 0://dropped
            randomitem = Random.Range(0,item_dropped.Length);
            item_returned = item_dropped[randomitem];
            break;

            case 1://stats
            randomitem = Random.Range(0,item_stats.Length);
            item_returned = item_stats[randomitem];
            break;

            case 2://activable
            randomitem = Random.Range(0,activable_item.Length);
            item_returned = activable_item[randomitem];
            break;

            case 3://bosskey
            GameObject statue = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Statue");
            Instantiate(statue,this.transform.position,Quaternion.identity);
            item_returned = 5;
            break;

            case 4://laser
            item_returned = 8;
            break;

            case 5://mirino
            item_returned = 9;
            break;

            case 6://raggio
            item_returned = 12;
            break;
        }
        
        return item_returned;
    }

    void SetTriggerItem()
    {
        if(item_id != 3 && item_id != 4) this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        else
        {
            if(item_id == 4 && player.life >= player.container || item_id == 3 && player.container >= 12)
            this.gameObject.GetComponent<Collider2D>().isTrigger = false; else this.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }

//FUNZIONI DI PICK ITEM
     void PickFunction(int item_id)
    {
        //consumabili principali
        if(item_id == 0)PickBomb();
        else if(item_id == 1)PickKey();
        else if(item_id == 2)PickCoin();
        else if(item_id == 4)PickHeart();
        else if(item_id == 5)PickBossKey();

        //consumabili stats
        else if(item_id == 6)PickStrenght();
        else if(item_id == 7)PickGrilletto();
        else if(item_id == 11)PickSpeed();
        else if(item_id == 10)PickRaggio();

        //activable
        else if(item_id == 8 && !ispicked)PickLaser();
        else if(item_id == 9 && !ispicked)PickMirino();
        else if(item_id == 12 && !ispicked)PickSigaro();
    }

    IEnumerator PlaySound()
    {
        this.gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.25f);
        if(item_id != 8 && item_id != 9 && item_id != 12 || !activable_change)Destroy(this.gameObject);
    }


    void PickBomb()     {if(player.bombs   < 99)player.bombs++;              StartCoroutine(PlaySound());}
    void PickKey()      {if(player.key     < 99)player.key++;                StartCoroutine(PlaySound());}
    void PickCoin()     {if(player.coin    < 99)player.coin++;               StartCoroutine(PlaySound());}
    void PickBossKey()  {if(player.bosskey < 99)player.bosskey++;            this.transform.parent.GetComponent<RoomEvent>().SetBossKeyTake(); StartCoroutine(PlaySound());}
    void PickHeart()    {if(player.life < player.container){player.life++;   StartCoroutine(PlaySound());}}

    void PickStrenght() {if(player.strenght   < 10)  {player.strenght++;      StartCoroutine(PlaySound());}}
    void PickGrilletto(){if(player.dps        > 0.1f){player.dps-=0.1f;       StartCoroutine(PlaySound());}}
    void PickSpeed()    {if(player.speed      < 10)  {player.speed++;         StartCoroutine(PlaySound());}}
    void PickRaggio()   {if(player.shootspeed < 10)  {player.shootspeed+=0.5f;StartCoroutine(PlaySound());}}

    void PickLaser() {PickActivable();player.activable_laser = true;StartCoroutine(PlaySound());}
    void PickMirino(){PickActivable();player.activable_mirino = true;StartCoroutine(PlaySound());}
    void PickSigaro(){PickActivable();player.activable_sigaro = true;StartCoroutine(PlaySound());}

    void PickActivable()
    {
        CharacterDropItem();
        player.ResetUseActivableBool();
        player.SetActivableBool();
    }

    void CharacterDropItem()
    {
        if(player.activable_laser)DroppedItem(4);
        else if(player.activable_mirino)DroppedItem(5);
        else if(player.activable_sigaro)DroppedItem(6);
    }

    void DroppedItem(int itemid)
    {
        Vector2 newpos = new Vector2(this.transform.position.x,this.transform.position.y);
        GameObject spawneditem = Instantiate(this.gameObject,newpos,Quaternion.identity);
        spawneditem.GetComponent<ItemSetting>().ispicked = true;
        spawneditem.GetComponent<ItemSetting>().SetItem(itemid);
        activable_change = true;
        if(activable_change)Destroy(this.gameObject);
    }

//FUNZIONI DI PURCHASE ITEM
    public void purchaseFunction(int item_id,int cursorpoint,ShopSystem shop)
    {
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
        item = Resources.Load<Item>("ScriptableObject/Items/item_" + item_id);
        //consumabili principali
        if(item_id == 0)purchaseBomb(player,item,cursorpoint,shop);
        else if(item_id == 1)purchaseKey(player,item,cursorpoint,shop);
        else if(item_id == 3)purchaseContainer(player,item,cursorpoint,shop);
        else if(item_id == 4)purchaseHeart(player,item,cursorpoint,shop);

        //consumabili stats
        else if(item_id == 6)purchaseStrenght(player,item,cursorpoint,shop);
        else if(item_id == 7)purchaseGrilletto(player,item,cursorpoint,shop);
        else if(item_id == 11)purchaseSpeed(player,item,cursorpoint,shop);
        else if(item_id == 10)purchaseRaggio(player,item,cursorpoint,shop);

        //activable
        else if(item_id == 8)purchaseLaser(player,item,cursorpoint,shop);
        else if(item_id == 9)purchaseMirino(player,item,cursorpoint,shop);
        else if(item_id == 12)purchaseSigaro(player,item,cursorpoint,shop);
    }

    void purchaseBomb(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.bombs < 99){player.bombs++;player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}}

    void purchaseKey(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.key < 99)  {player.key++;  player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}}

    void purchaseHeart(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.life < player.container) {player.life++; player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}    else return;}

    void purchaseContainer(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.container  < 12)         {player.container++;player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}else return;}


    void purchaseStrenght(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.strenght < 10){player.strenght++;         player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}}

    void purchaseGrilletto(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.dps >  0.1f)  {player.dps-=0.1f;          player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}}

    void purchaseSpeed(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.speed < 10)   {player.speed++;            player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}}

    void purchaseRaggio(Character player,Item item,int cursorpoint,ShopSystem shop)
    {if(player.shootspeed < 10) {player.shootspeed+=0.5f; player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}}

    void purchaseLaser(Character player,Item item,int cursorpoint,ShopSystem shop){player.ResetUseActivableBool();player.SetActivableBool();player.activable_laser = true;player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}
    void purchaseMirino(Character player,Item item,int cursorpoint,ShopSystem shop){player.ResetUseActivableBool();player.SetActivableBool();player.activable_mirino = true;player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}
    void purchaseSigaro(Character player,Item item,int cursorpoint,ShopSystem shop){player.ResetUseActivableBool();player.SetActivableBool();player.activable_sigaro = true;player.coin -= item.cost;shop.purchasedItem[cursorpoint]=true;}
//-----------------------------------------------------------------------------------------------------------

//FUNZIONI DI GESTIONI COLLISIONI E TRIGGER
    void OnTriggerEnter2D(Collider2D other){if(other.gameObject.CompareTag("Player"))PickFunction(item_id);}
    void OnTriggerExit2D(Collider2D other){if(other.gameObject.CompareTag("Player"))ispicked = false;}
}
