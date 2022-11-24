using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIdleBrehaviour : StateMachineBehaviour
{
    EnemyIA enemyIA;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyIA = animator.GetComponent<EnemyIA>();
        enemyIA.gameObject.GetComponent<Animator>().SetBool("idle",true);
        animator.speed = 1f;
    }
}
