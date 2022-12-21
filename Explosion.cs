using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float radio;
    [SerializeField] private float fuerzaExplosion;
    [SerializeField] private GameObject particulas;

    private void Start()
    {
        Instantiate(particulas, transform.position, Quaternion.identity);
        Invoke("ExplosionParticulas", 0.1f);
    }
    private void ExplosionParticulas()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, radio);
        foreach(Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("ParticulasExplosion"))
            {
                Rigidbody2D rb2D = colisionador.GetComponent<Rigidbody2D>();
                if(rb2D != null)
                {
                    Vector2 direccion = colisionador.transform.position - transform.position;
                    float distancia = 1 + direccion.magnitude;
                    float fuerzaFinal = fuerzaExplosion / distancia;
                    rb2D.AddForce(direccion * fuerzaFinal);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
