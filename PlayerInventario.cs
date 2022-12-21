using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInventario : MonoBehaviour
{
    public static PlayerInventario Instance;
    public bool activadoInventario;
    public GameObject inventario;
    public GameObject PanelStats;
    public GameObject InventarioHUD;
    public GameObject PanelSkills;
    public bool activadoStats;
    public bool activadoSkills;
    public GameObject[] Hotkeys;

    private void Awake()
    {
        if (PlayerInventario.Instance == null)
        {
            PlayerInventario.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        activadoInventario = false;
        inventario.SetActive(false);
        activadoStats = false;
        PanelStats = inventario.transform.Find("PanelStats").gameObject;
        PanelSkills = inventario.transform.Find("PanelSkills").gameObject;
        InventarioHUD = inventario.transform.Find("InventarioHUD").gameObject;
        Hotkeys = new GameObject[2];
        Hotkeys[0] = GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyQ").gameObject;
        Hotkeys[1] = GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyE").gameObject;
    }
    public void StartNewScene()
    {
        activadoInventario = false;
        if(inventario != null) inventario.SetActive(false);
        Time.timeScale = 1f;

        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            inventario = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").gameObject;
            PanelStats = inventario.transform.Find("PanelStats").gameObject;
            PanelSkills = inventario.transform.Find("PanelSkills").gameObject;
            InventarioHUD = inventario.transform.Find("InventarioHUD").gameObject;
            Hotkeys = new GameObject[2];
            Hotkeys[0] = GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyQ").gameObject;
            Hotkeys[1] = GameObject.Find("CanvasGUI").transform.Find("PlayerStats").Find("HotKeys").Find("HotkeyE").gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerMovimiento.Player.Controles["Inventory"]) & !PlayerCombateCuerpo.Player.estaMuerto)
        {
            activadoInventario = !activadoInventario;
            if (activadoInventario)
            {
                inventario.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                inventario.SetActive(false);
                Time.timeScale = 1f;
                UI_Inventory.UI_Inventario.ToggleStats(false);
                UI_Inventory.UI_Inventario.ToggleSkills(false);
            }
            PlayerCombateCuerpo.Player.ActualizarStatsCombate();
            UI_PanelStats.PanelStats.SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                            PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                            PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                            PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                            PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
            PlayerMovimiento.Player.playerSFX.playSoundFX(PlayerMovimiento.Player.playerSFX.guiActions, 0);
        }
        if (Input.GetKeyDown(PlayerMovimiento.Player.Controles["Hotkey1"]))
        {
            if (Hotkeys[0].GetComponent<HotkeyScript>().equipado)
            {
                int hotkeyPos = Hotkeys[0].GetComponent<HotkeyScript>().Hotkey;
                foreach (Item item in PlayerMovimiento.Player.inventarioPlayer.GetListaItem())
                {
                    if (item.equipadoHotkey == hotkeyPos)
                    {
                        PlayerMovimiento.Player.inventarioPlayer.UseConsumible(item);
                        UI_Inventory.UI_Inventario.ActualizarInventarioItems();
                        PlayerCombateCuerpo.Player.ActualizarStatsCombate();
                        UI_PanelStats.PanelStats.SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                                        PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                                        PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
                        Hotkeys[0].GetComponent<HotkeyScript>().CambiarCantidadItem();
                        StartCoroutine(DrinkPotionSFX());
                        break;
                    }
                }
            }

        }
        if (Input.GetKeyDown(PlayerMovimiento.Player.Controles["Hotkey2"]))
        {
            if (Hotkeys[1].GetComponent<HotkeyScript>().equipado)
            {
                int hotkeyPos = Hotkeys[1].GetComponent<HotkeyScript>().Hotkey;
                foreach (Item item in PlayerMovimiento.Player.inventarioPlayer.GetListaItem())
                {
                    if (item.equipadoHotkey == hotkeyPos)
                    {
                        PlayerMovimiento.Player.inventarioPlayer.UseConsumible(item);
                        UI_Inventory.UI_Inventario.ActualizarInventarioItems();
                        PlayerCombateCuerpo.Player.ActualizarStatsCombate();
                        UI_PanelStats.PanelStats.SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                                        PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                                        PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
                        Hotkeys[1].GetComponent<HotkeyScript>().CambiarCantidadItem();
                        StartCoroutine(DrinkPotionSFX());
                        break;
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (PlayerMovimiento.Player.inventarioPlayer.AddItem(collision.GetComponent<Item>().GetItem()))
            {
                PlayerMovimiento.Player.playerSFX.playSoundFX(PlayerMovimiento.Player.playerSFX.guiActions, 2);
                Destroy(collision.gameObject);
            }
        }
    }

    public IEnumerator DrinkPotionSFX()
    {
        int rnd = UnityEngine.Random.Range(4, 6);
        PlayerSFX.PSFX.playSoundFX(PlayerSFX.PSFX.guiActions, rnd);
        PlayerSFX.PSFX.playSoundFX(PlayerSFX.PSFX.guiActions, 3);
        yield return new WaitForSecondsRealtime(1f);
        //yield return null;
    }
}
