using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PanelStats : MonoBehaviour
{

    public static UI_PanelStats PanelStats;
    public CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI fuerza;
    [SerializeField] private TextMeshProUGUI inteligencia;
    [SerializeField] private TextMeshProUGUI destreza;
    [SerializeField] private TextMeshProUGUI constitucion;
    [SerializeField] private TextMeshProUGUI agilidad;
    [SerializeField] private TextMeshProUGUI hitpoints;
    [SerializeField] private TextMeshProUGUI soulmana;
    [SerializeField] private TextMeshProUGUI armorshield;
    [SerializeField] private TextMeshProUGUI attackdmg;
    [SerializeField] private TextMeshProUGUI magicdmg;
    [SerializeField] private TextMeshProUGUI hpregen;
    [SerializeField] private TextMeshProUGUI soulmanaregen;
    [SerializeField] private TextMeshProUGUI criticalChance;
    [SerializeField] private TextMeshProUGUI puntosHabilidad;
    [SerializeField] private GameObject[] botonesStats;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Awake()
    {
        if (UI_PanelStats.PanelStats == null)
        {
            UI_PanelStats.PanelStats = this;
        }
        else Destroy(this.gameObject);
    }
    public void SetPanelStats(float lvl, int puntoshabilidad, float strength, float strength2, float intelligence, float intelligence2, float dexterity, float dexterity2, float constitution, float constitution2, float agility, float agility2, float hp, float maxhp, float soul, float maxsoul, float armor, float maxarmor, float attackdmg, float magicdmg, float hpreg, float manareg, float critchance)
    {
        level.text = "" + lvl;
        puntosHabilidad.text = "Ability Points: " + puntoshabilidad;
        fuerza.text = "" + strength + " (" + strength2 + ")";
        inteligencia.text = "" + intelligence + " (" + intelligence2 + ")"; ;
        destreza.text = "" + dexterity + " (" + dexterity2 + ")"; ;
        constitucion.text = "" + constitution + " (" + constitution2 + ")"; ;
        agilidad.text = "" + agility + " (" + agility2 + ")"; ;
        hitpoints.text = (int)hp + "/" + (int)maxhp;
        soulmana.text = (int)soul + "/" + (int)maxsoul;
        armorshield.text = (int)armor + "/" + (int)maxarmor;
        this.attackdmg.text = "" + attackdmg;
        this.magicdmg.text = magicdmg + " bonus";
        hpregen.text = string.Format("{0:0.0}",hpreg) + " p/second";
        soulmanaregen.text = string.Format("{0:0.0}", manareg) + " p/second";
        criticalChance.text = string.Format("{0:0.0}", critchance) + "%";
    }
    private void Update()
    {
        if(PlayerStats.Stats.puntosHabilidad > 0)
        {
            foreach(GameObject boton in botonesStats)
            {
                boton.GetComponent<CanvasGroup>().alpha = 1f;
                boton.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else
        {
            foreach (GameObject boton in botonesStats)
            {
                boton.GetComponent<CanvasGroup>().alpha = 0f;
                boton.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        puntosHabilidad.text = "Ability Points: " + PlayerStats.Stats.puntosHabilidad;
    }
    public void ConfirmPuntosHabilidad()
    {
        PlayerCombateCuerpo.Player.ActualizarStatsCombate();
        SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                        PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                        PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                        PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                        PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
    }


}
