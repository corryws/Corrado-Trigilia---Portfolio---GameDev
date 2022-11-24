using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{ 
    UIGameplay UIManagement;
    RoomTemplateScript templatesroom;
    bool playerinRange;

    void Awake(){UIManagement  = GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>();
                 templatesroom = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();}

    void Update()
    {
        if(Input.GetKeyDown("q") && playerinRange)
        {
            GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindWithTag("Player").GetComponent<Character>().insideshop = true;
            GameObject.FindWithTag("Player").transform.position = this.gameObject.transform.position;
            UIManagement.ShopEnable(true);
            templatesroom.EnablePlayer(false);
            this.enabled = false;
        }
    }
                 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) playerinRange=true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")) playerinRange=false;
    }
}
