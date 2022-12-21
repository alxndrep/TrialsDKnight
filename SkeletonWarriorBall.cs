using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarriorBall : MonoBehaviour
{
    [Header("Stats Criatura")]
    [SerializeField] public float velocidadMoviento;
    [SerializeField] public float distanciaGolpe;
    [HideInInspector] public float velocidadMovimientoOriginal;
    [HideInInspector] public bool mirandoDerecha = true;
    [HideInInspector] public int direccion = 1;
    [HideInInspector] public float distanciaJugador;

    [Header("Componentes")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private float radioGolpe;
    [SerializeField] private GameObject efectoMuerto;
    [SerializeField] private AudioClip[] attack_sfx;
    private bool enSuelo;
    private AudioSource audioSRC;
    [SerializeField] private AudioClip[] soundsFX;
    private Animator animador;
    private SpriteRenderer spriteR;
    [HideInInspector] public Rigidbody2D cuerpoFisico;
    [HideInInspector] public EnemigoVidaHP enemigoVidaHP;
    [SerializeField] public ShadowTrail shadowTrail;
    private void Start()
    {
        audioSRC = GetComponent<AudioSource>();
        animador = GetComponent<Animator>();
        cuerpoFisico = GetComponent<Rigidbody2D>();
        enemigoVidaHP = GetComponent<EnemigoVidaHP>();
        spriteR = GetComponent<SpriteRenderer>();
        velocidadMovimientoOriginal = velocidadMoviento;
    }

    private void Update()
    {
        if(PlayerCombateCuerpo.Player != null)
        {
            distanciaJugador = Vector2.Distance(transform.position, PlayerCombateCuerpo.Player.transform.position);
            animador.SetFloat("Distancia", distanciaJugador);
            if (!PlayerCombateCuerpo.Player.estaInvulnerable) animador.SetBool("PuedeSeguir", true);
            if (enemigoVidaHP.vidaHP <= 0) ProcesarMuerte();
        }

    }

    public void ProcesarMuerte()
    {
        animador.SetTrigger("Muerto");
    }
    public void DestruirObjeto()
    {
        Instantiate(efectoMuerto, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        direccion *= -1;
    }
    public void GirarObjetivo(Vector3 objetivo)
    {
        bool estaEnSuelo = PlayerCombateCuerpo.Player.jugador.enSuelo;
        if (estaEnSuelo)
        {
            if (transform.position.x < objetivo.x & !mirandoDerecha)
            {
                Girar();
            }
            else if (transform.position.x > objetivo.x & mirandoDerecha)
            {
                Girar();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioGolpe);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            enemigoVidaHP.golpeado = false;
        }
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
                    audioSRC.PlayOneShot(soundsFX[1]);
                    int direccion = transform.position.x < colisionador.transform.position.x ? 1 : -1;
                    colisionador.GetComponent<PlayerCombateCuerpo>().RecibirDamage(GetComponent<EnemigoVidaHP>().damage, direccion);
                }

            }
        }

    }
    public void PlayHit()
    {
        audioSRC.PlayOneShot(soundsFX[0]);
    }
    public void PlayAttackSound()
    {
        audioSRC.PlayOneShot(attack_sfx[(int)Random.Range(0, 2)]);
    }
    public void PlayDeathSound()
    {
        audioSRC.PlayOneShot(GetComponent<EnemigoVidaHP>().soundsFX[4]);
    }
}
