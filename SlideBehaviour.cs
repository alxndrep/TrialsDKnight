using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PlayerMovimiento>().deslizando = true;
        PlayerMovimiento.Player.shadowTrail._Color = PlayerMovimiento.Player.originalColorShadowTrail;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovimiento.Player.shadowTrail.Sombras_Skill();
        PlayerMovimiento.Player.CrearEfectoDustPS();
        Collider2D[] objetos = Physics2D.OverlapCircleAll(PlayerCombateCuerpo.Player.controladorGolpe.position, PlayerCombateCuerpo.Player.radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                int direccion = PlayerCombateCuerpo.Player.transform.position.x > colisionador.transform.position.x ? -1 : 1;
                colisionador.GetComponent<EnemigoVidaHP>().ReboteDamage(direccion, 3);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<PlayerMovimiento>().deslizando = false;
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
