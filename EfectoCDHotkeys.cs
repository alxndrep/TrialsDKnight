using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EfectoCDHotkeys : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI texto;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void CambiarCDMaximo(float cantMaxima)
    {
        slider.maxValue = cantMaxima;
    }
    public void CambiarCDActual(float cantValor)
    {
        texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 1f);
        slider.value = cantValor;
        texto.text = "" + cantValor.ToString("0.0");
        if (slider.value < 0.05f)
        {
            texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 0f);
        }
    }
    public void InicializarCD(float cantValor)
    {
        texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 0f);
        CambiarCDMaximo(cantValor);
        CambiarCDActual(cantValor);
    }
}
