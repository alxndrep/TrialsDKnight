using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EfectosCD : MonoBehaviour
{
    private Slider slider;
    private bool completoCD;
    [SerializeField] private TextMeshProUGUI texto;
    private float tiempo;

    private void Start()
    {
        slider = GetComponent<Slider>();
        tiempo = 1.2f;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (completoCD)
        {
            tiempo -= Time.deltaTime;
            if(tiempo <= 0)
            {
                tiempo = 0.8f;
                gameObject.SetActive(false);
            }
        }
    }
    public void CambiarCDMaximo(float cantMaxima)
    {
        slider.maxValue = cantMaxima;
    }
    public void CambiarCDActual(float cantValor)
    {
        slider.value = cantValor;
        texto.text = "" + cantValor.ToString("0.0");
        if (cantValor <= 0)
        {
            texto.gameObject.SetActive(false);
            completoCD = true;
        }
    }
    public void InicializarCD(float cantValor)
    {
        completoCD = false;
        texto.gameObject.SetActive(true);
        gameObject.SetActive(true);
        CambiarCDMaximo(cantValor);
        CambiarCDActual(cantValor);
    }

}
