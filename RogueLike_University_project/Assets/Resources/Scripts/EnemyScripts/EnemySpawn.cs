using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    /*
    MEDIANTE QUESTO SCRIPT MI GESTISCO LO SPAWN RANDOM DEI NEMICI
    */
    GameObject Enemy;
    GameObject StairsPrefab;
    GameObject[] EnemySpawned;
    int numberOfEnemy;
    public int EnemyRemain;

    void Start()
    {
        Enemy        = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Enemy");
        StairsPrefab = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Stairs");
        SetEnemyToSpawn();
    }

//SET ENEMY TO SPAWN
    public void SetEnemyToSpawn()
    {
        EnemyRemain = GetNumberEnemyToSpawn(this.gameObject.GetComponent<RoomInfo>().isbossroom,
                                            this.gameObject.GetComponent<RoomInfo>().Isshooproom,
                                            this.gameObject.GetComponent<RoomInfo>().isbonusroom);

        EnemySpawned = new GameObject[EnemyRemain]; 
        CheckNumberOfEnemy();
    }

    int GetNumberEnemyToSpawn(bool isbossroom,bool isshsooproom,bool isbonusroom)
    {
        if(isbossroom) numberOfEnemy = 1;
        else if(isshsooproom || isbonusroom){numberOfEnemy = 0;}
        else numberOfEnemy = Random.Range(1,5);
        return numberOfEnemy;
    }

    void CheckNumberOfEnemy()
    {
        if(EnemyRemain == 0)CheckedEnemyRemain();
        else SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        for(int i=0;i<numberOfEnemy;i++)
        {
            if(this.gameObject.GetComponent<RoomInfo>().isbossroom == true) SetPosEnemyToSpawn(i,0,0,true);
            else SetPosEnemyToSpawn(i,Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f),false);     
        }
    }

    public void SetPosEnemyToSpawn(int index,float xpos,float ypos,bool isboss)
    {
        EnemySpawned[index] = Instantiate(Enemy,this.transform.position,Quaternion.identity);
        EnemySpawned[index].transform.SetParent(this.transform,true);
        EnemySpawned[index].transform.localPosition = new Vector2(xpos+0.20f,ypos+0.20f);

        CircleCollider2D enemycollider = EnemySpawned[index].gameObject.GetComponent<CircleCollider2D>();
        Rigidbody2D enemyrigidb = EnemySpawned[index].GetComponent<Rigidbody2D>();
        if(isboss)EnemySpawned[index].transform.localScale = new Vector2(1.5f,2.5f);
        
        enemyrigidb.bodyType = RigidbodyType2D.Kinematic;
        EnemySpawned[index].GetComponent<EnemyIA>().isboss = isboss;
        GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>().SetEnemyToPauseArray();
    }

//CHECK ENEMY IN A SINGLE ROOM
    public void CheckedEnemyRemain()
    {
        if(this != null)
        {
            if(EnemyRemain <= 0) 
            {
                GameObject.Find("DoorSound").GetComponent<AudioSource>().Play();
                this.gameObject.GetComponent<RoomInfo>().isclose = false;
                this.gameObject.GetComponent<GraphicsRoom>().SetGraphicDoor();
                
                if(this.gameObject.GetComponent<RoomInfo>().Isshooproom == true) Destroy(this);
                else if(this.gameObject.GetComponent<RoomInfo>().isbossroom == true)
                {
                    this.gameObject.GetComponent<RoomInfo>().defeatedboss = true;
                    this.gameObject.GetComponent<RoomInfo>().stairspawned = true;
                }

                GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>().IncrementActivableLoadBar();
                Destroy(this);
            }else this.gameObject.GetComponent<RoomInfo>().isclose = true;
        }
    }
}
