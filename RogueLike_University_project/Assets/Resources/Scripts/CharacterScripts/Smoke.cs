using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public bool isenemysmoke;
    void Start(){Destroy(this.gameObject,1f);}
}
