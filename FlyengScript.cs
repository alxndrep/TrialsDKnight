using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyengScript : MonoBehaviour
{
    [SerializeField] public float distancia;
    private Animator animador;
    private bool mirandoDerecha;

    [SerializeField] public float velocidadMoviento;
    private void Start()
    {
        animador = GetComponent<Animator>();
        mirandoDerecha = true;
    }
    private void Update()
    {
        distancia = Vector2.Distance(transform.position, PlayerCombateCuerpo.Player.transform.position);
        animador.SetFloat("Distancia", distancia);
        if (!PlayerCombateCuerpo.Player.estaInvulnerable)
        {
            animador.SetBool("PuedeSeguir", true);
        }
        else
        {
            animador.SetBool("PuedeSeguir", false);

        }

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


}
