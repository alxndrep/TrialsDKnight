using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarraStats : MonoBehaviour
{
    public Slider slider;
    private TextMeshProUGUI textCant;
    public float velocidad;
    public void Start()
    {
        velocidad = 1.5f;
        slider = GetComponent<Slider>();
        textCant = transform.Find("CantidadText").GetComponent<TextMeshProUGUI>();
    }

    public void CambiarStatMaximo(float cantMaxima)
    {
        slider.maxValue = cantMaxima;
        textCant.text = (int)slider.value + "/" + (int)slider.maxValue;
    }
    public void CambiarStatActual(float cantValor)
    {
        slider.value = cantValor;
        textCant.text = (int)slider.value + "/" + (int)slider.maxValue;
    }
    public float GetValorActual()
    {
        return slider.value;
    }

    public void InicializarBarraDeStats(float cantValor)
    {
        slider = GetComponent<Slider>();
        textCant = transform.Find("CantidadText").GetComponent<TextMeshProUGUI>();
        CambiarStatMaximo(cantValor);
        CambiarStatActual(cantValor);
    }
    public void InicializarBarraDeStats(float valmax, float valactual)
    {
        slider = GetComponent<Slider>();
        textCant = transform.Find("CantidadText").GetComponent<TextMeshProUGUI>();
        CambiarStatMaximo(valmax);
        CambiarStatActual(valactual);
    }
}
