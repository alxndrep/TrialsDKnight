using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarVolviendoBehaviour : StateMachineBehaviour
{
    private Vector2 puntoInicial;
    private BoarScript Boar;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Boar = animator.gameObject.GetComponent<BoarScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!Boar.enemigoVidaHP.golpeado)
        {
            Boar.cuerpoFisico.velocity = new Vector2(Boar.velocidadMoviento * Boar.direccion, Boar.cuerpoFisico.velocity.y);
            if(Boar.direccion == 1)
            {
                if (animator.transform.position.x >= puntoInicial.x)
                {
                    animator.SetTrigger("Llego");
                }
            }
            else
            {
                if (animator.transform.position.x <= puntoInicial.x)
                {
                    animator.SetTrigger("Llego");
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Volviendo", false);
        animator.SetBool("EstaSiguiendo", false);
        animator.SetBool("PuedeSeguir", false);
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
