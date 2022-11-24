using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    GameObject Obstacle;
    GameObject[] ObstacleSpawned;
    int numberOfObstacle;

    // Start is called before the first frame update
    void Start()
    {
        Obstacle = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Obstacle");
        numberOfObstacle = Random.Range(1,6);
        ObstacleSpawned = new GameObject[numberOfObstacle];
        SpawnObstacle();
    }

    public void SpawnObstacle()
    {
        for(int i=0;i<numberOfObstacle;i++)
        {
            ObstacleSpawned[i] = Instantiate(Obstacle,new Vector3(this.transform.position.x+Random.Range(0f,1f)+i,this.transform.position.y+Random.Range(0f,1f)+i, -1),Quaternion.identity);
            ObstacleSpawned[i].transform.SetParent(this.transform,true); 
        }
    }
}
