using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    /*
    QUESTO SCRIPT FA SI CHE LA TELECAMERA EFFETTUA UNA TRANSIZIONE TRA UNA STANZA E L'ALTRA NON
    APPENA IL PLAYER TRIGGERA GLI OBJECT POSIZIONATI ALLE USCITE DELLE STANZE
    */
    Transform target;
    public Vector2 maxPos,minPos;
    public float smoothing;

    public void SaveMainCamera(){GameManager.MainCameraDataSave(this.gameObject);}

    void LateUpdate(){SmothingCamera();}

public void SmothingCamera()
    {
        target = GameObject.FindWithTag("Player").transform;
        if(this.transform.position != target.position)
        {
            Vector3 targetPos  = new Vector3 (target.position.x,target.position.y,this.transform.position.z);
            targetPos.x        = Mathf.Clamp (targetPos.x,minPos.x,maxPos.x); 
            targetPos.y        = Mathf.Clamp (targetPos.y,minPos.y,maxPos.y);
            transform.position = Vector3.Lerp(transform.position,targetPos,smoothing);
        }
    }
}
