using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startpos;
    [SerializeField] private GameObject Camera;
    [SerializeField] private float parallaxEffect;

    private void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void FixedUpdate()
    {
        float temp = (Camera.transform.position.x * (1 - parallaxEffect));
        float dist = (Camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
