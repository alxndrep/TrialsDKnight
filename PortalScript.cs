using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private GameObject EfectoPortal;
    [SerializeField] private string SiguienteScene;
    void Start()
    {
        //boxCollider = GetComponent<BoxCollider2D>();
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            boxCollider.enabled = false;
            StartCoroutine(FadePlayer(0.5f, collision.gameObject));
            Instantiate(EfectoPortal, transform.position, Quaternion.identity);
            StartCoroutine(CambiarEscenario());
        }
    }*/
    
    IEnumerator FadePlayer(float segundos, GameObject player)
    {
        player.GetComponent<PlayerMovimiento>().sePuedeMover = false;
        yield return new WaitForSeconds(segundos);
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<PlayerMovimiento>().cuerpoFisico.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    IEnumerator CambiarEscenario()
    {
        yield return new WaitForSeconds(2.7f);
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(SiguienteScene);
        if (SceneManager.GetActiveScene().name.Equals("Tutorial")) Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LeanTween.move(collision.gameObject, transform.position, 0.5f);
            StartCoroutine(FadePlayer(0.5f, collision.gameObject));
            Instantiate(EfectoPortal, transform.position, Quaternion.identity);
            StartCoroutine(CambiarEscenario());
        }
    }
}
