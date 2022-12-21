using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyengSiguiendoBehaviour : StateMachineBehaviour
{

    private float velocidadMovimiento;
    [SerializeField] private float tiempoBase;
    [SerializeField] private AudioClip attack_sound;

    private float tiempoSeguir;
    private Transform jugador;
    private FlyengScript Mob;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Mob = animator.gameObject.GetComponent<FlyengScript>();
        velocidadMovimiento = Mob.velocidadMoviento;
        tiempoSeguir = tiempoBase;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator.gameObject.GetComponent<AudioSource>().PlayOneShot(attack_sound);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, jugador.position, (velocidadMovimiento * 1.5f) * Time.deltaTime);
        Mob.Girar(jugador.position);
        tiempoSeguir -= Time.deltaTime;
        if(Mob.distancia <= 0)
        {
            animator.SetTrigger("Ataque");
        }
        if (tiempoSeguir <= 0 | PlayerCombateCuerpo.Player.estaInvulnerable)
        {
            animator.SetTrigger("Volver");
        }
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
