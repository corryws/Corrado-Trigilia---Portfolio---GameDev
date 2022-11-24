using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
   public GameObject movespotprefab,prefabbullet,bullet;
   GameObject movespot;
   float nextshoot;

    // Start is called before the first frame update
    void Awake()
    {
        movespot = Instantiate(movespotprefab,this.transform.position,Quaternion.identity);
        movespot.transform.SetParent(this.transform,true);
    }

    // Update is called once per frame
    void FixedUpdate(){MoveAround();}
    
    void MoveAround()
    {
        float xrng = Random.Range(-0.5f,0.5f),yrng = Random.Range(-0.5f,0.5f);
        this.transform.position = Vector3.MoveTowards(this.transform.position,this.transform.GetChild(0).transform.position,1f * Time.deltaTime);
        if(Vector2.Distance(transform.position, movespot.transform.position) <= 5f)
          movespot.transform.localPosition = new Vector2(xrng,yrng);
        ShootPlayer();
    }


    void ShootPlayer()
    {
        if(bullet == null)
        {
          if(Time.time > nextshoot)
            {
                Vector2 direction = (Vector2)(this.transform.position - GameObject.FindWithTag("Player").transform.position);
                bullet = Instantiate(prefabbullet,this.transform.position,Quaternion.identity);
                SetBullet(bullet,"Sprites/WeaponSprites/enemy_bullet_1",true);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * -Random.Range(0.3f,0.7f);
                nextshoot = Time.time+2f;
            }
        }
    }

    void SetBullet(GameObject bullet,string pathbullet,bool enable)
    {
        Sprite BulletSprite = Resources.Load<Sprite>(pathbullet);
        bullet.GetComponent<BulletMovement>().ChangeBulletSprite(BulletSprite);
        bullet.GetComponent<BulletMovement>().enemybullet = enable;
    }

    void OnCollisionStay2D(Collision2D col)
    {   
       if(col.gameObject.CompareTag("Player"))
        if(!col.gameObject.GetComponent<Character>().vulnerability) 
          col.gameObject.GetComponent<Character>().TakeDamage(0.5f);

         if(col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Door"))
          this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
