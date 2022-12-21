using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropBag : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null & eventData.pointerDrag.GetComponent<ArmorSetSlot>() != null)
        {
            if (eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipado)
            {
                int slotEquipado = eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipadoPos;
                foreach (Item itemBusqueda in UI_Inventory.UI_Inventario.inventario.listaItem)
                {
                    if (itemBusqueda.equipadoPos == slotEquipado)
                    {
                        itemBusqueda.equipado = false;
                        itemBusqueda.equipadoPos = -99;
                    }
                }
                eventData.pointerDrag.GetComponent<ArmorSetSlot>().equipado = false;
                eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().sprite = null;
                eventData.pointerDrag.transform.Find("imageItem").GetComponent<Image>().color = new Color(0,0,0,0);
                PlayerMovimiento.Player.playerSFX.playSoundFX(PlayerMovimiento.Player.playerSFX.guiActions, 1);
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
    }
}
