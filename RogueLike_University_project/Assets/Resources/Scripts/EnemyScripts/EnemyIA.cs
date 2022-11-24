using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public enum EnemyState{IDLE,ATTACK,FOLLOW,DIE,PAUSE};
    public EnemyState state;
    public EnemySpawn enemyspawn;Enemy enemy;UIGameplay hudview;Sprite BulletSprite;
    GameObject prefabbullet,bullet,item,movespotprefab,minionprefab,minion;
    public GameObject floor,movespot,player;
    

    public int enemy_id,life,maxlife;
    public float dropprate,speed,
                 shootspeed,dps,nextshoot,
                 attackrange,changestate_time;

    public int Whiteminioncount;
    public bool minionspawned,isboss,ispause;

void Start(){SetOtherVariable();SetEnemyParameter();}
void FixedUpdate(){EnemyCheckState();}
//IA CHECK STATE
    void EnemyCheckState()
    {
        switch(state)
        {
            case EnemyState.IDLE:
                Idle();
            break;
            case EnemyState.ATTACK:
                SetState("attack");
            break;
            case EnemyState.FOLLOW:
                SetState("follow");
            break;
            case EnemyState.DIE:
                this.gameObject.GetComponent<Animator>().speed = 4f;
                EnemyDie();
            break;
            case EnemyState.PAUSE:
                EnemyPause();
            break;
        }
    }

    void Idle()
    {
        if(enemy_id == 2 && this.gameObject.GetComponent<CircleCollider2D>() != null)
            this.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;

        int floor_id = floor.GetComponent<Floor>().floorid;
        if(enemy_id == 5 && floor_id == 4)//white boss
        {
            if(Whiteminioncount <= 0) {minionspawned = false; state = EnemyState.FOLLOW;} else state = EnemyState.ATTACK;

        }else{if(GetPlayerDistance()) state = EnemyState.ATTACK; else state = EnemyState.FOLLOW;}  
    }

public void EnemyPause(){if(ispause)state = EnemyState.PAUSE;else state = EnemyState.IDLE;}

//SET FUNCTION
void SetState(string trigger){this.gameObject.GetComponent<Animator>().SetTrigger(trigger);EnemyPause();}
bool GetPlayerDistance(){return Vector3.Distance(this.transform.position,GetPlayerPosition())<= attackrange || enemy_id == 3 || enemy_id == 4;}
public void ReSetAttack()
    {
        if(this.gameObject.GetComponent<CircleCollider2D>() != null)
            this.gameObject.GetComponent<CircleCollider2D>().radius = 0.1f;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(bullet);
    }
public Vector2 GetPlayerPosition(){return player.transform.position;} 
Vector2 GetPlayerDirection(){return (Vector2)(this.transform.position - player.transform.position);}

public IEnumerator PlaySound() 
 {
    this.gameObject.GetComponent<AudioSource>(). PlayDelayed(0.045f);
    yield return new WaitForSeconds(0.2f);
 }

IEnumerator BlinkAnimation()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

//IA ATTACCO
public void SetAttackFunction(int enemyid)
    {
        switch(enemyid)
        {
            case 0:
              StartCoroutine(PlaySound()); 
            break;
            case 1:
              ShootPlayerIA(GetPlayerPosition());
            break;
            case 2:
              JumpToPlayerIA();
            break;
            case 3:
              TowerMovementIA("Sprites/WeaponSprites/enemy_bullet_1");
            break;
            case 4:
              TriggerMovementIA();
            break;
            case 5:
              SetBossAttackFunction();
            break;
        }
    }

void SetBossAttackFunction()
    {
        int floor_id = floor.GetComponent<Floor>().floorid;
        if(floor_id == 0)RedBossAttack();
        else if(floor_id == 1)AcquaBossAttack();
        else if(floor_id == 2)GreenBossAttack(170,255,0);
        else if(floor_id == 3)OrangeBossAttack();
        else if(floor_id == 4)WhiteBossAttack();
        else if(floor_id == 5)VioletBossAttack();
        else return;
    }

void RedBossAttack()
    {
        /*gira su se stesso*/
        StartCoroutine(PlaySound()); 
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        if(this.gameObject.GetComponent<CircleCollider2D>())
         this.gameObject.GetComponent<CircleCollider2D>().radius = 0.4f;
    }

void AcquaBossAttack() 
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position,-Vector3.up);
        //spara un raggio laser
        if(bullet == null && hit.collider.tag == "Player")
        {
            StartCoroutine(PlaySound()); 
            bullet = Instantiate(prefabbullet,new Vector3(this.transform.position.x,this.transform.position.y-3f,-5f),Quaternion.identity);
            bullet.transform.GetChild(0).gameObject.SetActive(false);
            bullet.GetComponent<CircleCollider2D>().enabled = false;
            bullet.GetComponent<BoxCollider2D>().enabled = true;
            bullet.transform.localScale = new Vector2(bullet.transform.localScale.x,4f);
            SetEnemyBullet(bullet,"Sprites/WeaponSprites/Water_Cannon",true); 
        }
    }

