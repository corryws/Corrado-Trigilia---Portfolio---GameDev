using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    /*
    QUESTO SCRIPT PERMETTE DI GESTIRE TUTTE LE MECCANICHE DI MOVIMENTO DEL GIOCATORE
    QUINDI MUOVERSI MEDIANTE WASD E SELEZIONARE L'ANIMAZIONE DELLA DIREZIONE IN
    MODO CORRETTO
    */
    Character player;Animator anim;
    int setvaluex=0;int setvaluey=0;
    void Awake(){player = GameObject.FindWithTag("Player").GetComponent<Character>(); anim = this.gameObject.GetComponent<Animator>();}
    
//FUNZIONE MOVIMENTO
    public void Movement(float Horizontal,float Vertical)
    {
        Vector2 newpos = new Vector2(Horizontal * player.speed, Vertical * player.speed);
        this.gameObject.GetComponent<Rigidbody2D>().velocity = newpos;
    }
//FUNZIONE GESTORE ANIMAZIONE
    public void AnimationDirection(){ SetAnimationDirection();} 

    void SetAnimationDirection()
    {
            if(Input.GetKey("w")){setvaluex = 0;setvaluey=1;}
            else if(Input.GetKey("a")){setvaluex=-1; setvaluey=0;}
            else if(Input.GetKey("s")){setvaluex=0; setvaluey=-1;}
            else if(Input.GetKey("d")){setvaluex=1; setvaluey=0;}

            if(Input.GetKey("up")){setvaluex = 0;setvaluey=1;}
            else if(Input.GetKey("left")){ setvaluex=-1; setvaluey=0;}
            else if(Input.GetKey("down")){setvaluex=0; setvaluey=-1;}
            else if(Input.GetKey("right")){setvaluex=1; setvaluey=0;}

            else if(Input.GetKey("x") && Input.GetKey("f"))
            {
                player.life = player.container =12;
                player.strenght = 50;
                player.bosskey = 10;
                player.key = 10;
                player.dps = 0.1f;
                player.coin = 9999;
                player.use_laser = player.activable_laser = true;
            }

            anim.SetFloat("velocityx",setvaluex);
            anim.SetFloat("velocityy",setvaluey); 

            Movement(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            this.GetComponent<CharacterShoot>().GetShooting();
    }


//FUNZIONE CHE AL TRIGGER DEGLI OBJECT ALLE ESTREMITA' E CAMBIA 
    //LA POSIZIONE DELLA TELECAMERA DETTATA DALLO SCRIPT CAMERAFOLLOWMOVEMENT
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Door"))
        {
            Transform cameraobj = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
            cameraobj.position = new Vector3(other.transform.position.x,other.transform.position.y,cameraobj.position.z);
        }
    }
//FUNZIONE CHE GESTISCE LE COLLISIONI
    void OnCollisionEnter2D(Collision2D col)
    {
       if(col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Door") || col.gameObject.CompareTag("Bullet")) 
       this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
