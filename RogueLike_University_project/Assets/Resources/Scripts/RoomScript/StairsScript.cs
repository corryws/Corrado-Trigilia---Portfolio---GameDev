using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsScript : MonoBehaviour
{

    Floor floor;
    GameObject cam;

    void Awake()
    {
        floor = GameObject.FindWithTag("Rooms").GetComponent<Floor>();
        cam = GameObject.FindWithTag("MainCamera");
    }

   public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerPrefs.DeleteAll();
            cam.transform.position = new Vector3(0,0,-10);
            other.transform.position = new Vector3(0,0,0);
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<Character>().enabled    = false;
            floor.NextFloor();
        }
    }
}
