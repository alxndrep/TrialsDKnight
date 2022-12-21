using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampasScript : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool esLetal;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!esLetal)
            {
                if (!PlayerCombateCuerpo.Player.denyingAttack)
                {
                    collision.gameObject.GetComponent<PlayerCombateCuerpo>().RecibirDamage(damage);
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<PlayerCombateCuerpo>().HP_Jugador > 0)
                {
                    collision.gameObject.GetComponent<PlayerCombateCuerpo>().RecibirDamageLetal(damage);
                }
            }

        }
        if (collision.collider.CompareTag("Enemigo"))
        {
            collision.gameObject.GetComponent<EnemigoVidaHP>().TomarDamage(damage*10);
        }
    }
}
