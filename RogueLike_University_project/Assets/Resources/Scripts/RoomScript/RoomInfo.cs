using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    /*
    QUESTO SCRIPT HA TUTTE LE INFORMAZIONI DI UNA DETERMINATA STANZA
    IN PARTICOLARE 
    ID 
    E SE SI TRATTA DI UNA BOSS ROOM O SHOOPROOM
    DA QUALE STANZA VIENE SEGUITA E IN QUALE DIREZIONE
    */
    public int ID_ROOM,FollowedByRoomID,FollowedByDirection,CurrentMiniMapIcon;
    public bool isbossroom,Isshooproom,isbonusroom,issingledoor,isclose,
                bosskeytaked,bosskeyspawned,shopspawned,stairspawned,
                defeatedboss;
}
