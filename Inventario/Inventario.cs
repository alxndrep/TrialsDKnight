using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventario
{
    public List<Item> listaItem;
    public event EventHandler OnItemListChanged;
    public int capacidadMax = 24;

    public Inventario()
    {
        listaItem = new List<Item>();
    }

    public bool AddItem(Item item)
    {
        if(listaItem.Count < capacidadMax)
        {
            if (item.acumulable)
            {
                bool itemYaEnInventario = false;
                foreach(Item itemInventario in listaItem)
                {
                    if (itemInventario.nombreItem.Equals(item.nombreItem))
                    {
                        itemInventario.cantidad += item.cantidad;
                        itemYaEnInventario = true;
                    }
                }
                if (!itemYaEnInventario)
                {
                    listaItem.Add(item);
                }
            }
            else
            {
                listaItem.Add(item);
            }

            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            return false;
        }

    }

    public List<Item> GetListaItem()
    {
        return listaItem;
    }
    public void UseConsumible(Item item)
    {
        float valor = item.efectoConsumibleStats.Valor;
        switch (item.efectoConsumibleStats.Stat)
        {
            case Item.ConsumibleType.agilidad:
                PlayerStats.Stats.agilidadMOD += valor;
                break;
            case Item.ConsumibleType.constitucion:
                PlayerStats.Stats.constitucionMOD += valor;
                break;
            case Item.ConsumibleType.destreza:
                PlayerStats.Stats.destrezaMOD += valor;
                break;
            case Item.ConsumibleType.fuerza:
                PlayerStats.Stats.fuerzaMOD += valor;
                break;
            case Item.ConsumibleType.hp:
                PlayerCombateCuerpo.Player.CurarHP(valor);
                break;
            case Item.ConsumibleType.inteligencia:
                PlayerStats.Stats.inteligenciaMOD += valor;
                break;
            case Item.ConsumibleType.mana:
                PlayerCombateCuerpo.Player.CurarManaSoul(valor);
                break;
            case Item.ConsumibleType.velocidad:
                //PlayerCombateCuerpo.Player.CurarManaSoul(valor);
                break;
            case Item.ConsumibleType.salto:
               // PlayerCombateCuerpo.Player.CurarManaSoul(valor);
                break;
            case Item.ConsumibleType.cd:
                //PlayerCombateCuerpo.Player.CurarManaSoul(valor);
                break;
        }
        if (item.cantidad == 1)
        {
            listaItem.Remove(item);
            UI_Inventory.UI_Inventario.ActualizarInventarioItems();
        }
        else item.cantidad--;
    }




}
