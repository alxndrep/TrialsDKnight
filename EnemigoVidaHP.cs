using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemigoVidaHP : MonoBehaviour
{
    [Header("Custom Parameters")]
    [SerializeField] private bool muerteCustom;
    [SerializeField] private bool rigidBodyCustom;

    [Header("Propiedades")]
    [SerializeField] public float vidaHP;
    [SerializeField] private double experiencia;
    [SerializeField] public float damage;
    [SerializeField] private Vector2 velocidadRebote;
    [SerializeField] public GameObject efectoMuerte;
    [SerializeField] public AudioClip[] soundsFX;
    [SerializeField] private GameObject[] efectoGolpeSangre;
    [SerializeField] private GameObject efectoTextoDamage;

    [HideInInspector] public bool golpeando;
    [HideInInspector] public bool muerto;
    [HideInInspector] public AudioSource audioSRC;
    [HideInInspector] public Animator animador;
    [HideInInspector] public Rigidbody2D cuerpoFisico;
    [HideInInspector] public bool golpeado;
    void Start()
    {
        animador = GetComponent<Animator>();
        cuerpoFisico = GetComponent<Rigidbody2D>();
        audioSRC = GetComponent<AudioSource>();
    }

    public bool TomarDamage(float damage, float knockback, float direccion, bool critico)
    {
        if(vidaHP > 0)
        {
            vidaHP -= damage;
            animador.SetTrigger("Golpeado");
            if(soundsFX.Length > 1)
            {
                audioSRC.PlayOneShot(soundsFX[(int)Random.Range(1,4)]);
            }
            if (critico)
            {
                EfectoTextoDmg efecto = Instantiate(efectoTextoDamage, new Vector2(transform.position.x - 1f, transform.position.y), Quaternion.identity).GetComponent<EfectoTextoDmg>();
                efecto.Setup(damage*0.15f, new Color(1, 0.6589359f, 0, 1));
                EfectoTextoDmg efecto2 = Instantiate(efectoTextoDamage, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity).GetComponent<EfectoTextoDmg>();
                efecto2.Setup(damage*0.55f, new Color(1, 0.6589359f, 0, 1));
                EfectoTextoDmg efecto3 = Instantiate(efectoTextoDamage, new Vector2(transform.position.x + 1f, transform.position.y), Quaternion.identity).GetComponent<EfectoTextoDmg>();
                efecto3.Setup(damage*0.3f, new Color(1, 0.6589359f, 0, 1));
            }
            else
            {
                EfectoTextoDmg efecto = Instantiate(efectoTextoDamage, transform.position, Quaternion.identity).GetComponent<EfectoTextoDmg>();
                efecto.Setup(damage);

            }
            Instantiate(efectoGolpeSangre[Random.Range(0, efectoGolpeSangre.Length)], transform.position, Quaternion.identity);
            if (vidaHP <= 0)
            {
                muerto = true;
                PlayerStats.Stats.AgregarExperiencia(experiencia);
                if(!muerteCustom) Muerte();
                return true;
            }
            else
            {
                if(!rigidBodyCustom) ReboteDamage(direccion, knockback);
                return false;
            }
        }
        return true;
    }
    public void TomarDamage(float damage)
    {
        if(vidaHP > 0)
        {
            vidaHP -= damage;
            animador.SetTrigger("Golpeado");
            if (soundsFX.Length > 1)
            {
                audioSRC.PlayOneShot(soundsFX[(int)Random.Range(1, 4)]);
            }
            Instantiate(efectoGolpeSangre[Random.Range(0, efectoGolpeSangre.Length)], transform.position, Quaternion.identity);
            if (vidaHP <= 0)
            {
                muerto = true;
                if (!muerteCustom) Muerte();
            }
        }
    }
    public void ReboteDamage(float direccion, float knockback)
    {
        golpeado = true;
        cuerpoFisico.velocity = Vector2.zero;
        cuerpoFisico.velocity = new Vector2((velocidadRebote.x + knockback)* direccion, velocidadRebote.y);
    }
    private void Muerte()
    {
        if (soundsFX.Length > 1)
        {
            audioSRC.Stop();
            audioSRC.PlayOneShot(soundsFX[4]);
            StartCoroutine(DestroyAfterSeconds(soundsFX[4].length));
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(efectoMuerte, transform.position, Quaternion.identity);
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Instantiate(efectoMuerte, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") & !muerto)
        {
            if (!PlayerCombateCuerpo.Player.denyingAttack)
            {
                animador.SetTrigger("Ataque");
                audioSRC.PlayOneShot(soundsFX[0]);
                collision.gameObject.GetComponent<PlayerCombateCuerpo>().RecibirDamage(damage, collision.GetContact(0).normal);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            golpeado = false;
        }
    }
    public void DesactivarColisionador()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
    public void StopAllClips()
    {
        audioSRC.Stop();
    }

}
