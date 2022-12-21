using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArmorSetSlot : MonoBehaviour, IDropHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] public Item.ItemType tipoItem;
    [SerializeField] public Item.ArmorType tipoArmor;
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    public int slotItemEquipado;
    [SerializeField][Range(0,8)] public int equipadoPos;
    public bool equipado;
    private CanvasGroup canvasGroup;
    private Vector2 posOriginal;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        posOriginal = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (equipado)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if(eventData.pointerDrag.GetComponent<DragDrop>() != null)
            {
                Item itemDroped = eventData.pointerDrag.GetComponent<Item>().GetItem();
                bool mismoTipoArmor = false;
                if (itemDroped.itemType == Item.ItemType.Armor)
                {
                    if (itemDroped.armorType == tipoArmor) mismoTipoArmor = true;
                }
                else if(itemDroped.armorType == Item.ArmorType.None) mismoTipoArmor = true;
                if (tipoItem == itemDroped.itemType & !itemDroped.equipado & mismoTipoArmor)
                {
                    foreach (Item itemBusqueda in UI_Inventory.UI_Inventario.inventario.listaItem)
                    {
                        if (itemBusqueda.SlotPos == itemDroped.SlotPos)
                        {
                            itemBusqueda.equipado = true;
                            itemBusqueda.equipadoPos = equipadoPos;
                        }
                        if (itemBusqueda.SlotPos == slotItemEquipado & equipado)
                        {
                            itemBusqueda.equipado = false;
                            itemBusqueda.equipadoPos = -1;

                        }
                    }
                    transform.Find("imageItem").GetComponent<Image>().sprite = itemDroped.GetSprite();
                    transform.Find("imageItem").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    itemDroped.equipado = true;
                    itemDroped.equipadoPos = equipadoPos;
                    equipado = true;
                    slotItemEquipado = itemDroped.SlotPos;
                    UI_Inventory.UI_Inventario.ActualizarInventarioItems();
                    PlayerCombateCuerpo.Player.ActualizarStatsCombate();
                    UI_PanelStats.PanelStats.SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                                    PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                                    PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                                    PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                                    PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
                    PlayerMovimiento.Player.playerSFX.playSoundFX(PlayerMovimiento.Player.playerSFX.guiActions, 0);
                }
            }
            else if(eventData.pointerDrag.GetComponent<ArmorSetSlot>() != null)
            {
                if(eventData.pointerDrag.GetComponent<ArmorSetSlot>().tipoItem == tipoItem & eventData.pointerDrag.GetComponent<ArmorSetSlot>().tipoArmor == tipoArmor)
                {
                    if (eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipado)
                    {
                        if (!equipado & eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipado)
                        {
                            eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipado = false;

                            foreach(Item itemBusqueda in UI_Inventory.UI_Inventario.inventario.listaItem)
                            {
                                if (itemBusqueda.equipadoPos == eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipadoPos) itemBusqueda.equipadoPos = equipadoPos;
                            }

                            transform.Find("imageItem").GetComponent<Image>().sprite = eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().sprite;
                            slotItemEquipado = eventData.pointerDrag.GetComponent<ArmorSetSlot>().slotItemEquipado;
                            transform.Find("imageItem").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                            equipado = true;

                            eventData.pointerDrag.GetComponent<ArmorSetSlot>().slotItemEquipado = -1;
                            eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().sprite = null;
                            eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().color = new Color(0, 0, 0, 0);
                        }
                        else if (equipado & eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipado)
                        {
                            int auxSlot = eventData.pointerDrag.GetComponent<ArmorSetSlot>().slotItemEquipado;
                            eventData.pointerDrag.GetComponent<ArmorSetSlot>().slotItemEquipado = slotItemEquipado;
                            slotItemEquipado = auxSlot;

                            int equipadoAux = eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipadoPos;
                            foreach (Item itemBusqueda in UI_Inventory.UI_Inventario.inventario.listaItem)
                            {
                                if (itemBusqueda.equipadoPos == equipadoAux){
                                    itemBusqueda.equipadoPos = equipadoPos;
                                    continue;
                                }
                                if (itemBusqueda.equipadoPos == equipadoPos) itemBusqueda.equipadoPos = equipadoAux;
                            }

                            Sprite auxSprite = eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().sprite;
                            eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().sprite = transform.Find("imageItem").GetComponent<Image>().sprite;
                            transform.Find("imageItem").GetComponent<Image>().sprite = auxSprite;
                        }
                        UI_Inventory.UI_Inventario.ActualizarInventarioItems();
                        PlayerCombateCuerpo.Player.ActualizarStatsCombate();
                        UI_PanelStats.PanelStats.SetPanelStats(PlayerStats.Stats.nivel, PlayerStats.Stats.puntosHabilidad, PlayerStats.Stats.fuerza, PlayerStats.Stats.fuerzaMOD, PlayerStats.Stats.inteligencia, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.destreza, PlayerStats.Stats.destrezaMOD, PlayerStats.Stats.constitucion, PlayerStats.Stats.constitucionMOD, PlayerStats.Stats.agilidad, PlayerStats.Stats.agilidadMOD,
                                        PlayerCombateCuerpo.Player.HP_Jugador, PlayerCombateCuerpo.Player.MaxHP_Jugador, PlayerCombateCuerpo.Player.SoulMana_Jugador, PlayerCombateCuerpo.Player.MaxSoulMana_Jugador,
                                        PlayerCombateCuerpo.Player.Armor_Jugador, PlayerCombateCuerpo.Player.MaxArmor_Jugador, PlayerCombateCuerpo.Player.damageAtaque, PlayerStats.Stats.inteligenciaMOD,
                                        PlayerStats.Stats.constitucionMOD * 0.075f, PlayerStats.Stats.inteligenciaMOD * 0.1f, PlayerCombateCuerpo.Player.criticalChance);
                        PlayerMovimiento.Player.playerSFX.playSoundFX(PlayerMovimiento.Player.playerSFX.guiActions, 1);
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.position = posOriginal;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posOriginal = rectTransform.position;
        if (equipado)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

    }
}
