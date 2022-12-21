using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBoss : MonoBehaviour
{
    [SerializeField] public AudioClip BossMusic;
    private AudioClip StageMusic;
    private AudioSource audioSRC;
    public GameObject CamaraPersonaje;
    [SerializeField] private GameObject[] RejasBoss;
    [SerializeField] public BarraStats barraHP;
    [SerializeField] public Item.Diccionario<GameObject, float>[] Loot;
    [HideInInspector] private bool spawnedLoot;
    private EnemigoVidaHP Enemigo;
    public bool isFighting;
    public bool yaEntro;
    
    public void Start()
    {
        spawnedLoot = false;
        isFighting = false;
        yaEntro = false;
        audioSRC = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        CamaraPersonaje = GameObject.Find("Cinemachine");
        Enemigo = GetComponent<EnemigoVidaHP>();
        barraHP = GameObject.Find("HPBoss").GetComponent<BarraStats>();
        barraHP.InicializarBarraDeStats(Enemigo.vidaHP);
    }
    public void Update()
    {
        if (isFighting)
        {
            barraHP.CambiarStatActual(Enemigo.vidaHP);
        }
        if(Enemigo.vidaHP <= 0 & !spawnedLoot)
        {
            for(int i=0; i < Loot.Length; i++)
            {
                float rnd = Random.Range(0, 101);
                if(rnd <= Loot[i].Value)
                {
                    Instantiate(Loot[i].Key, transform.position, Quaternion.identity);
                }
            }
            spawnedLoot = true;
        }
    }
    IEnumerator PlayCoroutineBossMusic(float tiempo)
    {
        SetRejasStatus(true);
        yield return new WaitForSeconds(3f);
        StageMusic = audioSRC.clip;
        audioSRC.Stop();
        audioSRC.clip = BossMusic;
        audioSRC.Play();
        isFighting = true;
        CamaraPersonaje.SetActive(false);
        CinemachineMovCamera.Instance.ChangeCamera("CameraBoss");
        
        barraHP.GetComponent<CanvasGroup>().alpha = 1;

        //CinemachineVirtualCamera vcam = GameObject.Find("Cinemachine").gameObject.GetComponent<CinemachineVirtualCamera>();
        //vcam.m_Follow = GameObject.Find("CamaraBoss").gameObject.transform;
        //vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 1f;
        //vcam.GetCinemachineComponent<CinemachineFramingTransposer>().enabled = false;
        //vcam.m_Lens.OrthographicSize = 15;
    }
    public void PlayBossMusic(float tiempoDemora)
    {
        if (!yaEntro)
        {
            yaEntro = true;
            StartCoroutine(PlayCoroutineBossMusic(tiempoDemora));
        }
    }
    public void SetRejasStatus(bool status)
    {
        foreach(GameObject reja in RejasBoss)
        {
            reja.SetActive(status);
        }
    }
    public void PlayStageMusic()
    {
        audioSRC.Stop();
        audioSRC.clip = StageMusic;
        audioSRC.Play();
    }
}
