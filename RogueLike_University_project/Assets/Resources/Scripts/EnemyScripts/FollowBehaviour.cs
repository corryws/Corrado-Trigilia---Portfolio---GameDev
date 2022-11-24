using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{
    EnemyIA enemyIA;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyIA = animator.GetComponent<EnemyIA>();
        animator.speed = 1f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enemyIA.enemy_id != 5)enemyIA.FollowTarget(enemyIA.GetPlayerPosition());
        else 
        {
            int floor_id = enemyIA.floor.GetComponent<Floor>().floorid;
            if(floor_id == 5)
            {
                if(enemyIA.gameObject.GetComponent<CircleCollider2D>() != null)
                enemyIA.GetComponent<CircleCollider2D>().isTrigger = true;
            }
            enemyIA.SetBossFollowFunction();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enemyIA.enemy_id == 5)
        {
            int floor_id = enemyIA.floor.GetComponent<Floor>().floorid;
            if(floor_id == 1)enemyIA.TowerMovementIA("Sprites/WeaponSprites/Water_Wave_0");
            if(floor_id == 4)enemyIA.WhiteBossFollow();
            if(floor_id == 5 && enemyIA.GetComponent<CircleCollider2D>() != null)enemyIA.GetComponent<CircleCollider2D>().isTrigger = false;
        }
       
    }

}
