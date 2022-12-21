using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDialogueSystem : MonoBehaviour
{
    private Animator animador;
    void Start()
    {
        animador = GetComponent<Animator>();
        animador.SetTrigger("Appear");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
