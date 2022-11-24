using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    /*QUESTO SCRIPT PERMETTE DI AGGIUNGERE LE STANZE SPAWNATE IN UNA LISTA
    E SALVO L'"ID" DELLA STANZA IN MODO DA POTER SAPERE
    ESATTAMENTE CHE STANZA E QUINDI GESTIRE I SUOI EVENTI */
    RoomTemplateScript templatesroom;

    // Start is called before the first frame update
    void Awake()
    {
        templatesroom = GameObject.FindWithTag("Rooms").GetComponent<RoomTemplateScript>();
        templatesroom.rooms.Add(this.gameObject);
        this.gameObject.GetComponent<RoomInfo>().ID_ROOM = templatesroom.rooms.Count;
    }
}
