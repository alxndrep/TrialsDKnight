using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkullScript : MonoBehaviour
{
    [SerializeField] public float distancia;
    private Animator animador;
    private bool mirandoDerecha;
    private ParticleSystem particulas;
    private EnemigoVidaHP Enemigo;
    private bool isMuerto;

    [SerializeField] public float velocidadMoviento;
    [SerializeField] private GameObject efectoMuerte;
    private void Start()
    {
        isMuerto = false;
        animador = GetComponent<Animator>();
        mirandoDerecha = true;
        Enemigo = GetComponent<EnemigoVidaHP>();
        animador.SetBool("PuedeSeguir", true);
        particulas = GetComponent<ParticleSystem>();
        Instantiate(efectoMuerte, transform.position, Quaternion.identity);
    }
    private void Update()
    {
        distancia = Vector2.Distance(transform.position, PlayerCombateCuerpo.Player.transform.position);
        animador.SetFloat("Distancia", distancia);
        if (GetComponent<EnemigoVidaHP>().vidaHP <= 0) Muerte();
    }
    public void Girar(Vector3 objetivo)
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
    public void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }
    public void Muerte()
    {
        if (!isMuerto)
        {
            isMuerto = true;
            particulas.Stop();
            Enemigo.audioSRC.PlayOneShot(Enemigo.soundsFX[4]);
            Instantiate(efectoMuerte, transform.position, Quaternion.identity);
            StartCoroutine(DestroyAfterSeconds(Enemigo.soundsFX[4].length));
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Muerte();
        }
    }
    IEnumerator DestroyAfterSeconds(float seconds)
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
