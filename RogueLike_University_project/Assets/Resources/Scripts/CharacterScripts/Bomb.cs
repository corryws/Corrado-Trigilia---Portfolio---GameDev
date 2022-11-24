using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float explosiontime;

    void Start(){StartCoroutine(Explosion());}

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;
        this.gameObject.GetComponent<Animator>().SetBool("explosion",true);
    }


    public void OnCollisionStay2D(Collision2D other)
    {
        if(this.gameObject.GetComponent<Animator>().GetBool("explosion"))
        {
                if(other.gameObject.CompareTag("Enemy"))
                if(other.gameObject.GetComponent<EnemyIA>().Whiteminioncount <= 0)
                    other.gameObject.GetComponent<EnemyIA>().CheckStatusLife(1);
            
            if(other.gameObject.CompareTag("Minion")){other.gameObject.transform.parent.GetComponent<EnemyIA>().Whiteminioncount-=1; Destroy(other.gameObject);}

            if(other.gameObject.CompareTag("Door")) Destroy(this.gameObject);
        } 
    }   

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(this.gameObject.GetComponent<Animator>().GetBool("explosion"))
         if(other.gameObject.CompareTag("Player"))other.gameObject.GetComponent<Character>().TakeDamage(0.5f);
    } 
}
