using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerCombateCuerpo : MonoBehaviour
{
    public static PlayerCombateCuerpo Player;
    [Header("Componentes")]
    [SerializeField] private AudioSource audioSRC;
    [SerializeField] public BarraStats barraDeVida;
    [SerializeField] public BarraStats barraDeArmor;
    [SerializeField] public BarraStats barraDeSoulMana;
    [SerializeField] public EfectosCD InvulnerabilidadCD;
    [SerializeField] public GameObject DamageFlicker;
    [SerializeField] private GameObject efectoTextoDamage;
    [SerializeField] private GameObject efectoTextoHeal;
    [SerializeField] private GameObject[] efectoDenyAttack;
    [SerializeField] private float TiempoEfectoDenyAttack;
    [HideInInspector] public PlayerMovimiento jugador;
    private Animator animador;
    private PlayerSFX playerSFX;
    [HideInInspector] public bool estaMuerto;

    [Header("Vida, Armadura y SoulMana")]
    [SerializeField] private float tiempoPerdidaControl;
    [SerializeField] private float tiempoInvulnerable;
    private float tiempoInvulnerableRestante;
    [HideInInspector] public bool estaInvulnerable;
    [HideInInspector] public float MaxHP_Jugador;
     public float HP_Jugador;
    [HideInInspector] public float MaxArmor_Jugador;
     public float Armor_Jugador;
    [HideInInspector] public float MaxSoulMana_Jugador;
     public float SoulMana_Jugador;
    private float tiempoRegeneracion;

    [Header("Combate Cuerpo a Cuerpo")]
    [SerializeField] public Transform controladorGolpe;
    [SerializeField] public float radioGolpe;
    [HideInInspector] public float damageAtaque;
    [HideInInspector] public float criticalChance;
    [SerializeField] private float enfriamientoAtaqueBase;
    [SerializeField] private float enfriamientoAtaque;
    [SerializeField] private float cantidadAtaques;
    [SerializeField] private float knockbackArma;
    [HideInInspector] public bool denyingAttack;
    [HideInInspector] public float tiempoRestanteDenyAttack;
    [HideInInspector] public bool atacando;
    private float enfriamientoAtaqueRestante;
    private float ataquesRestantes;
    [HideInInspector] public bool golpeado;

    [Header("Efectos y Spells")]
    [SerializeField] private GameObject efectoHeal;
    [SerializeField] private GameObject[] efectoGolpeSangre;
    public IEnumerator invulnerableFX;

    private void Awake()
    {
        if (PlayerCombateCuerpo.Player == null)
        {
            PlayerCombateCuerpo.Player = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private float calcularSumaUpgradeArmorSet(Inventario inventario, Item.ItemUpgrades upgrade)
    {
        float calculo = 0;
        foreach(Item item in inventario.GetListaItem())
        {
            if (item.equipado)
            {
                for (int i = 0; i < item.caracteristicas.Length; i++)
                {
                    if(item.caracteristicas[i].Key == upgrade)
                    {
                        calculo += item.caracteristicas[i].Value;
                    }
                }
            }
        }
        return calculo;
    }
    public void ActualizarStatsCombate()
    {
        PlayerStats.Stats.ActualizarStatsCombate();

        PlayerMovimiento.Player.velocidadMovimiento = PlayerMovimiento.Player.velocidadMovimientoBase + (PlayerStats.Stats.agilidadMOD * 0.1f);
        float newhp = (8 * PlayerStats.Stats.constitucionMOD) + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, Item.ItemUpgrades.vida);
        float newarmor = PlayerStats.Stats.constitucionMOD + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, Item.ItemUpgrades.armor);
        float newsoulmana = (5 * PlayerStats.Stats.inteligenciaMOD) + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, Item.ItemUpgrades.soulmana);
        damageAtaque = calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, Item.ItemUpgrades.ataque) + (PlayerStats.Stats.fuerzaMOD * 0.66f) + (PlayerStats.Stats.destrezaMOD * 0.5f);
        criticalChance = (3 + PlayerStats.Stats.destrezaMOD * 0.33f);
        enfriamientoAtaque = enfriamientoAtaqueBase - (PlayerStats.Stats.destrezaMOD * 0.016f + PlayerStats.Stats.agilidadMOD * 0.008f);

        MaxSoulMana_Jugador = newsoulmana;
        if (SoulMana_Jugador > MaxSoulMana_Jugador) SoulMana_Jugador = MaxSoulMana_Jugador;
        barraDeSoulMana.CambiarStatMaximo(newsoulmana);
        barraDeSoulMana.CambiarStatActual(SoulMana_Jugador);

        MaxHP_Jugador = newhp;
        if (HP_Jugador > MaxHP_Jugador) HP_Jugador = MaxHP_Jugador;
        barraDeVida.CambiarStatMaximo(newhp);
        barraDeVida.CambiarStatActual(HP_Jugador);

        MaxArmor_Jugador = newarmor;
        if (Armor_Jugador > MaxArmor_Jugador) Armor_Jugador = MaxArmor_Jugador;
        barraDeArmor.CambiarStatMaximo(newarmor);
        barraDeArmor.CambiarStatActual(Armor_Jugador);

        PlayerCombateHabilidad.Player.CalculoCooldownHabilidad();
    }
    private void Start()
    {
        ActualizarStatsCombate();
        SceneManager.sceneLoaded += onSceneLoaded;
        jugador = GetComponent<PlayerMovimiento>();
        animador = GetComponent<Animator>();
        ataquesRestantes = cantidadAtaques;
        playerSFX = GetComponent<PlayerSFX>();
        HP_Jugador = MaxHP_Jugador;
        Armor_Jugador = MaxArmor_Jugador;
        SoulMana_Jugador = MaxSoulMana_Jugador;
        tiempoInvulnerableRestante = tiempoInvulnerable;

        barraDeVida.InicializarBarraDeStats(MaxHP_Jugador, HP_Jugador);
        barraDeArmor.InicializarBarraDeStats(MaxArmor_Jugador, Armor_Jugador);
        barraDeSoulMana.InicializarBarraDeStats(MaxSoulMana_Jugador, SoulMana_Jugador);

        tiempoRegeneracion = 1f;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("MainMenu"))
        {
            barraDeVida = GameObject.Find("BarraHP").GetComponent<BarraStats>();
            barraDeArmor = GameObject.Find("BarraArmor").GetComponent<BarraStats>();
            barraDeSoulMana = GameObject.Find("BarraSoulMana").GetComponent<BarraStats>();
            InvulnerabilidadCD = GameObject.Find("InvulnerableCD").GetComponent<EfectosCD>();
            barraDeVida.InicializarBarraDeStats(MaxHP_Jugador, HP_Jugador);
            barraDeArmor.InicializarBarraDeStats(MaxArmor_Jugador, Armor_Jugador);
            barraDeSoulMana.InicializarBarraDeStats(MaxSoulMana_Jugador, SoulMana_Jugador);
        }
    }
    private void Update()
    {
        if (!estaMuerto)
        {
            if (Time.timeScale > 0 & jugador.sePuedeMover)
            {
                ProcesarGolpe();
                if (golpeado)
                {
                    tiempoInvulnerableRestante -= Time.deltaTime;
                    InvulnerabilidadCD.CambiarCDActual(tiempoInvulnerableRestante);
                    if (tiempoInvulnerableRestante <= 0)
                    {
                        tiempoInvulnerableRestante = tiempoInvulnerable;
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), false);
                        StopCoroutine(invulnerableFX);
                        StopAllCoroutines();
                        estaInvulnerable = false;
                        DamageFlicker.GetComponent<Light2D>().intensity = 0;
                        golpeado = false;
                        jugador.shadowTrail._Color = jugador.originalColorShadowTrail;
                    }
                }
                if (denyingAttack)
                {
                    jugador.shadowTrail.Sombras_Skill();
                    Time.timeScale = 0.5f;
                    tiempoRestanteDenyAttack -= Time.deltaTime;
                    jugador.cuerpoFisico.AddForce(new Vector2(15 * (jugador.mirandoDerecha ? -1 : 1), 0));
                    if(tiempoRestanteDenyAttack <= 0f)
                    {
                        Time.timeScale = 1f;
                        denyingAttack = false;
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), false);
                        GameObject.Find("GrayScaleEffect").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0f);
                        estaInvulnerable = false;

                    }
                }
            }
            ProcesarRegeneracion();
        }


    }
    public void ProcesarRegeneracion()
    {
        tiempoRegeneracion -= Time.deltaTime;
        if (tiempoRegeneracion <= 0)
        {
            if (!golpeado)
            {
                if(Armor_Jugador < MaxArmor_Jugador)
                {
                    Armor_Jugador += (PlayerStats.Stats.constitucionMOD * 0.075f);
                    barraDeArmor.CambiarStatActual(Armor_Jugador);
                }
            }


            if(SoulMana_Jugador < MaxSoulMana_Jugador)
            {
                SoulMana_Jugador += (PlayerStats.Stats.inteligenciaMOD * 0.1f);
                barraDeSoulMana.CambiarStatActual(SoulMana_Jugador);
            }
            tiempoRegeneracion = 1f;
        }
    }
    public void CurarHP(float heal)
    {
        if(HP_Jugador + heal >= MaxHP_Jugador) HP_Jugador = MaxHP_Jugador;
        else HP_Jugador += heal;

        barraDeVida.CambiarStatActual(HP_Jugador);
        EfectoTextoDmg efecto = Instantiate(efectoTextoHeal, transform.position, Quaternion.identity).transform.Find("Texto").GetComponent<EfectoTextoDmg>();
        efecto.Setup(heal, new Color(0.1835147f, 0.6132076f, 0f, 1f));
    }
    public void CurarManaSoul(float soulRegen)
    {
        if (SoulMana_Jugador + soulRegen >= MaxSoulMana_Jugador) SoulMana_Jugador = MaxSoulMana_Jugador;
        else SoulMana_Jugador += soulRegen;

        barraDeSoulMana.CambiarStatActual(SoulMana_Jugador);
        EfectoTextoDmg efecto = Instantiate(efectoTextoHeal, transform.position, Quaternion.identity).transform.Find("Texto").GetComponent<EfectoTextoDmg>();
        efecto.Setup(soulRegen, new Color(0.06550375f, 0.1675892f, 0.6037736f, 1f));
    }
    public void Revivir()
    {
        animador.SetBool("EstaMuerto", false);
        animador.SetTrigger("Revivir");
        estaMuerto = false;
        jugador.sePuedeMover = true;
        estaInvulnerable = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), false);
        HP_Jugador = MaxHP_Jugador;
        SoulMana_Jugador = MaxSoulMana_Jugador;
        Armor_Jugador = MaxArmor_Jugador;
        ActualizarStatsCombate();
    }

    public void ProcesarMuerte()
    {
        animador.SetBool("EstaMuerto", true);
        jugador.sePuedeMover = false;
        estaMuerto = true;
        animador.SetTrigger("Muerte");
        estaInvulnerable = true;
        audioSRC.PlayOneShot(playerSFX.player_death);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), true);
        GameObject.Find("PantallaMuerte").GetComponent<PantallaMuerte>().Setup();
    }
    public void RecibirDamageLetal(float damage)
    {
        Armor_Jugador = 0;
        HP_Jugador = 0;
        barraDeVida.CambiarStatActual(HP_Jugador);
        barraDeArmor.CambiarStatActual(Armor_Jugador);
        Instantiate(efectoGolpeSangre[(int)UnityEngine.Random.Range(0, efectoGolpeSangre.Length)], transform.position, Quaternion.identity);
        EfectoTextoDmg efecto = Instantiate(efectoTextoDamage, transform.position, Quaternion.identity).GetComponent<EfectoTextoDmg>();
        efecto.Setup(damage);
        StartCoroutine(SetVelocidadonHit(0));
        ProcesarMuerte();
    }
    public void RecibirDamage(float damage)
    {
        if (!estaInvulnerable)
        {
            Instantiate(efectoGolpeSangre[(int)UnityEngine.Random.Range(0, efectoGolpeSangre.Length)],transform.position,Quaternion.identity);
            EfectoTextoDmg efecto = Instantiate(efectoTextoDamage, transform.position, Quaternion.identity).GetComponent<EfectoTextoDmg>();
            efecto.Setup(damage);
            if (Armor_Jugador >= damage * 0.66f)
            {
                Armor_Jugador -= damage * 0.66f;
                HP_Jugador -= damage * 0.33f;
                barraDeVida.CambiarStatActual(HP_Jugador);
                barraDeArmor.CambiarStatActual(Armor_Jugador);
            }
            else
            {
                float dif_armor_dmg = -(Armor_Jugador - damage * 0.66f);
                Armor_Jugador = 0;
                HP_Jugador -= (dif_armor_dmg + damage*0.33f);
                barraDeVida.CambiarStatActual(HP_Jugador);
                barraDeArmor.CambiarStatActual(Armor_Jugador);
            }
            if(HP_Jugador <= 0)
            {
                StartCoroutine(SetVelocidadonHit(0));
                ProcesarMuerte();
            }
            else
            {
                audioSRC.PlayOneShot(playerSFX.hitSFX[(int)UnityEngine.Random.Range(0, playerSFX.hitSFX.Length)]);
                animador.SetTrigger("Hit");
                animador.SetBool("Golpeado", true);
                StartCoroutine(SetVelocidadonHit(0));
                CinemachineMovCamera.Instance.MoverCamera(18, 18, 0.8f);
                StartCoroutine(Perdercontrol());
                estaInvulnerable = true;
                invulnerableFX = EfectoInvulnerable();
                StartCoroutine(invulnerableFX);
                InvulnerabilidadCD.InicializarCD(tiempoInvulnerable);
                StartCoroutine(DesactivarColision());
            }
        }


    }
    public void RecibirDamage(float damage, Vector2 posicion)
    {
        if (!estaInvulnerable)
        {
            Instantiate(efectoGolpeSangre[(int)UnityEngine.Random.Range(0, efectoGolpeSangre.Length)], transform.position, Quaternion.identity);
            EfectoTextoDmg efecto = Instantiate(efectoTextoDamage, transform.position, Quaternion.identity).GetComponent<EfectoTextoDmg>();
            efecto.Setup(damage);
            if (Armor_Jugador >= damage * 0.66f)
            {
                Armor_Jugador -= damage * 0.66f;
                HP_Jugador -= damage * 0.33f;
                barraDeVida.CambiarStatActual(HP_Jugador);
                barraDeArmor.CambiarStatActual(Armor_Jugador);
            }
            else
            {
                float dif_armor_dmg = -(Armor_Jugador - damage * 0.66f);
                Armor_Jugador = 0;
                HP_Jugador -= (dif_armor_dmg + damage * 0.33f);
                barraDeVida.CambiarStatActual(HP_Jugador);
                barraDeArmor.CambiarStatActual(Armor_Jugador);
            }
            if(HP_Jugador <= 0)
            {
                StartCoroutine(SetVelocidadonHit(0));
                jugador.ReboteDamage(posicion);
                ProcesarMuerte();
            }
            else
            {
                audioSRC.PlayOneShot(playerSFX.hitSFX[(int)UnityEngine.Random.Range(0, playerSFX.hitSFX.Length)]);
                animador.SetTrigger("Hit");
                animador.SetBool("Golpeado", true);
                StartCoroutine(SetVelocidadonHit(0));
                CinemachineMovCamera.Instance.MoverCamera(18, 18, 0.8f);
                StartCoroutine(Perdercontrol());
                estaInvulnerable = true;
                invulnerableFX = EfectoInvulnerable();
                StartCoroutine(invulnerableFX);
                InvulnerabilidadCD.InicializarCD(tiempoInvulnerable);
                StartCoroutine(DesactivarColision());
                jugador.ReboteDamage(posicion);
            }
        }

    }

    public void RecibirDamage(float damage, float direccion)
    {
        if (!estaInvulnerable)
        {
            Instantiate(efectoGolpeSangre[(int)UnityEngine.Random.Range(0, efectoGolpeSangre.Length)], transform.position, Quaternion.identity);
            EfectoTextoDmg efecto = Instantiate(efectoTextoDamage, transform.position, Quaternion.identity).GetComponent<EfectoTextoDmg>();
            efecto.Setup(damage);
            if (Armor_Jugador >= damage * 0.66f)
            {
                Armor_Jugador -= damage * 0.66f;
                HP_Jugador -= damage * 0.33f;
                barraDeVida.CambiarStatActual(HP_Jugador);
                barraDeArmor.CambiarStatActual(Armor_Jugador);
            }
            else
            {
                float dif_armor_dmg = -(Armor_Jugador - damage * 0.66f);
                Armor_Jugador = 0;
                HP_Jugador -= (dif_armor_dmg + damage * 0.33f);
                barraDeVida.CambiarStatActual(HP_Jugador);
                barraDeArmor.CambiarStatActual(Armor_Jugador);
            }
            if (HP_Jugador <= 0)
            {
                StartCoroutine(SetVelocidadonHit(0));
                jugador.ReboteDamage(direccion);
                ProcesarMuerte();
            }
            else
            {
                audioSRC.PlayOneShot(playerSFX.hitSFX[(int)UnityEngine.Random.Range(0, playerSFX.hitSFX.Length)]);
                animador.SetTrigger("Hit");
                animador.SetBool("Golpeado", true);
                StartCoroutine(SetVelocidadonHit(0));
                CinemachineMovCamera.Instance.MoverCamera(18, 18, 0.8f);
                StartCoroutine(Perdercontrol());
                estaInvulnerable = true;
                invulnerableFX = EfectoInvulnerable();
                StartCoroutine(invulnerableFX);
                InvulnerabilidadCD.InicializarCD(tiempoInvulnerable);
                StartCoroutine(DesactivarColision());
                jugador.ReboteDamage(direccion);
            }
        }

    }
    private IEnumerator SetVelocidadonHit(float velocidad)
    {
        yield return new WaitForSeconds(0.25f);
        jugador.cuerpoFisico.velocity = new Vector2(velocidad, jugador.cuerpoFisico.velocity.y);
    }
    private IEnumerator Perdercontrol()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), true);
        jugador.sePuedeMover = false;
        yield return new WaitForSeconds(tiempoPerdidaControl);
        jugador.sePuedeMover = true;
        animador.SetBool("Golpeado", false);
    }
    private IEnumerator DesactivarColision()
    {
        yield return new WaitForSeconds(0.44f);
        golpeado = true;
    }

    private IEnumerator EfectoInvulnerable()
    {
        while (true)
        {
            DamageFlicker.GetComponent<Light2D>().intensity = 2;
            //GetComponent<SpriteRenderer>().material.color = Color.yellow;
            yield return new WaitForSeconds(0.15f);
            DamageFlicker.GetComponent<Light2D>().intensity = 0;
            //GetComponent<SpriteRenderer>().material.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
    }
    public void InvulnerabilidadFX()
    {
        StartCoroutine(DesactivarColision());
    }

    private void ProcesarGolpe()
    {
        if (Input.GetKeyDown(jugador.Controles["Attack"]) & ataquesRestantes > 0 & !jugador.deslizando & !jugador.esquivando & !jugador.dodgeando & jugador.sePuedeMover & !jugador.deslizandoPared & !animador.GetBool("Golpeado"))
        {
            atacando = true;
            if (!animador.GetBool("EstaAgachado")) enfriamientoAtaqueRestante = enfriamientoAtaque;
            else enfriamientoAtaqueRestante = enfriamientoAtaque*0.65f;
            float critChance = UnityEngine.Random.Range(0, 101);
            if (critChance <= criticalChance & !animador.GetBool("EstaAgachado"))
            {
                animador.SetTrigger("AtaqueCritico");
                ataquesRestantes = cantidadAtaques;
            }
            else animador.SetTrigger("Ataque");
            ataquesRestantes--;
        }
        if (atacando)
        {
            enfriamientoAtaqueRestante -= Time.deltaTime;
            if (enfriamientoAtaqueRestante <= 0)
            {
                atacando = false;
                ataquesRestantes = cantidadAtaques;
            }
        }
    }

    public void RealizarDamage(AnimationEvent animEvent)
    {
        float damageMultiplicador = animEvent.floatParameter;
        int isCritico = animEvent.intParameter;

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        bool enemigoMurio;
        if (isCritico == 0) audioSRC.PlayOneShot(playerSFX.swordSlashSFX[UnityEngine.Random.Range(0, 3)]);
        else
        {
            audioSRC.PlayOneShot(playerSFX.swordSlashSFX[0]);
            audioSRC.PlayOneShot(playerSFX.swordSlashSFX[1]);
            audioSRC.PlayOneShot(playerSFX.swordSlashSFX[2]);
        }
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                int direccion = jugador.transform.position.x > colisionador.transform.position.x ? -1 : 1;
                if (!denyingAttack)
                {
                    if(isCritico == 0) enemigoMurio = colisionador.GetComponent<EnemigoVidaHP>().TomarDamage(damageAtaque * damageMultiplicador, knockbackArma, direccion, false);
                    else enemigoMurio = colisionador.GetComponent<EnemigoVidaHP>().TomarDamage(damageAtaque * damageMultiplicador, knockbackArma, direccion, true);
                    if (!animador.GetBool("EstaAgachado"))
                    {
                        if (!enemigoMurio)
                        {
                            audioSRC.PlayOneShot(playerSFX.swordHitSFX[1]);
                        }
                        else audioSRC.PlayOneShot(playerSFX.sword_kill);
                    }
                    else
                    {
                        if (!enemigoMurio) audioSRC.PlayOneShot(playerSFX.swordHitSFX[0]);
                        else audioSRC.PlayOneShot(playerSFX.sword_kill);
                    }
                    CinemachineMovCamera.Instance.MoverCamera(6, 6, 0.2f);
                }

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ataque") & !denyingAttack)
        {
            collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(250 * (jugador.mirandoDerecha ? 1 : -1), 0));
            estaInvulnerable = true;
            GameObject.Find("GrayScaleEffect").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.85f);
            Vector2 efectoPos = collision.transform.position;
            float distancia = Vector2.Distance(transform.position, efectoPos) / 2;
            efectoPos = new Vector2(transform.position.x + distancia * (jugador.mirandoDerecha ? 1 : -1), transform.position.y);
            denyingAttack = true;
            transform.Find("ControladorAtaque").GetComponent<CircleCollider2D>().enabled = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Jugador"), LayerMask.NameToLayer("Enemigo"), true);
            Instantiate(efectoDenyAttack[0], new Vector2(transform.Find("ControladorAtaque").transform.position.x + transform.Find("ControladorAtaque").GetComponent<CircleCollider2D>().radius, transform.Find("ControladorAtaque").transform.position.y + transform.Find("ControladorAtaque").GetComponent<CircleCollider2D>().radius), Quaternion.identity);
            Instantiate(efectoDenyAttack[1], efectoPos, Quaternion.identity);
            tiempoRestanteDenyAttack = TiempoEfectoDenyAttack;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Ataque"))
        {
            if(other.gameObject.GetComponent<ParticulaDmg>() != null)
            {
                RecibirDamage(other.gameObject.GetComponent<ParticulaDmg>().Damage);
            }
        }
    }

}
