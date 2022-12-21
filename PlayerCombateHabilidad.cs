using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombateHabilidad : MonoBehaviour
{
    public static PlayerCombateHabilidad Player;
    [Header("Habilidades")]
    [SerializeField] private GameObject[] habilidadesEquipadas = new GameObject[3];

    public float[] enfriamientoHabilidad = new float[3];
    public float[] enfriamientoHabilidadRestante = new float[3];
    public bool[] CDHabilidad = new bool[3];

    private bool atacando;
    private float atacandoCD;
    [SerializeField] private float CDLanzamientoHabilidad;

    [Header("Componentes")]
    [SerializeField] public Transform controladorAtaque;
    [SerializeField] private GameObject outOfManaFX;
    [SerializeField] public GameObject[] Hotkeys;
    private bool existeOutOfMana;
    private float outOfManaCD;
    [HideInInspector] public PlayerMovimiento jugador;
    private Animator animador;
    private void Awake()
    {
        if (Player == null)
        {
            Player = this;
        }
        else Destroy(this.gameObject);
    }
    public void Start()
    {
        jugador = GetComponent<PlayerMovimiento>();
        animador = GetComponent<Animator>();
        Hotkeys = new GameObject[3];
        Hotkeys[0] = (GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyBtn1").gameObject);
        Hotkeys[1] = (GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyBtn2").gameObject);
        Hotkeys[2] = (GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyBtn3").gameObject);
        for (int i = 0; i < 3; i++)
        {
            if (habilidadesEquipadas[i].GetComponent<Spell>() != null)
            {
                Spell spell = habilidadesEquipadas[i].GetComponent<Spell>();
                EquiparSpell(spell, i);
            }
            else
            {
                Self_Spell spell = habilidadesEquipadas[i].GetComponent<Self_Spell>();
                EquiparSpell(spell, i);
            }
        }
        CalculoCooldownHabilidad();
    }
    private void Update()
    {
        if (Time.timeScale > 0 & jugador.sePuedeMover)
        {
            ProcesarHabilidad1();
            ProcesarHabilidad2();
            ProcesarHabilidad3();
            ProcesarAtacando();
        }

    }

    public void CalculoCooldownHabilidad()
    {
        float cd_temp;
        if (habilidadesEquipadas[0].GetComponent<Spell>()) cd_temp = habilidadesEquipadas[0].GetComponent<Spell>().enfriamientoCD;
        else cd_temp = habilidadesEquipadas[0].GetComponent<Self_Spell>().enfriamientoCD;
        enfriamientoHabilidad[0] = cd_temp - ((PlayerStats.Stats.inteligenciaMOD * 0.15f))/10;

        if (habilidadesEquipadas[1].GetComponent<Spell>()) cd_temp = habilidadesEquipadas[1].GetComponent<Spell>().enfriamientoCD;
        else cd_temp = habilidadesEquipadas[1].GetComponent<Self_Spell>().enfriamientoCD;
        enfriamientoHabilidad[1] = cd_temp - ((PlayerStats.Stats.inteligenciaMOD * 0.15f)) / 10;

        if (habilidadesEquipadas[2].GetComponent<Spell>()) cd_temp = habilidadesEquipadas[2].GetComponent<Spell>().enfriamientoCD;
        else cd_temp = habilidadesEquipadas[2].GetComponent<Self_Spell>().enfriamientoCD;
        enfriamientoHabilidad[2] = cd_temp - ((PlayerStats.Stats.inteligenciaMOD * 0.15f)) / 10;
    }
    public void EquiparSpell(Spell spell, int slotSpell)
    {
        switch (slotSpell)
        {
            case 0:
                habilidadesEquipadas[0] = spell.gameObject;
                enfriamientoHabilidad[0] = spell.enfriamientoCD;
                Hotkeys[0].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().sprite = spell.spriteSpell;
                Hotkeys[0].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().color = Color.white;
                break;
            case 1:
                habilidadesEquipadas[1] = spell.gameObject;
                enfriamientoHabilidad[1] = spell.enfriamientoCD;
                Hotkeys[1].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().sprite = spell.spriteSpell;
                Hotkeys[1].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().color = Color.white;
                break;
            case 2:
                habilidadesEquipadas[2] = spell.gameObject;
                enfriamientoHabilidad[2] = spell.enfriamientoCD;
                Hotkeys[2].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().sprite = spell.spriteSpell;
                Hotkeys[2].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().color = Color.white;
                break;
        }
    }
    public void EquiparSpell(Self_Spell spell, int slotSpell)
    {
        switch (slotSpell)
        {
            case 0:
                habilidadesEquipadas[0] = spell.gameObject;
                enfriamientoHabilidad[0] = spell.enfriamientoCD;
                Hotkeys[0].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().sprite = spell.spriteSpell;
                Hotkeys[0].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().color = Color.white;
                break;
            case 1:
                habilidadesEquipadas[1] = spell.gameObject;
                enfriamientoHabilidad[1] = spell.enfriamientoCD;
                Hotkeys[1].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().sprite = spell.spriteSpell;
                Hotkeys[1].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().color = Color.white;
                break;
            case 2:
                habilidadesEquipadas[2] = spell.gameObject;
                enfriamientoHabilidad[2] = spell.enfriamientoCD;
                Hotkeys[2].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().sprite = spell.spriteSpell;
                Hotkeys[2].transform.Find("Imagen").Find("Sprite").GetComponent<Image>().color = Color.white;
                break;
        }
    }
    private void ProcesarAtacando()
    {
        if (atacando)
        {
            jugador.shadowTrail.Sombras_Skill();
            atacandoCD -= Time.deltaTime;
            if (atacandoCD <= 0) atacando = false;
        }
        if (existeOutOfMana)
        {
            outOfManaCD -= Time.deltaTime;
            if (outOfManaCD <= 0) existeOutOfMana = false;
        }

    }
    private void ProcesarHabilidad1()
    {

        if (Input.GetKeyDown(jugador.Controles["Ability1"]) & !jugador.deslizando & !jugador.esquivando & !jugador.dodgeando & jugador.sePuedeMover & !CDHabilidad[0] & !atacando)
        {
            float costoSoulMana = 0;
            if (habilidadesEquipadas[0].GetComponent<Spell>() != null) costoSoulMana = habilidadesEquipadas[0].GetComponent<Spell>().costoSoulMana;
            else costoSoulMana = habilidadesEquipadas[0].GetComponent<Self_Spell>().costoSoulMana;
            if (PlayerCombateCuerpo.Player.barraDeSoulMana.GetValorActual() >= costoSoulMana)
            {
                Hotkeys[0].GetComponent<EfectoCDHotkeys>().InicializarCD(enfriamientoHabilidad[0]);
                animador.SetTrigger("AtaqueHabilidad");
                CDHabilidad[0] = true;
                atacando = true;
                atacandoCD = CDLanzamientoHabilidad;
                enfriamientoHabilidadRestante[0] = enfriamientoHabilidad[0];
                PlayerCombateCuerpo.Player.SoulMana_Jugador -= costoSoulMana;
                PlayerCombateCuerpo.Player.barraDeSoulMana.CambiarStatActual(PlayerCombateCuerpo.Player.SoulMana_Jugador);
                StartCoroutine(DelayLanzamientoHabilidad(0));
            }
            else if(!existeOutOfMana)
            {
                existeOutOfMana = true;
                outOfManaCD = 1.4f;
                Instantiate(outOfManaFX, transform.position, Quaternion.identity);
            }

        }
        if (CDHabilidad[0])
        {
            enfriamientoHabilidadRestante[0] -= Time.deltaTime;
            Hotkeys[0].GetComponent<EfectoCDHotkeys>().CambiarCDActual(enfriamientoHabilidadRestante[0]);
            if(enfriamientoHabilidadRestante[0] <= 0)
            {
                CDHabilidad[0] = false;
            }
        }


    }
    private void ProcesarHabilidad2()
    {
        if (Input.GetKeyDown(jugador.Controles["Ability2"]) & !jugador.deslizando & !jugador.esquivando & !jugador.dodgeando & jugador.sePuedeMover & !CDHabilidad[1] & !atacando)
        {
            float costoSoulMana = 0;
            if (habilidadesEquipadas[1].GetComponent<Spell>() != null) costoSoulMana = habilidadesEquipadas[1].GetComponent<Spell>().costoSoulMana;
            else costoSoulMana = habilidadesEquipadas[1].GetComponent<Self_Spell>().costoSoulMana;
            if (PlayerCombateCuerpo.Player.barraDeSoulMana.GetValorActual() >= costoSoulMana)
            {
                Hotkeys[1].GetComponent<EfectoCDHotkeys>().InicializarCD(enfriamientoHabilidad[1]);
                animador.SetTrigger("AtaqueHabilidad");
                CDHabilidad[1] = true;
                atacando = true;
                atacandoCD = CDLanzamientoHabilidad;
                enfriamientoHabilidadRestante[1] = enfriamientoHabilidad[1];
                PlayerCombateCuerpo.Player.SoulMana_Jugador -= costoSoulMana;
                PlayerCombateCuerpo.Player.barraDeSoulMana.CambiarStatActual(PlayerCombateCuerpo.Player.SoulMana_Jugador);
                StartCoroutine(DelayLanzamientoHabilidad(1));
            }
            else if (!existeOutOfMana)
            {
                existeOutOfMana = true;
                outOfManaCD = 1.4f;
                Instantiate(outOfManaFX, transform.position, Quaternion.identity);
            }
        }
        if (CDHabilidad[1])
        {
            enfriamientoHabilidadRestante[1] -= Time.deltaTime;
            Hotkeys[1].GetComponent<EfectoCDHotkeys>().CambiarCDActual(enfriamientoHabilidadRestante[1]);
            if (enfriamientoHabilidadRestante[1] <= 0)
            {
                CDHabilidad[1] = false;
            }
        }
    }
    private void ProcesarHabilidad3()
    {
        if (Input.GetKeyDown(jugador.Controles["Ability3"]) & !jugador.deslizando & !jugador.esquivando & !jugador.dodgeando & jugador.sePuedeMover & !CDHabilidad[2] & !atacando)
        {
            float costoSoulMana = 0;
            if (habilidadesEquipadas[2].GetComponent<Spell>() != null) costoSoulMana = habilidadesEquipadas[2].GetComponent<Spell>().costoSoulMana;
            else costoSoulMana = habilidadesEquipadas[2].GetComponent<Self_Spell>().costoSoulMana;
            if (PlayerCombateCuerpo.Player.barraDeSoulMana.GetValorActual() >= costoSoulMana)
            {
                Hotkeys[2].GetComponent<EfectoCDHotkeys>().InicializarCD(enfriamientoHabilidad[2]);
                animador.SetTrigger("AtaqueHabilidad");
                CDHabilidad[2] = true;
                atacando = true;
                atacandoCD = CDLanzamientoHabilidad;
                enfriamientoHabilidadRestante[2] = enfriamientoHabilidad[2];
                PlayerCombateCuerpo.Player.SoulMana_Jugador -= costoSoulMana;
                PlayerCombateCuerpo.Player.barraDeSoulMana.CambiarStatActual(PlayerCombateCuerpo.Player.SoulMana_Jugador);
                StartCoroutine(DelayLanzamientoHabilidad(2));
            }
            else if (!existeOutOfMana)
            {
                existeOutOfMana = true;
                outOfManaCD = 1.4f;
                Instantiate(outOfManaFX, transform.position, Quaternion.identity);
            }
        }
        if (CDHabilidad[2])
        {
            enfriamientoHabilidadRestante[2] -= Time.deltaTime;
            Hotkeys[2].GetComponent<EfectoCDHotkeys>().CambiarCDActual(enfriamientoHabilidadRestante[2]);
            if (enfriamientoHabilidadRestante[2] <= 0)
            {
                CDHabilidad[2] = false;
            }
        }
    }

    IEnumerator DelayLanzamientoHabilidad(int habilidad)
    {
        yield return new WaitForSeconds(0);
        if (habilidadesEquipadas[habilidad].GetComponent<Spell>() != null) Instantiate(habilidadesEquipadas[habilidad], controladorAtaque.position, controladorAtaque.rotation);
        else
        {
            GameObject spell = Instantiate(habilidadesEquipadas[habilidad], transform.position, Quaternion.identity);
            spell.transform.parent = transform;
        }
    }
}
