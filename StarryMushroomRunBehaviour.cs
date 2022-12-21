using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarryMushroomRunBehaviour : StateMachineBehaviour
{
    [SerializeField] private float segundosCorriendo;
    private float timerCorriendo;
    private Transform Player;
    private Enemigo_Runner enemigoRunner;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemigoRunner = animator.GetComponent<Enemigo_Runner>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timerCorriendo = segundosCorriendo;
        animator.SetBool("EstaSiguiendo", true);
        enemigoRunner.GirarObjetivo(Player.position);
        enemigoRunner.enemigoVidaHP.audioSRC.PlayOneShot(enemigoRunner.enemigoVidaHP.soundsFX[2]);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemigoRunner.DustPS.Play();
        timerCorriendo -= Time.deltaTime;
        if(timerCorriendo <= 0)
        {
            animator.SetTrigger("Muerte");
            enemigoRunner.Muerte();
            animator.SetBool("EstaSiguiendo", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("EstaSiguiendo", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
