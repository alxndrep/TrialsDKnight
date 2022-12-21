using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EfectoTextoDmg : MonoBehaviour
{
    [SerializeField] Vector2 movimiento;
    [SerializeField] TextMeshPro texto;
    [HideInInspector] public float dmg;
    public void Setup(float dmg)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(movimiento.x, movimiento.y));
        texto.text = "" + (int)dmg;
    }
    public void Setup(float dmg, Color color)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(movimiento.x, movimiento.y));
        texto.enableVertexGradient = true;
        texto.colorGradient = new VertexGradient(color);
        texto.text = "" + (int)dmg;
    }

}
