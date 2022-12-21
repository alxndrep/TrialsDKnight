using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarScript : MonoBehaviour
{


    [Header("Stats Criatura")]
    [SerializeField] public float velocidadMoviento;
    [SerializeField] public float distanciaPatrullaje;
    [HideInInspector] public Vector2 puntoInicio;
    private Vector2 puntoDireccion;
    [HideInInspector] public bool mirandoDerecha = true;
    [HideInInspector] public int direccion = 1;
    [HideInInspector] public float distanciaJugador;

    [Header("Componentes")]
    [SerializeField] private Vector3 dimensionesCajaSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Transform controladorSuelo2;
    [SerializeField] private Transform controladorPared;
    [SerializeField] private float distanciaReconocimientoPared;
    [SerializeField] private float distanciaReconocimientoSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] public ParticleSystem DustPS;
    private bool enSuelo;
    private AudioSource audioSRC;
    private Animator animador;
    private SpriteRenderer spriteR;
    [HideInInspector] public Rigidbody2D cuerpoFisico;
    [HideInInspector] public EnemigoVidaHP enemigoVidaHP;


    void Start()
    {
        audioSRC = GetComponent<AudioSource>();
        animador = GetComponent<Animator>();
        puntoInicio = transform.position;
        puntoDireccion = new Vector2(puntoInicio.x + distanciaPatrullaje, puntoInicio.y);
        cuerpoFisico = GetComponent<Rigidbody2D>();
        enemigoVidaHP = GetComponent<EnemigoVidaHP>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        distanciaJugador = Vector2.Distance(transform.position, PlayerCombateCuerpo.Player.transform.position);
        animador.SetFloat("Distancia", distanciaJugador);
        if (!PlayerCombateCuerpo.Player.estaInvulnerable) animador.SetBool("PuedeSeguir", true);
    }

    private void FixedUpdate()
    {

        if (!animador.GetBool("EstaSiguiendo") & !enemigoVidaHP.golpeado)
        {
            if (!enemigoVidaHP.golpeado) enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSuelo, 0f, capaSuelo);
            RaycastHit2D informacionPared = Physics2D.Raycast(controladorPared.position, Vector2.right * direccion, distanciaReconocimientoPared, capaSuelo);
            RaycastHit2D informacionSuelo = Physics2D.Raycast(controladorSuelo2.position, Vector2.down, distanciaReconocimientoSuelo, capaSuelo);
            cuerpoFisico.velocity = new Vector2(velocidadMoviento * direccion, cuerpoFisico.velocity.y);
            if (!enSuelo || informacionSuelo.collider == null) Girar();
            if (informacionPared.collider != null) Girar();
        }
    }
    public void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        direccion *= -1;
        CrearEfectoDustPS();
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

    public void CrearEfectoDustPS()
    {
        DustPS.Play();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSuelo);
        Gizmos.DrawLine(controladorSuelo2.transform.position, controladorSuelo2.transform.position + Vector3.down * distanciaReconocimientoSuelo);
        Gizmos.DrawLine(controladorPared.transform.position, controladorPared.transform.position + Vector3.right * direccion * distanciaReconocimientoPared);
    }
}

