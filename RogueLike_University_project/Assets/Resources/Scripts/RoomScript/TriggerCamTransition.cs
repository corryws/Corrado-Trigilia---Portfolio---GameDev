using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamTransition : MonoBehaviour
{
    /*QUESTO SCRIPT E' ASSEGNATO AI GAMEOBJECT POSTI ALLE USCITE DELLE STANZE
    E SONO GLI EFFETTIVI TRIGGER PER CAMBIARE LA POSIZIONE DELLA TELECAMERA*/
    [Header("0 bot 1 top 2 left 3 right")]
    public int direction;
    public Vector2 CameraChange;
    public Vector3 PlayerChange;
    private CameraFollowPlayer cam;
    RoomTemplateScript templatesroom;

    void Start(){cam = Camera.main.GetComponent<CameraFollowPlayer>();}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            templatesroom = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();
            cam.minPos += CameraChange; 
            cam.maxPos += CameraChange; 
            other.transform.localPosition += PlayerChange;
            MoveMiniMapCamera();
        }else return;
    }

    void MoveMiniMapCamera()
    {   //inversed
        switch(direction)
        {
            case 0:
                templatesroom.SetXYMiniMapIcon(0f,30f);
            break;
            case 1:
                templatesroom.SetXYMiniMapIcon(0f,-30f);
            break;
            case 2:
                templatesroom.SetXYMiniMapIcon(30f,0f);
            break;
            case 3:
                templatesroom.SetXYMiniMapIcon(-30f,0f);
            break;
        }
    }
}
