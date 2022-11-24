using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /*QUESTO SCRIPT MI PERMETTE DI CAPIRE E/O GESTIRE TUTTE LE VARIABILI CHE HA IL 
    PLAYER DURANTE IL GIOCO, QUESTO SCRIPT VERRA' RICHIAMATO DAGLI ALTRI SCRIPT
    APPUNTO PER USARE LE SUE VARIABILI*/
    [Header("VARIABILI ITEM CONSUMABILI")] 
    public float life;
    public int container,bombs,bosskey,key,coin;

    [Header("VARIABILI STATISTICHE")]
    public int strenght;
    public float speed,shootspeed,dps,nextshoot;

    [Header("VARIABILI ITEM ATTIVABILI")]
    public bool activable_laser;
    public bool activable_mirino,activable_sigaro;
    public bool use_laser,use_mirino,use_sigaro;
    
    [Header("ALTRE VARIABILI")]
    public bool vulnerability,insideshop;
    UIGameplay hudview;

    public void Awake(){life = container;hudview = GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>();} 
    public void Update(){SetLife();}
    public void FixedUpdate(){if(!insideshop)this.GetComponent<CharacterMovement>().AnimationDirection();}

    public void SavePlayer(){GameManager.PlayerDataSave(this.gameObject);}

    public void SetActivableBool(){activable_laser = activable_mirino = activable_sigaro = false;}
    public void ResetUseActivableBool(){this.gameObject.GetComponent<LineRenderer>().enabled = use_laser = use_mirino = use_sigaro = false;}

    void SetLife()
    {
        if(container >= 12)container = 12; 
        if(life >= container)life = container; 
        if(life <= 0)
        {
            hudview.DeathScreenEnable(true);
            this.GetComponent<CharacterShoot>().enabled = 
            this.GetComponent<CharacterMovement>().enabled = false;
        }
    }

    public void TakeDamage(float damage)
    {
        vulnerability = true;
        if(vulnerability) 
        {
            life-=damage;
            StartCoroutine(SetInvincible(true));
        }
    }

    public IEnumerator SetInvincible(bool isvulnerabile)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        vulnerability = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Smoke") && other.gameObject.GetComponent<Smoke>().isenemysmoke)
        {
            TakeDamage(0.5f);
            Destroy(other);
        }   
    }
}
