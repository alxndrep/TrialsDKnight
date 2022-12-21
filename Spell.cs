using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public float velocidad;
    [SerializeField] private AudioClip explodeSFX;
    [SerializeField] public float costoSoulMana;
    [SerializeField] public float enfriamientoCD;
    [SerializeField] public Sprite spriteSpell;
    private AudioSource audioSRC;
    private Animator animador;

    private void Start()
    {
        animador = GetComponent<Animator>();
        audioSRC = GetComponent<AudioSource>();
    }
    private void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            int direccion = PlayerMovimiento.Player.transform.position.x > collision.transform.position.x ? -1 : 1;
            collision.gameObject.GetComponent<EnemigoVidaHP>().TomarDamage(damage + PlayerStats.Stats.inteligenciaMOD, 2, direccion, false);
            CinemachineMovCamera.Instance.MoverCamera(6, 6, 0.2f);
            animador.SetTrigger("Choco");
            velocidad = 0;
        }

        if (collision.gameObject.CompareTag("Piso"))
        {
            animador.SetTrigger("Choco");
            velocidad = 0;
        }
    }

    public void PlayExplodeSound()
    {
        audioSRC.PlayOneShot(explodeSFX);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
