using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panelDescripcion;
    private CanvasGroup canvasGroup;
    private Vector2 posOriginal;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        posOriginal = rectTransform.position;
        panelDescripcion = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").Find("InventarioHUD").Find("UI").Find("Panel").gameObject;
        canvas = GameObject.Find("CanvasGUI").GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posOriginal = rectTransform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.position = posOriginal;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        panelDescripcion.gameObject.SetActive(true);
        panelDescripcion.transform.Find("nombre").GetComponent<TextMeshProUGUI>().alpha = 1f;
        panelDescripcion.transform.Find("descripcion").GetComponent<TextMeshProUGUI>().alpha = 1f;
        panelDescripcion.transform.Find("atributos").GetComponent<TextMeshProUGUI>().alpha = 1f;
        panelDescripcion.transform.Find("caracteristicas").GetComponent<TextMeshProUGUI>().alpha = 1f;
        panelDescripcion.transform.Find("Divisor").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        panelDescripcion.transform.Find("sprite").GetComponent<Image>().color = new Color(1, 1, 1, 1);

        panelDescripcion.transform.Find("nombre").GetComponent<TextMeshProUGUI>().text = GetComponent<Item>().nombreItem;
        panelDescripcion.transform.Find("descripcion").GetComponent<TextMeshProUGUI>().text = GetComponent<Item>().descripcion;
        panelDescripcion.transform.Find("sprite").GetComponent<Image>().sprite = GetComponent<Item>().GetSprite();

        int posItemLista = 0;
        for (int i = 0; i < PlayerMovimiento.Player.inventarioPlayer.listaItem.Count; i++)
        {
            if(PlayerMovimiento.Player.inventarioPlayer.listaItem[i].SlotPos == GetComponent<Item>().SlotPos)
            {
                posItemLista = i;
            }
        }
        if(PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos.Length > 0)
        {
            string cadenaAtributo = "";
            for (int i = 0; i < PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos.Length; i++)
            {
                switch (PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos[i].Key)
                {
                    case PlayerStats.stats.fuerza:
                        cadenaAtributo += "Strength +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos[i].Value;
                        break;
                    case PlayerStats.stats.destreza:
                        cadenaAtributo += "Dexterity +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos[i].Value;
                        break;
                    case PlayerStats.stats.constitucion:
                        cadenaAtributo += "Constitution +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos[i].Value;
                        break;
                    case PlayerStats.stats.inteligencia:
                        cadenaAtributo += "Intelligence +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos[i].Value;
                        break;
                    case PlayerStats.stats.agilidad:
                        cadenaAtributo += "Agility +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].atributos[i].Value;
                        break;
                }
                if (i == 0 | i == 2) cadenaAtributo += " / ";
                else cadenaAtributo += "\n";

            }
            panelDescripcion.transform.Find("atributos").GetComponent<TextMeshProUGUI>().text = cadenaAtributo;
        }
        if (PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas.Length > 0)
        {
            string cadenaAtributo = "";
            for (int i = 0; i < PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas.Length; i++)
            {
                switch (PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Key)
                {
                    case Item.ItemUpgrades.armor:
                        cadenaAtributo += "Armor +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Value + " / ";
                        break;
                    case Item.ItemUpgrades.ataque:
                        cadenaAtributo += "Attack +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Value + " / ";
                        break;
                    case Item.ItemUpgrades.salto:
                        cadenaAtributo += "\nJump +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Value + " / ";
                        break;
                    case Item.ItemUpgrades.cd:
                        cadenaAtributo += "\nAbility Cooldown -" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Value*100 + "%\n";
                        break;
                    case Item.ItemUpgrades.soulmana:
                        cadenaAtributo += "SoulMana +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Value + " / ";
                        break;
                    case Item.ItemUpgrades.vida:
                        cadenaAtributo += "HP +" + PlayerMovimiento.Player.inventarioPlayer.listaItem[posItemLista].caracteristicas[i].Value + " / ";
                        break;
                }
            }
            panelDescripcion.transform.Find("caracteristicas").GetComponent<TextMeshProUGUI>().text = cadenaAtributo;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CleanPanelDescripcion();
    }

    public void CleanPanelDescripcion()
    {
        panelDescripcion.transform.Find("nombre").GetComponent<TextMeshProUGUI>().alpha = 0f;
        panelDescripcion.transform.Find("descripcion").GetComponent<TextMeshProUGUI>().alpha = 0f;
        panelDescripcion.transform.Find("atributos").GetComponent<TextMeshProUGUI>().alpha = 0f;
        panelDescripcion.transform.Find("caracteristicas").GetComponent<TextMeshProUGUI>().alpha = 0f;
        panelDescripcion.transform.Find("Divisor").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panelDescripcion.transform.Find("sprite").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panelDescripcion.transform.Find("caracteristicas").GetComponent<TextMeshProUGUI>().text = "";
        panelDescripcion.transform.Find("atributos").GetComponent<TextMeshProUGUI>().text = "";
        panelDescripcion.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(GetComponent<Item>().itemType == Item.ItemType.Consumible)
            {
                foreach(Item item in PlayerMovimiento.Player.inventarioPlayer.listaItem)
                {
                    if(GetComponent<Item>().SlotPos == item.SlotPos)
                    {
                        if (item.cantidad == 1) CleanPanelDescripcion();
                        PlayerMovimiento.Player.inventarioPlayer.UseConsumible(item);
                        UI_Inventory.UI_Inventario.ActualizarInventarioItems();
                        PlayerCombateCuerpo.Player.ActualizarStatsCombate();
                        UI_PanelStats.PanelStats.SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                                        PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                                        PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
                        if (item.equipadoHotkey > 0)
                        {
                            switch (item.equipadoHotkey)
                            {
                                case 1:
                                    PlayerInventario.Instance.Hotkeys[0].GetComponent<HotkeyScript>().CambiarCantidadItem();
                                    break;
                                case 2:
                                    PlayerInventario.Instance.Hotkeys[1].GetComponent<HotkeyScript>().CambiarCantidadItem();
                                    break;
                            }
                        }
                        StartCoroutine(DrinkPotionSFX());
                        break;
                    }
                }
                
            }
        }
    }
    public IEnumerator DrinkPotionSFX()
    {
        int rnd = Random.Range(4, 6);
        PlayerSFX.PSFX.playSoundFX(PlayerSFX.PSFX.guiActions, rnd);
        PlayerSFX.PSFX.playSoundFX(PlayerSFX.PSFX.guiActions, 3);
        yield return new WaitForSecondsRealtime(1f);
        //yield return null;
    }

}