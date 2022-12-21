using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SiguienteEscenario : MonoBehaviour
{
    [SerializeField] private string SiguienteScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(SiguienteScene);
        }
    }
}
