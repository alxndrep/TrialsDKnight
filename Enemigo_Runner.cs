using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo_Runner : MonoBehaviour
{
    [Header("Stats Criatura")]
    [SerializeField] public float velocidadMoviento;
    [SerializeField] public float fuerzaSalto;
    [HideInInspector] public bool mirandoDerecha = true;
    [HideInInspector] public int direccion = 1;
    [HideInInspector] public float distanciaJugador;
    [HideInInspector] public bool puedeSeguir;
    [HideInInspector] public bool estaMuerto;

    [Header("Componentes")]
    [SerializeField] private Vector3 dimensionesCajaSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Transform controladorSuelo2;
    [SerializeField] private Transform controladorPared;
    [SerializeField] private float distanciaReconocimientoPared;
    [SerializeField] private float distanciaReconocimientoSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] public ParticleSystem DustPS;

    private bool salto;
    private bool enSuelo;
    private Animator animador;
    [HideInInspector] public Rigidbody2D cuerpoFisico;
    [HideInInspector] public EnemigoVidaHP enemigoVidaHP;
    void Start()
    {
        animador = GetComponent<Animator>();
        cuerpoFisico = GetComponent<Rigidbody2D>();
        enemigoVidaHP = GetComponent<EnemigoVidaHP>();
        estaMuerto = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanciaJugador = Vector2.Distance(transform.position, PlayerCombateCuerpo.Player.transform.position);
        animador.SetFloat("Distancia", distanciaJugador);
        if (!PlayerCombateCuerpo.Player.estaInvulnerable) puedeSeguir = true;
        if(enemigoVidaHP.vidaHP <= 0 & !estaMuerto)
        {
            Muerte();
            animador.SetTrigger("Muerte");
        }
    }

    private void FixedUpdate()
    {

        if (!enemigoVidaHP.golpeado & !estaMuerto & puedeSeguir & animador.GetBool("EstaSiguiendo"))
        {
            cuerpoFisico.velocity = new Vector2(velocidadMoviento * direccion, cuerpoFisico.velocity.y);
            RaycastHit2D informacionPared = Physics2D.Raycast(controladorPared.position, Vector2.right * direccion, distanciaReconocimientoPared, capaSuelo);
            RaycastHit2D informacionSuelo = Physics2D.Raycast(controladorSuelo2.position, Vector2.down, distanciaReconocimientoSuelo, capaSuelo);
            enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSuelo, 0f, capaSuelo);
            if (enSuelo) salto = false;
            if (enSuelo & informacionSuelo.collider == null) Saltar();
            if (informacionPared.collider != null) Saltar();

        }
    }

    public void Saltar()
    {
        if (!salto)
        {
            salto = true;
            cuerpoFisico.velocity = new Vector2(velocidadMoviento * direccion, fuerzaSalto);
        }
    }
    public void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        direccion *= -1;
        DustPS.Play();
    }

    public void GirarObjetivo(Vector3 objetivo)
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

    public void Muerte()
    {
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), PlayerCombateCuerpo.Player.GetComponent<BoxCollider2D>());
        animador.SetBool("EstaMuerto", true);
        estaMuerto = true;
        enemigoVidaHP.audioSRC.PlayOneShot(enemigoVidaHP.soundsFX[4]);
        cuerpoFisico.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        LeanTween.alpha(gameObject, 0f, 2f).setOnComplete(DestroyGameObject);
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSuelo);
        Gizmos.DrawLine(controladorSuelo2.transform.position, controladorSuelo2.transform.position + Vector3.down * distanciaReconocimientoSuelo);
        Gizmos.DrawLine(controladorPared.transform.position, controladorPared.transform.position + Vector3.right * direccion * distanciaReconocimientoPared);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Piso"))
        {
            cuerpoFisico.velocity = Vector2.zero;
        }
    }
}
