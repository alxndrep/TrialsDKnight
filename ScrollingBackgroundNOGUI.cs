using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroundNOGUI : MonoBehaviour
{
    [SerializeField] public float velocidadScroll;
    [SerializeField] private Renderer bgRenderer;

    private void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(0, velocidadScroll * Time.deltaTime);
    }
}
