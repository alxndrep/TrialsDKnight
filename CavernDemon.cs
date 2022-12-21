using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernDemon : MonoBehaviour
{
    private EnemigoVidaHP Enemigo;
    private float EnemigoMaxHP;
    private ScriptBoss Boss;
    private ParticleSystem particulas;
    [SerializeField] private ParticleSystem ParticulasAtaque;
    [SerializeField] private Transform[] posicionesAtaque;
    [SerializeField] private Transform[] posicionsSpawn;
    [SerializeField] private float TiempoMovimiento;
    [SerializeField] private float DuracionAtaque;
    [SerializeField] private float TiempoEsperaAtaque;
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private AudioClip[] soundsFX;
    [SerializeField] private AudioSource FireBreathAudioSRC;
    [SerializeField] private AudioSource WingsAudioSRC;
    [SerializeField] private Transform ControladorAtaque;
    [SerializeField] private GameObject FireSkull;
    private float tiempoRestanteMovimiento;
    private float tiempoRestanteDuracionAtaque;
    private float tiempoRestanteEsperaAtaque;
    private bool mirandoDerecha = false;
    private bool atacando = false;
    private bool sePuedeMover = false;
    private int posRandom;
    private int ultimaPosicion;
    private Animator animador;
    private bool ResumirMusica;
    private bool SegundaFase;
    private bool comenzoSegundaFase;
    private float tiempoCambioFase;
    private bool invocoSkulls;

    private void Start()
    {
        Enemigo = GetComponent<EnemigoVidaHP>();
        particulas = GetComponent<ParticleSystem>();
        animador = GetComponent<Animator>();
        Boss = GetComponent<ScriptBoss>();
        tiempoRestanteMovimiento = TiempoMovimiento;
        tiempoRestanteDuracionAtaque = DuracionAtaque;
        tiempoRestanteEsperaAtaque = TiempoEsperaAtaque;
        posRandom = (int)Random.Range(0, posicionesAtaque.Length);
        ultimaPosicion = posRandom;
        ParticulasAtaque.Stop();
        EnemigoMaxHP = Enemigo.vidaHP;
        invocoSkulls = false;
    }
    void Update()
    {
        Enemigo.cuerpoFisico.velocity = Vector2.zero;        
        if (Enemigo.muerto)
        {
            particulas.Stop();
            ParticulasAtaque.Stop();
            ParticulasAtaque.gameObject.SetActive(false);
            WingsAudioSRC.Stop();
            if (!ResumirMusica)
            {
                Boss.barraHP.GetComponent<CanvasGroup>().alpha = 0;
                Boss.PlayStageMusic();
                Boss.CamaraPersonaje.SetActive(true);
                CinemachineMovCamera.Instance.ChangeCamera("Cinemachine");
                Boss.SetRejasStatus(false);
                ResumirMusica = true;
                GameObject.Find("EventSystem").GetComponent<AudioSource>().pitch = 1f;
                GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = 0.5f;

            }
        }
        else
        {
            if (Enemigo.vidaHP <= EnemigoMaxHP * 0.33f & !SegundaFase)
            {
                SegundaFase = true;
                animador.SetTrigger("SegundaFase");
                Boss.isFighting = false;
                tiempoCambioFase = 3f;
            }
            if (SegundaFase & !comenzoSegundaFase)
            {
                tiempoCambioFase -= Time.deltaTime;
                if (tiempoCambioFase <= 0)
                {
                    Boss.isFighting = true;
                    comenzoSegundaFase = true;
                    GetComponent<CapsuleCollider2D>().enabled = true;
                }
            }
            if (Boss.isFighting)
            {

                if (!atacando)
                {
                    transform.position = Vector2.MoveTowards(transform.position, posicionesAtaque[posRandom].position, (velocidadMovimiento) * Time.deltaTime);
                    Girar(posicionesAtaque[posRandom].position);
                    if (Vector2.Distance(transform.position, posicionesAtaque[posRandom].position) == 0)
                    {
                        tiempoRestanteEsperaAtaque -= Time.deltaTime;
                        Girar(PlayerMovimiento.Player.transform.position);

                        if (tiempoRestanteEsperaAtaque <= 0)
                        {
                            atacando = true;
                            animador.SetTrigger("Firebreath");
                            animador.SetBool("Atacando", true);
                            float critChance = Random.Range(0, 101);
                            PlayFireBreath();
                            tiempoRestanteEsperaAtaque = TiempoEsperaAtaque;
                            if (critChance <= 35f)
                            {
                                invocoSkulls = true;
                            }
                            else
                            {
                                invocoSkulls = false;
                            }
                            while (true)
                            {
                                posRandom = Random.Range(0, posicionesAtaque.Length);
                                if (posRandom != ultimaPosicion)
                                {
                                    ultimaPosicion = posRandom;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    tiempoRestanteDuracionAtaque -= Time.deltaTime;
                    if(tiempoRestanteDuracionAtaque <= 0.5f) ParticulasAtaque.Stop();
                    if (tiempoRestanteDuracionAtaque <= 0 & !sePuedeMover)
                    {
                        animador.SetTrigger("Volver");
                        animador.SetBool("Atacando", false);
                        sePuedeMover = true;
                        FireBreathAudioSRC.Stop();
                    }
                    if (sePuedeMover & !ParticulasAtaque.IsAlive())
                    {
                        tiempoRestanteMovimiento -= Time.deltaTime;
                        if (tiempoRestanteMovimiento <= 0)
                        {
                            sePuedeMover = false;
                            atacando = false;
                            tiempoRestanteMovimiento = TiempoMovimiento;
                            tiempoRestanteDuracionAtaque = DuracionAtaque;
                        }
                    }
                }
            }
        }

    }

    public void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }
    public void Girar(Vector3 objetivo)
    {
        if (transform.position.x < objetivo.x & !mirandoDerecha)
        {
            Girar();
        }
        else if (transform.position.x > objetivo.x & mirandoDerecha)
        {
            Girar();
        }
    }

    public void AtaqueParticulasPlay()
    {
        if(SegundaFase)
        {
            var offset = 135f;
            Vector2 direction = PlayerCombateCuerpo.Player.transform.position - ControladorAtaque.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ControladorAtaque.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        if(!invocoSkulls) ParticulasAtaque.Play();
        else
        {
            Instantiate(FireSkull, posicionsSpawn[0].position, Quaternion.identity);
            Instantiate(FireSkull, posicionsSpawn[1].position, Quaternion.identity);
            if (comenzoSegundaFase)
            {
                Instantiate(FireSkull, posicionsSpawn[2].position, Quaternion.identity);
                Instantiate(FireSkull, posicionsSpawn[3].position, Quaternion.identity);
            }
        }
    }
    public void PlayFireBreath()
    {
        FireBreathAudioSRC.Play();
    }
    public void StopFireBreath()
    {
        FireBreathAudioSRC.Stop();
    }
    public void PlayWings()
    {
        WingsAudioSRC.clip = soundsFX[(int)Random.Range(0, 2)];
        WingsAudioSRC.Play();
    }
    public void StopWings()
    {
        WingsAudioSRC.Stop();
    }
    public void ComenzarSegundaFase()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = new Color(0, 128f / 255f, 161f / 255f, 145f / 255f);
        main = ControladorAtaque.GetComponent<ParticleSystem>().main;
        main.startColor = new Color(0, 128f / 255f, 161f / 255f, 145f / 255f);
        var trail = ControladorAtaque.GetComponent<ParticleSystem>().trails;
        trail.colorOverTrail = new Color(0, 255f / 255f, 218f / 255f, 255f / 255f);
        StopFireBreath();
        WingsAudioSRC.PlayOneShot(soundsFX[2]);
        CinemachineMovCamera.Instance.MoverCamera(25, 25, 2.5f);
        GetComponent<CapsuleCollider2D>().enabled = false;
        ParticulasAtaque.Stop();
        sePuedeMover = false;
        atacando = false;
        tiempoRestanteMovimiento = TiempoMovimiento;
        animador.SetBool("Atacando", false);
        Boss.barraHP.CambiarStatActual(Enemigo.vidaHP);
        tiempoRestanteDuracionAtaque = DuracionAtaque;
        velocidadMovimiento *= 1.5f;
        animador.SetFloat("VelocidadFase", 1.5f);
        ControladorAtaque.GetComponent<ParticulaDmg>().Damage += 10;
        GameObject.Find("EventSystem").GetComponent<AudioSource>().pitch = 1.05f;
        GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = 0.7f;
        foreach (var param in animador.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                if(!param.name.Equals("SegundaFase")) animador.ResetTrigger(param.name);
            }
        }

    }
}