void GreenBossAttack(int r,int g,int b)
    {
        //Spawna del fumo "smog" 
        if(Time.time > nextshoot)
        {
            StartCoroutine(PlaySound());  
            float xpos = Random.Range(-1f,1f); float ypos = Random.Range(-1f,1f);
            StartCoroutine(PredictGreenSmog(xpos,ypos,r,g,b));
            nextshoot = Time.time+dps;
        }
    }

 IEnumerator PredictGreenSmog(float xpos,float ypos,int r,int g,int b)
 {
    //Debug.Log("spawnerà qui! " + xpos + ypos);
    yield return new WaitForSeconds(0.5f);
    GameObject Smog = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Smoke");
    GameObject target = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Target");

    GameObject targetinstance = Instantiate(target,this.transform.position,Quaternion.identity);
    targetinstance.transform.SetParent(this.transform.parent,true);
    targetinstance.transform.localPosition = new Vector2(xpos,ypos);
    yield return new WaitForSeconds(1f);
    GameObject Smoginstance = Instantiate(Smog,this.transform.position,Quaternion.identity);
    Smoginstance.GetComponent<Smoke>().isenemysmoke = true;
    Smoginstance.transform.SetParent(this.transform.parent,true);
    Smoginstance.transform.localPosition = new Vector2(xpos,ypos);
    Smoginstance.transform.localScale  = new Vector2(2,2);
    Smoginstance.GetComponent<SpriteRenderer>().color = new Color32((byte)r,(byte)g,(byte)b,255);
    Destroy(targetinstance);
 }   
void OrangeBossAttack()
 {
     //se il giocatore si ci trova davanti esegue una carica
     RaycastHit2D hit = Physics2D.Raycast(this.transform.position,-Vector3.up);
        //spara un raggio laser
        if(hit.collider.tag == "Player")
        {
            Debug.Log("raycast");
            Vector3 directionplayer = this.transform.position - player.transform.position;
            this.transform.Translate(Vector3.up*directionplayer.y*-10*Time.deltaTime);
        }else{
            Vector2 target = GetPlayerPosition();
            if(Time.time > nextshoot)
            {
                Vector2 direction = GetPlayerDirection();
                int negativex=0; 
                if(direction.x < this.transform.position.x) 
                negativex=-1; else negativex=1;
                this.transform.localScale = new Vector2(this.transform.localScale.x*negativex,this.transform.localScale.y);

                bullet = Instantiate(prefabbullet,this.transform.position,Quaternion.identity);
                StartCoroutine(PlaySound());
                SetEnemyBullet(bullet,"Sprites/WeaponSprites/enemy_bullet_1",true);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * -shootspeed;
                nextshoot = Time.time+dps;
            } 
        }
 } 
void WhiteBossAttack()
    {
         //Spawna minion se ci sono minion è inattaccabile se non ci sono minion va in idle state
        if(minionspawned == false && Whiteminioncount != 0)
        {
            minionspawned = true;
            for(int i=0;i<Whiteminioncount;i++)
            {
                float xrng = Random.Range(-0.6f,0.5f),yrng = Random.Range(-0.6f,0.5f);
                Vector2 minionpos = new Vector2(xrng,yrng);
                minion = Instantiate(minionprefab,this.transform.position,Quaternion.identity);
                minion.transform.SetParent(this.transform,true);
                minion.transform.localPosition = minionpos;
            }
        }
    }

void VioletBossAttack()
 {
    //se l'attackrange è lontano allora spara verso il giocatore
    //se l'attackrange è vicino allora esegue  il pattern del boss verde
    if(life >= ((maxlife*50)/100))
    {
        Vector2 target = GetPlayerPosition();
        if(Time.time > nextshoot)
        {
            Vector2 direction = GetPlayerDirection();
            int negativex=0; 
            if(direction.x < this.transform.position.x) 
            negativex=-1; else negativex=1;
            this.transform.localScale = new Vector2(this.transform.localScale.x*negativex,this.transform.localScale.y);

            bullet = Instantiate(prefabbullet,this.transform.position,Quaternion.identity);
            StartCoroutine(PlaySound());
            SetEnemyBullet(bullet,"Sprites/WeaponSprites/enemy_bullet_2",true);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * -shootspeed;
            nextshoot = Time.time+dps;
        }   
    }else GreenBossAttack(100,25,255);
 }

void JumpToPlayerIA()
 {
     if(this.gameObject.GetComponent<CircleCollider2D>() != null) this.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
 }
