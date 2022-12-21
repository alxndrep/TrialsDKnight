using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerMovimiento : MonoBehaviour
{
    public static PlayerMovimiento Player;

    [Header("Componentes")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private EfectosCD DodgeCD;
    [SerializeField] private EfectosCD EsquivarCD;
    [HideInInspector] public Inventario inventarioPlayer;
    [SerializeField] private UI_Inventory uiInventario;
    [HideInInspector] public string UltimoEscenario;
    [HideInInspector] public ShadowTrail shadowTrail;
    [HideInInspector] public Color originalColorShadowTrail;
    [HideInInspector] public ParticleSystem DustPS;
    private Vector2 PosicionComienzoEscenario;

    [HideInInspector] public Rigidbody2D cuerpoFisico;
    [HideInInspector] public Animator animador;
    private BoxCollider2D boxCollider;
    [HideInInspector] public PlayerSFX playerSFX;

    /////////////////
    [Header("Variables de Distancia Techo")]
    [SerializeField] private Transform controladorTecho;
    [SerializeField] private Vector3 dimensionesCajaTecho;

    [Header("Variables de Distancia Suelo")]
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCajaSuelo;

    [Header("Movimiento")]
    [SerializeField] public float velocidadMovimiento;
    [HideInInspector] public float velocidadMovimientoBase;
    [HideInInspector] public float inputX;
    private float movimientoHorizontal = 0f;
    [HideInInspector] public bool mirandoDerecha;
    private bool puedePararse;
    [HideInInspector] public bool sePuedeMover;
    [SerializeField] private Vector2 velocidadRebote;
    public Vector2 ultimoCheckPoint;

    [Header("Salto Pared")]
    [SerializeField] private Transform controladorPared;
    [SerializeField] private Vector3 dimensionesCajaPared;
    [SerializeField] private float velocidadDeslizarPared;
    [SerializeField] private float tiempoSaltoPared;
    private bool saltandoDePared;
    private bool enPared;
    [HideInInspector] public bool deslizandoPared;


    [Header("Saltos")]
    [Range(0, 3)] [SerializeField] private int cantidadSaltos;
    [Range(0, 0.4f)] [SerializeField] private float buttonTiempo;
    [SerializeField] private float fuerzaSalto;
    [HideInInspector] public bool enSuelo;
    private bool salto = false;
    private int saltosRestantes;
    private float saltoTiempo;

    [Header("Esquivar y Deslizar")]
    [SerializeField] private float enfriamientoED;
    [SerializeField] private float enfriamientoSlide;
    [SerializeField] private float fuerzaED;
    [SerializeField] private float alturaED;
    [HideInInspector] public bool esquivando;
    [HideInInspector] public bool dodgeando;
    [HideInInspector] public bool deslizando;
    private bool CDesquivando;
    private bool CDdeslizando;
    private float esquivarRestantes;
    private float dodgeRestantes;
    private float enfriamientoDeslizarRestante;
    private float enfriamientoEDRestante;

    [Header("Controles")]
    [HideInInspector] public Dictionary<string, KeyCode> Controles;

    private void Awake()
    {
        if (PlayerMovimiento.Player == null)
        {
            PlayerMovimiento.Player = this;
            inventarioPlayer = new Inventario();
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), false);
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = true;
        PlayerCombateCuerpo.Player.golpeado = false;
        PlayerCombateCuerpo.Player.estaInvulnerable = false;
        PlayerCombateCuerpo.Player.DamageFlicker.GetComponent<Light2D>().intensity = 0;
        Time.timeScale = 1f;
        mirandoDerecha = true;
        //sePuedeMover = true;
        transform.eulerAngles = Vector3.zero;
        PosicionComienzoEscenario = GameObject.Find("PosicionComienzoEscenario").transform.position;
        uiInventario = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").GetComponent<UI_Inventory>();
        DodgeCD = GameObject.Find("DodgeCD").GetComponent<EfectosCD>();
        EsquivarCD = GameObject.Find("EsquivandoCD").GetComponent<EfectosCD>();
        PlayerCombateCuerpo.Player.barraDeVida = GameObject.Find("BarraHP").GetComponent<BarraStats>();
        PlayerCombateCuerpo.Player.barraDeVida.Start();
        PlayerCombateCuerpo.Player.barraDeArmor = GameObject.Find("BarraArmor").GetComponent<BarraStats>();
        PlayerCombateCuerpo.Player.barraDeArmor.Start();
        PlayerCombateCuerpo.Player.barraDeSoulMana = GameObject.Find("BarraSoulMana").GetComponent<BarraStats>();
        PlayerCombateCuerpo.Player.barraDeSoulMana.Start();
        PlayerCombateCuerpo.Player.InvulnerabilidadCD = GameObject.Find("InvulnerableCD").GetComponent<EfectosCD>();
        shadowTrail = GetComponent<ShadowTrail>();
        DustPS = transform.Find("DustPS").GetComponent<ParticleSystem>();
        PlayerCombateHabilidad.Player.Start();
        PlayerStats.Stats.barraExperiencia = GameObject.Find("BarraExperiencia").GetComponent<BarraStats>();
        PlayerStats.Stats.barraExperiencia.Start();
        PlayerStats.Stats.barraExperiencia.InicializarBarraDeStats((float)PlayerStats.Stats.experienciaSgteNivel, (float)PlayerStats.Stats.experienciaActual);
        PlayerInventario.Instance.inventario = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").gameObject;
        PlayerInventario.Instance.PanelStats = PlayerInventario.Instance.inventario.transform.Find("PanelStats").gameObject;
        PlayerInventario.Instance.InventarioHUD = PlayerInventario.Instance.inventario.transform.Find("InventarioHUD").gameObject;
        PlayerInventario.Instance.StartNewScene();
        SceneManager.sceneLoaded += onSceneLoaded;
        transform.position = PosicionComienzoEscenario;
        velocidadMovimientoBase = 7.5f;
        uiInventario.SetInventario(inventarioPlayer);
        Controles = ConfigControles.Instance.getConfiguracion();
        mirandoDerecha = true;
        cuerpoFisico = GetComponent<Rigidbody2D>();
        animador = GetComponent<Animator>();
        saltosRestantes = cantidadSaltos;
        boxCollider = GetComponent<BoxCollider2D>();
        playerSFX = GetComponent<PlayerSFX>();
        originalColorShadowTrail = shadowTrail._Color;
        cuerpoFisico.velocity = Vector3.zero;
        Input.ResetInputAxes();
        cuerpoFisico.constraints = RigidbodyConstraints2D.FreezeRotation;


    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(this != null)
        {
            if (!scene.name.Equals("MainMenu"))
            {
                Start();
                if (GameObject.Find("PosicionComienzoEscenario") != null)
                {
                    gameObject.SetActive(true);
                    PosicionComienzoEscenario = GameObject.Find("PosicionComienzoEscenario").transform.position;
                    transform.position = PosicionComienzoEscenario;
                }
                uiInventario.SetInventario(inventarioPlayer);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), false);
                PlayerCombateCuerpo.Player.ActualizarStatsCombate();
            }
        }

    }

    void Update()
    {
        if(Time.timeScale > 0 & sePuedeMover)
        {
            if (Input.GetKey(Controles["Left"]) || Input.GetKey(Controles["Right"]))
            {
                inputX = Input.GetAxis("Horizontal");
                if (PlayerCombateCuerpo.Player.denyingAttack) movimientoHorizontal = 0;
                else movimientoHorizontal = inputX * velocidadMovimiento;
                animador.SetBool("EstaCorriendo", true);
            }
            else
            {
                movimientoHorizontal = 0;
                inputX = 0;
                animador.SetBool("EstaCorriendo", false);
            }
            if(Input.GetKey(Controles["Down"]) & animador.GetBool("HaySuelo"))
            {
                animador.SetBool("EstaAgachado", true);
            }
            else if(Input.GetKeyUp(Controles["Down"]))
            {
                animador.SetBool("EstaAgachado", false);
            }
            ProcesarMovimiento(movimientoHorizontal);
            //// Procesar si se esta deslizando x la pared /////
            ProcesarSaltoPared();
            animador.SetBool("DeslizandoPared", deslizandoPared);
            if (!enSuelo && enPared && inputX != 0 && !deslizando & !esquivando)
            {
                deslizandoPared = true;
            }
            else deslizandoPared = false;
            if(saltandoDePared) shadowTrail.Sombras_Skill();
            /////////////////////////////////
            ProcesarSlide();
            ProcesarSalto();
            ProcesarEsquivar();
            ProcesarPararse();
            ProcesarDeslizamientoPared();
            //if(!salto) ProcesarHaySuelo();
        }
        if (!salto) ProcesarHaySuelo();
    }

    private void ProcesarDeslizamientoPared()
    {
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, capaSuelo);
        if (deslizandoPared)
        {
            CrearEfectoDustPS();
            cuerpoFisico.velocity = new Vector2(cuerpoFisico.velocity.x, Mathf.Clamp(cuerpoFisico.velocity.y, -velocidadDeslizarPared, float.MaxValue));
        }
    }

    private void ProcesarSlide()
    {
        if (Input.GetKeyDown(Controles["Down"]) & enSuelo & !deslizando & !CDdeslizando & animador.GetBool("EstaCorriendo") & !animador.GetBool("Golpeado"))
        {
            EsquivarCD.InicializarCD(enfriamientoSlide);
            animador.SetTrigger("Deslizar");
            CDdeslizando = true;
            cuerpoFisico.velocity = Vector2.zero;
            cuerpoFisico.velocity = mirandoDerecha? new Vector2(velocidadMovimiento, cuerpoFisico.velocity.y) : new Vector2(-velocidadMovimiento, cuerpoFisico.velocity.y);
            enfriamientoDeslizarRestante = enfriamientoSlide;
            animador.SetBool("EstaAgachado", false);
            playerSFX.playSoundFX(playerSFX.SlideAndRollsSFX, (int) Random.Range(0, playerSFX.SlideAndRollsSFX.Length));

        }
        if (CDdeslizando)
        {
            animador.SetBool("EstaAgachado", false);
            enfriamientoDeslizarRestante -= Time.deltaTime;
            EsquivarCD.CambiarCDActual(enfriamientoDeslizarRestante);
            if (enfriamientoDeslizarRestante <= 0)
            {
                CDdeslizando = false;
                animador.SetBool("EstaAgachado", true);
            }
        }
    }
    public IEnumerator DodgeCollisionDesactivar(float dodgeTiempoInvulnerable)
    {
        if (!PlayerCombateCuerpo.Player.estaInvulnerable)
        {
            Physics2D.IgnoreLayerCollision(8, 9, true);
            PlayerCombateCuerpo.Player.estaInvulnerable = true;
            yield return new WaitForSeconds(dodgeTiempoInvulnerable);
            Physics2D.IgnoreLayerCollision(8, 9, false);
            PlayerCombateCuerpo.Player.estaInvulnerable = false;
        }

    }
    private void ProcesarEsquivar()
    {
        if (Input.GetKeyDown(Controles["Run"]) & !esquivando & !CDesquivando & !deslizandoPared & esquivarRestantes > 0 & animador.GetBool("EstaCorriendo") & !animador.GetBool("Golpeado"))
        {
            DodgeCD.InicializarCD(enfriamientoED);
            animador.SetTrigger("Esquivar");
            CDesquivando = true;
            esquivarRestantes--;
            cuerpoFisico.velocity = Vector2.zero;
            StartCoroutine(DodgeCollisionDesactivar(0.75f));
            if (!animador.GetBool("EstaAgachado")) cuerpoFisico.velocity = mirandoDerecha ? new Vector2(velocidadMovimiento, alturaED) : new Vector2(-velocidadMovimiento, alturaED);
            else cuerpoFisico.velocity = mirandoDerecha ? new Vector2(velocidadMovimiento, 0) : new Vector2(-velocidadMovimiento, 0);
            enfriamientoEDRestante = enfriamientoED;
            playerSFX.playSoundFX(playerSFX.jumpSFX, (int)Random.Range(0, playerSFX.jumpSFX.Length));
        }
        else if (Input.GetKeyDown(Controles["Run"]) & !dodgeando & !CDesquivando & !CDdeslizando & dodgeRestantes > 0 & !animador.GetBool("EstaCorriendo"))
        {
            DodgeCD.InicializarCD(enfriamientoED);
            animador.SetTrigger("Dodge");
            CDesquivando = true;
            dodgeRestantes--;
            StartCoroutine(DodgeCollisionDesactivar(0.75f));
            enfriamientoEDRestante = enfriamientoED;

        }
        if (CDesquivando)
        {
            enfriamientoEDRestante -= Time.deltaTime;
            DodgeCD.CambiarCDActual(enfriamientoEDRestante);
            if (enfriamientoEDRestante <= 0)
            {
                CDesquivando = false;
            }
        }
    }

    private void ProcesarHaySuelo()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSuelo, 0f, capaSuelo);
        if (enSuelo)
        {
            saltosRestantes = cantidadSaltos;
            esquivarRestantes = 1;
            dodgeRestantes = 1;
            animador.SetBool("HaySuelo", true);
        }
        else
        {
            animador.SetBool("HaySuelo", false);
        }
    }
    public void ProcesarPararse()
    {
        if(!deslizando | !dodgeando | !esquivando)
        {
            controladorTecho.position = new Vector2(controladorTecho.position.x, boxCollider.bounds.center.y + boxCollider.bounds.extents.y + boxCollider.edgeRadius);
            puedePararse = Physics2D.OverlapBox(controladorTecho.position, dimensionesCajaTecho, 0f, capaSuelo);
            if (!puedePararse & !Input.GetKey(Controles["Down"])) animador.SetBool("EstaAgachado", false);
            else animador.SetBool("EstaAgachado", true);
        }

    }

    void ProcesarSalto()
    {

        if (Input.GetKeyDown(Controles["Jump"]) & saltosRestantes > 0 & !deslizandoPared & !animador.GetBool("Golpeado"))
        {
            bool puedeSaltar = Physics2D.OverlapBox(controladorTecho.position, dimensionesCajaTecho, 0f, capaSuelo);
            if (!puedeSaltar)
            {
                if (enSuelo) CrearEfectoDustPS();
                animador.SetBool("HaySuelo", false);
                saltosRestantes--;
                saltoTiempo = 0;
                salto = true;
                cuerpoFisico.velocity = Vector3.zero;
                animador.SetTrigger("Salto");
                playerSFX.playSoundFX(playerSFX.jumpSFX, (int)Random.Range(0, playerSFX.jumpSFX.Length));
            }
        }
        if (salto)
        {
            cuerpoFisico.velocity = new Vector2(cuerpoFisico.velocity.x, 11 + ((PlayerStats.Stats.agilidad*0.33f)));
            saltoTiempo += Time.deltaTime;
        }
        if (Input.GetKeyUp(Controles["Jump"]) | saltoTiempo > buttonTiempo)
        {
            salto = false;
        }
    }
    void ProcesarSaltoPared()
    {
        if(Input.GetKeyDown(Controles["Jump"]) & enPared & deslizandoPared & !animador.GetBool("Golpeado"))
        {
            CrearEfectoDustPS();
            animador.SetTrigger("Salto");
            enPared = false;
            saltosRestantes--;
            GestionarGiro(mirandoDerecha ? -1 : 1);
            cuerpoFisico.velocity = Vector3.zero;
            cuerpoFisico.velocity = new Vector2(velocidadMovimiento*1.3f * -inputX, 13 + (PlayerStats.Stats.agilidad*0.33f));
            playerSFX.playSoundFX(playerSFX.jumpSFX, (int)Random.Range(0, playerSFX.jumpSFX.Length));
            StartCoroutine(CambioSaltoPared());
        }
    }
    private IEnumerator CambioSaltoPared()
    {
        saltandoDePared = true;
        yield return new WaitForSeconds(tiempoSaltoPared);
        saltandoDePared = false;
    }
    void ProcesarMovimiento(float mover)
    {
        if (!saltandoDePared)
        {
            if (!esquivando & !deslizando)
            {
                Vector3 velocidadObjetivo = new Vector2(mover, cuerpoFisico.velocity.y);
                cuerpoFisico.velocity = new Vector2(velocidadObjetivo.x, cuerpoFisico.velocity.y);
                GestionarGiro(mover);
            }
            else if((esquivando | deslizando) & salto)
            {
                Vector3 velocidadObjetivo = new Vector2(mover, cuerpoFisico.velocity.y);
                cuerpoFisico.velocity = new Vector2(velocidadObjetivo.x, cuerpoFisico.velocity.y);
                GestionarGiro(mover);
            }
        }

        
    }
    void GestionarGiro(float mover)
    {
        if ((mirandoDerecha && mover  < 0) || (mirandoDerecha == false && mover > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            if(enSuelo) CrearEfectoDustPS();
        }
    }
    public void ReboteDamage(Vector2 puntoGolpe)
    {
        if (puntoGolpe.y < -0.9f) cuerpoFisico.velocity = new Vector2(-velocidadRebote.x, velocidadRebote.y);
        else cuerpoFisico.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }
    public void ReboteDamage(float direccion)
    {
        cuerpoFisico.velocity = Vector2.zero;
        cuerpoFisico.velocity = new Vector2(velocidadRebote.x * direccion, velocidadRebote.y);
    }
    public void SetGolpeado(int state)
    {
        animador.SetBool("Golpeado", state == 1? true:false );
    }

    public void CrearEfectoDustPS()
    {
        DustPS.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSuelo);
        Gizmos.DrawWireCube(controladorTecho.position, dimensionesCajaTecho);
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }

    public void SetCheckPoint(Vector2 position)
    {
        ultimoCheckPoint = position;
    }

}