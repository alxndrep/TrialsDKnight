using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkullBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private float velocidadMovimiento;
    [SerializeField] private AudioClip attack_sound;
    [SerializeField] private float DistanciaMax;

    private Transform jugador;
    private FireSkullScript Mob;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Mob = animator.gameObject.GetComponent<FireSkullScript>();
        velocidadMovimiento = Mob.velocidadMoviento;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator.gameObject.GetComponent<AudioSource>().PlayOneShot(attack_sound);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Vector2.Distance(animator.transform.position, jugador.position) > DistanciaMax)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, jugador.position, velocidadMovimiento * Time.deltaTime);
            Mob.Girar(jugador.position);
        }
;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
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
