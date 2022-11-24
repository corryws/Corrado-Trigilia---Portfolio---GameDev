using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    EnemyIA enemyIA;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyIA = animator.GetComponent<EnemyIA>();
        if(enemyIA.enemy_id == 5)
        {
            int floor_id = enemyIA.floor.GetComponent<Floor>().floorid;
            if(floor_id == 4) animator.speed = 5f;
        }
           
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enemyIA.life >= 0) 
        {
            if(enemyIA.Whiteminioncount > 0)animator.speed = 0f; else animator.speed = 1f;
            enemyIA.SetAttackFunction(enemyIA.enemy_id);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if(enemyIA.life >= 0) enemyIA.ReSetAttack(); 
    }
}
