using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoEstatico : MonoBehaviour
{

    [Header("Stats Criatura")]
    [SerializeField] public float distanciaGolpe;
    [HideInInspector] public bool mirandoDerecha = true;
     public float distanciaJugador;

    [Header("Componentes")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private ShadowTrail shadowTrail;
    [SerializeField] private float radioGolpe;
    [SerializeField] private AudioClip AttackClip;
    [HideInInspector] public Animator animador;
    private EnemigoVidaHP Enemigo;
    private float direccionJugador;
    private AudioSource audioSRC;

    // Start is called before the first frame update
    void Start()
    {
        controladorAtaque = transform.Find("ControladorAtaque").transform;
        audioSRC = GetComponent<AudioSource>();
        animador = GetComponent<Animator>();
        Enemigo = GetComponent<EnemigoVidaHP>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") & Enemigo.vidaHP > 0)
        {
            distanciaJugador = Vector2.Distance(transform.position, PlayerCombateCuerpo.Player.transform.position);
            animador.SetBool("PuedeAtacar", PlayerCombateCuerpo.Player.estaInvulnerable ? false:true);
            direccionJugador = PlayerCombateCuerpo.Player.transform.position.x;
            animador.SetFloat("Distancia", distanciaJugador);
            if (direccionJugador > transform.position.x & !mirandoDerecha) Girar();
            if (direccionJugador < transform.position.x & mirandoDerecha) Girar();
        }

    }

    public void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioGolpe);
    }

    public void RealizarDamage()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Player"))
            {
                if (!PlayerCombateCuerpo.Player.estaInvulnerable & !PlayerCombateCuerpo.Player.denyingAttack)
                {
                    audioSRC.PlayOneShot(AttackClip);
                    audioSRC.PlayOneShot(Enemigo.soundsFX[0]);
                    int direccion = transform.position.x < colisionador.transform.position.x ? 1 : -1;
                    colisionador.GetComponent<PlayerCombateCuerpo>().RecibirDamage(GetComponent<EnemigoVidaHP>().damage, direccion);
                }

            }
        }
    }
}