void ShootPlayerIA(Vector2 target)
    {
        if(bullet == null)
        {
          if(Time.time > nextshoot)
            {
                Vector2 direction = GetPlayerDirection();
                int negativex=0; 
                if(direction.x < this.transform.position.x) 
                negativex=-1; else negativex=1;
                this.transform.localScale = new Vector2(this.transform.localScale.x*negativex,this.transform.localScale.y);

                bullet = Instantiate(prefabbullet,this.transform.position,Quaternion.identity);
                StartCoroutine(PlaySound());
                SetEnemyBullet(bullet,"Sprites/WeaponSprites/enemy_bullet_1",true);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * -shootspeed;
                nextshoot = Time.time+dps;
            }
        }
    }

public void TowerMovementIA(string spritebulletpath)
    {
        if(Time.time > nextshoot)
        {
            TowerSplitDirection(new Vector2(this.transform.position.x,
            this.transform.position.y+0.5f),new Vector3(0,0,-90f),new Vector2(0,shootspeed),true,spritebulletpath);//sopra

            TowerSplitDirection(new Vector2(this.transform.position.x,
            this.transform.position.y-0.5f),new Vector3(0,0,90f),new Vector2(0,-shootspeed),true,spritebulletpath);//sotto

            TowerSplitDirection(new Vector2(this.transform.position.x-0.5f,
            this.transform.position.y),new Vector3(0,0,0),new Vector2(-shootspeed,0),true,spritebulletpath);//sinistra

            TowerSplitDirection(new Vector2(this.transform.position.x+0.5f,
            this.transform.position.y),new Vector3(0,180f,0f),new Vector2(shootspeed,0),true,spritebulletpath);//destra

            nextshoot = Time.time + dps;
        }
    }

void TowerSplitDirection(Vector2 Spawnpos,Vector3 rotate,Vector2 directionspeed,bool enable,string spritebulletpath)
    {
        bullet = Instantiate(prefabbullet,Spawnpos,Quaternion.identity);
        StartCoroutine(PlaySound());
        SetEnemyBullet(bullet,spritebulletpath,enable);
        bullet.GetComponent<BulletMovement>().SpawnDirection(shootspeed,rotate,directionspeed);
    }

void TriggerMovementIA()
    {
        Vector3 directionplayer = this.transform.position - player.transform.position;
        //transform.Translate(Vector3.forward * Time.deltaTime);

        if((int)this.transform.position.y == (int)player.transform.localPosition.y)
            this.transform.Translate(Vector3.right*directionplayer.x*-speed*Time.deltaTime);
        else if((int)this.transform.position.x == (int)player.transform.localPosition.x)
            this.transform.Translate(Vector3.up*directionplayer.y*-speed*Time.deltaTime);

        if((int)this.transform.position.y == (int)player.transform.localPosition.y ||
           (int)this.transform.position.x == (int)player.transform.localPosition.x) StartCoroutine(PlaySound());
           
    }

//IA FOLLOW
public void FollowTarget(Vector2 CurrentTargetPos)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position,CurrentTargetPos,speed * Time.deltaTime);

        if(enemy_id == 0)StartCoroutine(PlaySound()); 
        
        if(enemy_id == 2 && this.gameObject.GetComponent<CircleCollider2D>() != null)
        {
            this.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            StartCoroutine(PlaySound()); 
        }
    }

public void BossMoveAround()
    {
        int floor_id = floor.GetComponent<Floor>().floorid;
        float xrng = Random.Range(-1f,1f),yrng = Random.Range(-1f,1f);
        this.transform.position = Vector3.MoveTowards(this.transform.position,movespot.transform.position,speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, movespot.transform.position) <= 0.1f)
          movespot.transform.localPosition = new Vector2(xrng,yrng);
          
    }
public void SetBossFollowFunction()
    {
        int floor_id = floor.GetComponent<Floor>().floorid;
        if(floor_id != 4)BossMoveAround();
        else return;
    }

public void WhiteBossFollow()
    {
        //esegue percentuale random 
        if(Random.Range(0,100) >= 30)Whiteminioncount=Random.Range(1,8);
    }
