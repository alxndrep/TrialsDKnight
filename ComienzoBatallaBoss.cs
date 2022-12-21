using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComienzoBatallaBoss : MonoBehaviour
{
    [SerializeField] public ScriptBoss Boss;
    [SerializeField] private float TiempoDemora;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Boss.PlayBossMusic(TiempoDemora);
    }
}
