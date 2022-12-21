using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    private RawImage ReferenciaImagen;
    [SerializeField] private float MovimientoHorizontal, MovimientoVertical;

    private void Start()
    {
        ReferenciaImagen = GetComponent<RawImage>();
    }
    // Update is called once per frame
    void Update()
    {
        ReferenciaImagen.uvRect = new Rect(ReferenciaImagen.uvRect.position + new Vector2(MovimientoHorizontal, MovimientoVertical) * Time.deltaTime, ReferenciaImagen.uvRect.size);
    } 
}