//SETTA LO STATO DIE SE LA VITA DEI NEMICI E' SOTTO LO 0'
    public void CheckStatusLife(int damage)
    {
        StartCoroutine(BlinkAnimation());
        if(life < damage) 
        {
            hudview.DamageHPBossBar(damage);
            hudview.EnableHPBossBar(0,0,false);
            enemyspawn.EnemyRemain-=1; 
            state = EnemyState.DIE;
        }
        else DamageFunction();
    }
    void DamageFunction()
    {
        life -= player.GetComponent<Character>().strenght;
        hudview.DamageHPBossBar(player.GetComponent<Character>().strenght);
    }
    
    public void EnemyDie()
    {
        GameObject[] redtargets = GameObject.FindGameObjectsWithTag("target");
        foreach(GameObject redtarget in redtargets) {if(redtarget!=null)Destroy(redtarget);}
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.gameObject.GetComponent<Animator>().SetBool("die",true);
        Destroy(this.gameObject.GetComponent<CircleCollider2D>());
    }
    public void DropItem()
    {
        if(Random.Range(0f,100f) <= dropprate)
        {
            if(isboss)
            {
                GameObject spawneditem = Instantiate(item,this.transform.parent.localPosition,Quaternion.identity);
                spawneditem.transform.position = new Vector2((int)this.transform.position.x,this.transform.position.y+0.8f);
                spawneditem.GetComponent<ItemSetting>().SetItem(Random.Range(1,3));//Random.Range(1,3)
            }else{
                GameObject spawneditem = Instantiate(item,this.transform.position,Quaternion.identity);
                spawneditem.transform.SetParent(this.transform.parent);
                spawneditem.GetComponent<ItemSetting>().SetItem(0);
            }
        }
        
    }
    public void IncrementEnemyScore()
    {
        if(enemy_id != 5) 
        {
            if(GameManager.instance != null && GameManager.gamemode == 1)GameManager.score_manager.IncrementScore(200);
            else if(GameManager.instance != null && GameManager.gamemode == 0)GameManager.score_manager.IncrementScore(100);

        }else 
        {
            if(GameManager.instance != null && GameManager.gamemode == 1)GameManager.score_manager.IncrementScore(300);  
            else if(GameManager.instance != null && GameManager.gamemode == 0)GameManager.score_manager.IncrementScore(200);  
        } 
    }
    public void DestroyEnemy(){Destroy(this.gameObject);}

//SET ALL ENEMY VARIABLE
    void SetOtherVariable()
    {
        prefabbullet = Resources.Load<GameObject>("Prefabs/GameplayPrefab/bullet");
        item         = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Item");
        movespotprefab     = Resources.Load<GameObject>("Prefabs/GameplayPrefab/EnemyMoveSpot");
        minionprefab = Resources.Load<GameObject>("Prefabs/GameplayPrefab/Minion");
        hudview      = GameObject.FindWithTag("UIManagement").GetComponent<UIGameplay>();
        floor        = GameObject.FindWithTag("Rooms");
        player       = GameObject.FindWithTag("Player");
    }

    void SetEnemyParameter()
    {
        if(isboss)enemy_id = 5;
        else enemy_id = Random.Range(0,5);
        if(enemy_id != 5) this.gameObject.GetComponent<CircleCollider2D>().radius = 0.15f;
        enemyspawn   = gameObject.transform.parent.GetComponent<EnemySpawn>();
        int floor_id = floor.GetComponent<Floor>().floorid;
        SetResourceEnemy("ScriptableObject/Enemy/floor_"+floor_id+"/enemy_"+ enemy_id);
    }

    void SetResourceEnemy(string enemy_path)
    {
        enemy = Resources.Load<Enemy>(enemy_path);//enemy_enemyid
        SetEnemyVariable();
    }
    
    void SetEnemyVariable()
    {
        life  = enemy.life;
        maxlife = life;
        speed = enemy.speed;
        shootspeed = enemy.shootspeed;
        dps = enemy.dps;
        nextshoot = enemy.nextshoot;
        dropprate = enemy.dropprate;
        attackrange = enemy.attackrange;
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController =  enemy.ThisEnemyAnimator;
        this.gameObject.GetComponent<AudioSource>().clip = enemy.verseclip;
        if(enemy_id == 2)this.transform.localScale = new Vector2(1.5f,2f);
        if(isboss) 
        {
            hudview.EnableHPBossBar(0,life,true);
            movespot = Instantiate(movespotprefab,this.transform.position,Quaternion.identity);
            movespot.transform.SetParent(this.transform.parent,true);
        }
    }

    void SetEnemyBullet(GameObject bullet,string pathbullet,bool enable)
    {
        BulletSprite = Resources.Load<Sprite>(pathbullet);
        bullet.GetComponent<BulletMovement>().ChangeBulletSprite(BulletSprite);
        bullet.GetComponent<BulletMovement>().enemybullet = enable;
    }

//SET COLLIDER
    void OnCollisionStay2D(Collision2D col)
    {   
       if(col.gameObject.CompareTag("Player") && !ispause)
        if(!col.gameObject.GetComponent<Character>().vulnerability)
        {
            if(GameManager.gamemode == 1)col.gameObject.GetComponent<Character>().TakeDamage(1f);
            else col.gameObject.GetComponent<Character>().TakeDamage(0.5f);
        }
          

         if(col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Door"))
          this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Smoke") && !other.gameObject.GetComponent<Smoke>().isenemysmoke && !ispause)
          if(Whiteminioncount <= 0)
           {
               CheckStatusLife(1);
               Destroy(other);
           }
    }
}
