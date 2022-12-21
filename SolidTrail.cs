using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidTrail : MonoBehaviour
{
    private SpriteRenderer mySprite;
    private Shader myMaterial;
    [SerializeField] public Color _Color;
    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myMaterial = Shader.Find("GUI/Text Shader");
    }

    void ColorSprite()
    {
        mySprite.material.shader = myMaterial;
        mySprite.color = _Color;
    }
    private void Update()
    {
        ColorSprite(); 
    }
    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
