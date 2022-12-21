using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiempoVidaSpell : MonoBehaviour
{
    [SerializeField] private float tiempoDeVida;
    void Start()
    {
        Invoke("DestruirSpell", tiempoDeVida);
    }

    private void DestruirSpell()
    {
        if (GetComponent<Spell>() != null)
        {
            GetComponent<Spell>().velocidad = 0;
            GetComponent<Animator>().SetTrigger("Choco");
        }else if (GetComponent<Self_Spell>() != null)
        {
            Destroy(gameObject);
        }
    }
}
