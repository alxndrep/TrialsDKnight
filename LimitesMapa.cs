using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitesMapa : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject portal;    
    IEnumerator TeleportPlayer(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerCombateCuerpo>().RecibirDamage(damage);
        Vector2 checkpoint = player.GetComponent<PlayerMovimiento>().ultimoCheckPoint;
        player.transform.position = new Vector2(checkpoint.x, checkpoint.y + 3f);
        Instantiate(portal, new Vector2(checkpoint.x, checkpoint.y + 5f), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(collision.gameObject));
        }
        else if (collision.CompareTag("Enemigo")) collision.gameObject.GetComponent<EnemigoVidaHP>().TomarDamage(99999);

    }
}
