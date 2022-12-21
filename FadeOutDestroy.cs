using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutDestroy : MonoBehaviour
{
    [SerializeField] private float velocidadFadeOut;
    private SpriteRenderer spriteR;
    private bool empezoEfecto;

    private void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        Invoke("EmpezarEfecto", 1.5f);
    }
    private void Update()
    {
        if (empezoEfecto)
        {
            spriteR.color = new Color(1f, 1f, 1f, spriteR.color.a - (Time.deltaTime * velocidadFadeOut));
            if (spriteR.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void EmpezarEfecto() { empezoEfecto = true; }

}
