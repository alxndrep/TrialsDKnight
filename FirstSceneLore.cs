using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneLore : MonoBehaviour
{
    public Animator animador;

    [SerializeField] public Transform ubicacionEddie;

    [SerializeField] public Animator eddieAnimador;
    [SerializeField] public DialogSystem dialogSystem;
    [SerializeField] private GameObject PortalComienzo;
    [SerializeField] private GameObject EfectoSuit;
    [SerializeField] private GameObject HealEffect;
    private bool flag;

    private void Update()
    {
        if((dialogSystem.index == 6 | dialogSystem.index == 16) & !flag)
        {
            flag = true;
            animador.SetTrigger("Magic");
        }

        if(dialogSystem.index == 7 & flag)
        {
            flag = false;
            animador.SetTrigger("BackIdle");
            Instantiate(HealEffect, ubicacionEddie.position, Quaternion.identity);
        }


        if(dialogSystem.index == 17 & flag)
        {
            flag = false;
            animador.SetTrigger("BackIdle");
            eddieAnimador.SetTrigger("SuitChange");
            Instantiate(EfectoSuit, ubicacionEddie.position, Quaternion.identity);
        }

        if(dialogSystem.index == 18 & !flag)
        {
            flag = true;
            animador.SetTrigger("Vanish");
            LeanTween.delayedCall(3f, InvokePortal);
        }
    }
    void InvokePortal()
    {
        Instantiate(PortalComienzo, ubicacionEddie.position, Quaternion.identity);
        eddieAnimador.SetTrigger("Portal");
        StartCoroutine(CambiarMainMenu());
    }

    private void Start()
    {
        animador = GetComponent<Animator>();
        PlayerMovimiento.Player.cuerpoFisico.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke("initScene", 2f);
    }

    public void initScene()
    {
        animador.SetTrigger("Appear");
        dialogSystem.initDialogue();
    }

    IEnumerator CambiarMainMenu()
    {
        yield return new WaitForSeconds(2f);
        dialogSystem.endDialogue();
        yield return new WaitForSeconds(1f);
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("Tutorial");
        if (SceneManager.GetActiveScene().name.Equals("Trial Begin")) Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
    }
}
