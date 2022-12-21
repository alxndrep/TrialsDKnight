using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarSiguiendoBehaviour : StateMachineBehaviour
{
    [SerializeField] private float tiempoBase;
    [SerializeField] private float multiplicadorMovimiento;
    [SerializeField] AudioClip boar_attack;

    private float tiempoSeguir;
    private Transform Player;
    private BoarScript Boar;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Boar = animator.gameObject.GetComponent<BoarScript>();
        tiempoSeguir = tiempoBase;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator.SetBool("EstaSiguiendo", true);
        animator.gameObject.GetComponent<AudioSource>().PlayOneShot(boar_attack);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Boar.CrearEfectoDustPS();

        if (!Boar.enemigoVidaHP.golpeado)
        {
            if(!PlayerCombateCuerpo.Player.estaInvulnerable) Boar.GirarObjetivo(Player.position);
            Boar.cuerpoFisico.velocity = new Vector2(Boar.velocidadMoviento * multiplicadorMovimiento * Boar.direccion, Boar.cuerpoFisico.velocity.y);
            tiempoSeguir -= Time.deltaTime;
            if(tiempoSeguir <= 0)
            {
                animator.SetTrigger("Ataco");
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

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
