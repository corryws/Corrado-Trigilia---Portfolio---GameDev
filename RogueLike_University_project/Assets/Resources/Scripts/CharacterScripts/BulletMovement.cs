using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
  /*QUESTO SCRIPT E' ASSEGNATO ALL'OGGETTO BULLET 
  E PERMETTE AD ESSO DI ESEGUIRE DIVERSE AZIONI 
  TRA CUI MUOVERSI VERSO LE 4 DIREZIONI*/
    Character player;
    Vector2 getdirection;
    public bool enemybullet;

    void Awake(){player = GameObject.FindWithTag("Player").GetComponent<Character>();}
    void Start()
    {
      if(getdirection.y > 0)
          this.gameObject.transform.GetChild(0).transform.position = new Vector2(
          this.gameObject.transform.position.x,this.gameObject.transform.position.y-0.2f);
      else if(getdirection.y != 0)
          this.gameObject.transform.GetChild(0).transform.position = new Vector2(
          this.gameObject.transform.position.x,this.gameObject.transform.position.y+0.2f);
    }
    void Update(){SetDistanceFromShadow();Destroy(this.gameObject,2f);}

//FUNZIONE CHE PERMETTE IL MOVIMENTO DELLE 4 DIREZIONI + LA FUNZIONE DI SPAWN DATA DAL NEMICO CHE FARA' SEGUIRE IL PLAYER DAL BULLET
    public void ChangeBulletSprite(Sprite bulletsprite){this.gameObject.GetComponent<SpriteRenderer>().sprite = bulletsprite;}

    public void SpawnDirection(float bulletspeed,Vector3 Rotate,Vector2 directionspeed)
    {
      getdirection = directionspeed;
      this.transform.Rotate(Rotate); 
      this.gameObject.GetComponent<Rigidbody2D>().velocity = directionspeed;
    }

    public void SpawnFollowPlayer(float bulletspeed)
    {this.transform.position = Vector2.MoveTowards(this.transform.position,player.gameObject.transform.position,bulletspeed);}

    public void SetDistanceFromShadow()
    {
      if(getdirection.x != 0)
      {
          this.gameObject.transform.GetChild(0).transform.position = new Vector2(
          this.gameObject.transform.GetChild(0).transform.position.x,
          this.gameObject.transform.GetChild(0).transform.position.y + 0.002f
          );
      }else if(getdirection.y > 0)
      {
          this.gameObject.transform.GetChild(0).transform.position = new Vector2(
            this.gameObject.transform.GetChild(0).transform.position.x,
            this.gameObject.transform.GetChild(0).transform.position.y+0.002f
          );
      }else if(getdirection.y != 0)
      {
          this.gameObject.transform.GetChild(0).transform.position = new Vector2(
            this.gameObject.transform.GetChild(0).transform.position.x,
            this.gameObject.transform.GetChild(0).transform.position.y-0.002f
          );
      }

      if(this.gameObject.transform.GetChild(0).transform.localScale.x>1.5f &&
         this.gameObject.transform.GetChild(0).transform.localScale.y>1.2f)
         Destroy(this.gameObject);   
    }

//FUNZIONE CHE MI PERMETTE DI CAPIRE CON COSA COLLIDE IL BULLET
    //ESSENDO IL PLAYER E I NEMICI SPAWNANO LO STESSO BULLET ALLORA TRAMITE
    //UNA VARIABILE BOOLEANA CAPISCO SE SI TRATTA DI UN BULLET NEMICO O NO
    //NEL CASO COLLIDE COL GIOCATORE ED E' UN BULLET NEMICO, IL PLAYER SUBISCE DANNO
    //STESSA LOGICA COI NEMICI 
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(enemybullet  && !player.gameObject.GetComponent<Character>().vulnerability)
             {
               if(GameManager.gamemode == 1)player.gameObject.GetComponent<Character>().TakeDamage(1f);
               else player.gameObject.GetComponent<Character>().TakeDamage(0.5f);
               Destroy(this.gameObject);
             }
        }

        if(other.gameObject.CompareTag("Enemy"))
        {
           if(!enemybullet && other.gameObject.GetComponent<EnemyIA>().Whiteminioncount <= 0 && other.gameObject.GetComponent<CircleCollider2D>().radius != 0.4f)
           {
             other.gameObject.GetComponent<EnemyIA>().CheckStatusLife(GameObject.FindWithTag("Player").GetComponent<Character>().strenght);
             Destroy(this.gameObject);
           }
        }

        if(!enemybullet && other.gameObject.CompareTag("Minion"))
        {
           other.gameObject.transform.parent.GetComponent<EnemyIA>().Whiteminioncount-=1;
           Destroy(other.gameObject);
           Destroy(this.gameObject);
        }

        if(other.gameObject.CompareTag("Walls") || other.gameObject.CompareTag("Door")){Destroy(this.gameObject);}
        if(other.gameObject.name == "bulletshadowrange" && getdirection.x != 0) Destroy(this.gameObject);
    }
    

    void OnTriggerStay2D (Collider2D other)
    {
        if(other.gameObject.CompareTag("Door")) Destroy(this.gameObject);
    }

    void CallParticle()
    {
      this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
}
