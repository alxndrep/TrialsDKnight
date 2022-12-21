using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWBSiguiendoBehaviour : StateMachineBehaviour
{
    [SerializeField] private float tiempoBase;
    [SerializeField] private float multiplicadorMovimiento;

    private float tiempoSeguir;
    private Transform Player;
    private SkeletonWarriorBall Skeleton;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Skeleton = animator.gameObject.GetComponent<SkeletonWarriorBall>();
        tiempoSeguir = tiempoBase;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator.SetBool("EstaSiguiendo", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!Skeleton.enemigoVidaHP.golpeado)
        {
            if (!PlayerCombateCuerpo.Player.estaInvulnerable)
            {
                Skeleton.GirarObjetivo(Player.position);
            }

            Skeleton.cuerpoFisico.velocity = new Vector2(Skeleton.velocidadMoviento * multiplicadorMovimiento * Skeleton.direccion, Skeleton.cuerpoFisico.velocity.y);
            tiempoSeguir -= Time.deltaTime;
            if(animator.GetFloat("Distancia") <= Skeleton.distanciaGolpe & !PlayerCombateCuerpo.Player.estaInvulnerable)
            {
                animator.SetTrigger("Ataque");
            }
            if (tiempoSeguir <= 0)
            {
                animator.SetTrigger("Volver");
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<SkeletonWarriorBall>().cuerpoFisico.velocity = Vector2.zero;
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
