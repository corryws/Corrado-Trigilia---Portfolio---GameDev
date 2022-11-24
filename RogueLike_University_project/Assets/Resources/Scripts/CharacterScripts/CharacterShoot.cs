using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour
{
    /*
    QUESTO SCRIPT MI PERMETTE DI GESTIRE LO SPARO 
    DEL GIOCATORE ALLA PREMUTA DEI TASTI
    DIREZIONALI
    */
    Character player;
    public Sprite BulletSprite;
    LineRenderer line;
    GameObject prefabbullet,bulletobj,bomb,smoke;

    void Awake()
    {
        prefabbullet = Resources.Load<GameObject>("Prefabs/GameplayPrefab/bullet");
        bomb = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Bomb");
        smoke = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Smoke");
        player = this.gameObject.GetComponent<Character>();
        line = this.gameObject.GetComponent<LineRenderer>();
    }


    public void GetShooting()
    {
        Instantiatebomb();
        ActivableKey();

        if(player.use_laser)Laser();
        else Instantiatebullet();
        if(player.use_mirino)Mirino();
        if(player.use_sigaro)Smoke();
    }

    void ActivableKey()
    {
        if(Input.GetKey(KeyCode.Space) 
           && GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>().GetActivableValue())
        {
            player.use_laser = player.activable_laser;
            player.use_mirino = player.activable_mirino;
            player.use_sigaro = player.activable_sigaro;
            GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>().ResetActivableLoadBar();
        }
    }

    void Instantiatebomb()
    {
        if(Input.GetKey("e") && player.bombs >= 1 && Time.time>player.nextshoot)
        {
            Vector2 direction = this.transform.position;
            Animator anim;
            anim = this.gameObject.GetComponent<Animator>();
            if(anim.GetFloat("velocityx") > 0)//destra
                direction = new Vector2(this.transform.position.x+0.7f,this.transform.position.y);
            else if(anim.GetFloat("velocityx") < 0)//sinistra
                direction = new Vector2(this.transform.position.x-0.7f,this.transform.position.y);
            else if(anim.GetFloat("velocityy") > 0)//sopra
                direction = new Vector2(this.transform.position.x,this.transform.position.y+0.7f);
            else if(anim.GetFloat("velocityy") < 0)//sotto
                direction = new Vector2(this.transform.position.x,this.transform.position.y-0.7f);
            
            player.bombs--;
            Instantiate(bomb,direction,bomb.transform.rotation);
            player.nextshoot = Time.time + 0.5f;
        }
    }

    void Instantiatebullet()
    { 
        DirectionInstatiate("up",0.12f,0.5f,player.shootspeed,new Vector3(0,0,-90f),new Vector2(0,player.shootspeed));
        DirectionInstatiate("down",0.12f,-0.5f,player.shootspeed,new Vector3(0,0,90f),new Vector2(0,-player.shootspeed));
        DirectionInstatiate("left",-0.5f,-0.07f,player.shootspeed,new Vector3(0,0,0f),new Vector2(-player.shootspeed,0));
        DirectionInstatiate("right",0.5f,-0.07f,player.shootspeed,new Vector3(0,180f,0),new Vector2(player.shootspeed,0));
    }

    void DirectionInstatiate(string key,float addx,float addy,float shootspeed,Vector3 rotate,Vector2 directionspeed)
    {
        if(Input.GetKey(key) && Time.time>player.nextshoot)
        {
            bulletobj = Instantiate(prefabbullet,new Vector3(this.transform.position.x+addx,this.transform.position.y+addy,-5f),Quaternion.identity);

            bulletobj.transform.GetChild(0).gameObject.SetActive(true);

            bulletobj.GetComponent<BulletMovement>().ChangeBulletSprite(BulletSprite);
            bulletobj.GetComponent<BulletMovement>().SpawnDirection(shootspeed,rotate,directionspeed);

            this.gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/CharacterSound/Shoot_Sound_Effect_1");
            this.gameObject.GetComponent<AudioSource>().Play();

            player.nextshoot = Time.time + player.dps;
        }
    }

    void Mirino()
    {
        //appare un mirino che fa vedere la traiettoria del proiettile
        Vector2 currentplayerpos = this.transform.position;
        line.enabled = true;
        line.SetColors(Color.yellow,Color.white);
        line.positionCount = 2;

        line.SetPosition(0,currentplayerpos);
        if(Input.GetKey("w"))         line.SetPosition(1,currentplayerpos+ new Vector2(0,10));
        else if(Input.GetKey("s"))  line.SetPosition(1,currentplayerpos+new Vector2(0,-10));
        else if(Input.GetKey("a"))  line.SetPosition(1,currentplayerpos+new Vector2(-10,0));
        else if(Input.GetKey("d")) line.SetPosition(1,currentplayerpos+new Vector2(10,0));

        if(Input.GetKey("up"))         line.SetPosition(1,currentplayerpos+ new Vector2(0,10));
        else if(Input.GetKey("down"))  line.SetPosition(1,currentplayerpos+new Vector2(0,-10));
        else if(Input.GetKey("left"))  line.SetPosition(1,currentplayerpos+new Vector2(-10,0));
        else if(Input.GetKey("right")) line.SetPosition(1,currentplayerpos+new Vector2(10,0));
    }

    void Laser()
    {
        //spari un laser
        Vector2 currentplayerpos = this.transform.position;
        line.SetColors(Color.white,Color.white);
        line.positionCount = 2;

        line.SetPosition(0,currentplayerpos);
        if(Time.time>player.nextshoot)
        {
            if(Input.GetKey("up")){ line.enabled = true; line.SetPosition(1,currentplayerpos+ new Vector2(0,10)); LaserRayCast(Vector3.up);}
            else if(Input.GetKey("down")){ line.enabled = true;  line.SetPosition(1,currentplayerpos+new Vector2(0,-10)); LaserRayCast(-Vector3.up);}
            else if(Input.GetKey("left")){ line.enabled = true;  line.SetPosition(1,currentplayerpos+new Vector2(-10,0)); LaserRayCast(Vector3.left);}
            else if(Input.GetKey("right")){ line.enabled = true; line.SetPosition(1,currentplayerpos+new Vector2(10,0)); LaserRayCast(-Vector3.left);}

             player.nextshoot = Time.time + 0.1f;
        }else line.enabled=false;
    }

    void LaserRayCast(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,direction);
        if(hit.collider.tag == "Enemy")
        {
            hit.collider.gameObject.GetComponent<EnemyIA>().CheckStatusLife(GameObject.FindWithTag("Player").GetComponent<Character>().strenght);
        }
    }

    void Smoke()
    {
        //spawna del fumo ogni volta che il player si muove,tale smoke despawna dopo tot secondi
        if(Time.time>player.nextshoot)
        {
            Instantiate(smoke,this.transform.position,Quaternion.identity);
            player.nextshoot = Time.time + 0.2f;
        }
        
    }


}
