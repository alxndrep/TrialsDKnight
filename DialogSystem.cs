using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogSystem : MonoBehaviour
{
    [System.Serializable]
    public class Dialogo<TKey, TValue>
    {
        public Dialogo()
        {
        }

        public Dialogo(TKey key, TValue value)
        {
            Autor = key;
            Mensaje = value;
        }

        public TKey Autor;
        public TValue Mensaje;
    }

    [Header("Configuracion Scene")]
    [SerializeField] private bool iniciarScene;
    [SerializeField] [Range(0, 5)] private float delayComienzo;
    [SerializeField] private bool restringirMovimiento;

    [Header("Configuracion Dialogos")]
    [SerializeField] Dialogo<string, string>[] lineasDialogo;
    [SerializeField][Range(0,1)] private float dialogueSpeed;

    [Header("Componentes")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private TextMeshProUGUI authorComponent;
    [SerializeField] private TextMeshProUGUI TipText;
    private CanvasGroup canvasGroup;
    private AudioSource audioSRC;
    [HideInInspector] public int index;
    [HideInInspector] public bool dialogoIniciado;
    void Start()
    {
        dialogoIniciado = false;
        canvasGroup = GetComponent<CanvasGroup>();
        audioSRC = GetComponent<AudioSource>();
        if (restringirMovimiento)
        {
            PlayerMovimiento.Player.sePuedeMover = false;
            Input.ResetInputAxes();
            PlayerMovimiento.Player.animador.SetBool("EstaCorriendo", false);
        }
        else PlayerMovimiento.Player.sePuedeMover = true;
        if (iniciarScene) Invoke("initDialogue", delayComienzo);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PlayerMovimiento.Player.Controles["Use"]) & dialogoIniciado)
        {
            if (textComponent.text == lineasDialogo[index].Mensaje)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lineasDialogo[index].Mensaje;
                TipText.gameObject.SetActive(true);
                audioSRC.Stop();
            }
        }
    }
    public void initDialogue()
    {
        dialogoIniciado = true;
        textComponent.text = string.Empty;
        LeanTween.alphaCanvas(canvasGroup, 1, 1.5f).setOnComplete(StartDialogue);
    }
    void StartDialogue()
    {
        index = 0;
        authorComponent.text = lineasDialogo[index].Autor;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioSRC.Play();
        foreach (char c in lineasDialogo[index].Mensaje.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        audioSRC.Stop();
        yield return new WaitForSeconds(1f);
        
        TipText.gameObject.SetActive(true);
    }

    void NextLine()
    {
        if (index < lineasDialogo.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            TipText.gameObject.SetActive(false);
            authorComponent.text = lineasDialogo[index].Autor;
            StartCoroutine(TypeLine());
        }
        else endDialogue();
        
    }

    public void endDialogue()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, .5f);
        dialogoIniciado = false;
        if(restringirMovimiento) PlayerMovimiento.Player.sePuedeMover = true;
    }
}
