using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GameObject RoomParent;
    public bool isdoorboss,isshoopdoor,isbonusdoor,shopopen,bossopen;

    void Awake(){RoomParent = this.gameObject.transform.parent.gameObject.transform.parent.gameObject;}
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && !RoomParent.GetComponent<RoomInfo>().isclose)
        {
            if(isdoorboss)
            {
                if(other.gameObject.GetComponent<Character>().bosskey >= 1)
                {
                    other.gameObject.GetComponent<Character>().bosskey -= 1;
                    isdoorboss=false;bossopen = true;
                }
            }

            if(isshoopdoor)
            {
                if(other.gameObject.GetComponent<Character>().key >= 1)
                {
                    other.gameObject.GetComponent<Character>().key -= 1;
                    isshoopdoor=false;shopopen=true;
                }
            }
            RoomParent.GetComponent<GraphicsRoom>().SetGraphicDoor();       
        }
    }

    
}
