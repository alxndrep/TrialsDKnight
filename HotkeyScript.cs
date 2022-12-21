using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HotkeyScript : MonoBehaviour, IDropHandler
{
    public Item referenciaItem;
    [SerializeField] private Image spriteItem;
    public bool equipado;
    [SerializeField] private TextMeshProUGUI textoCantidad;
    [SerializeField] [Range(1, 2)] public int Hotkey;

    public void Start()
    {
        spriteItem = transform.Find("Sprite").GetComponent<Image>();
        OnSceneLoad();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<DragDrop>() != null)
            {
                if (eventData.pointerDrag.GetComponent<Item>().itemType == Item.ItemType.Consumible)
                {
                    foreach(Item item in PlayerMovimiento.Player.inventarioPlayer.GetListaItem())
                    {
                        if(item.slotPos == eventData.pointerDrag.GetComponent<Item>().slotPos)
                        {
                            if(item.equipadoHotkey > 0)
                            {
                                PlayerInventario.Instance.Hotkeys[item.equipadoHotkey - 1].GetComponent<HotkeyScript>().SetNoneItemHotkey();
                            }
                            referenciaItem = eventData.pointerDrag.GetComponent<Item>().GetItem();
                            spriteItem.sprite = item.spriteItem;
                            spriteItem.color = new Color(1, 1, 1, 1);
                            textoCantidad.text = item.cantidad.ToString();
                            item.equipadoHotkey = Hotkey;
                            equipado = true;
                            UI_Inventory.UI_Inventario.ActualizarInventarioItems();
                            break;
                        }
                    }

                }
            }
        }
    }

    public void CambiarCantidadItem()
    {
        bool encontrado = false;
        foreach (Item item in PlayerMovimiento.Player.inventarioPlayer.listaItem)
        {
            if (item.equipadoHotkey == Hotkey)
            {
                textoCantidad.text = item.cantidad.ToString();
                encontrado = true;
                break;
            }
        }
        if (!encontrado) SetNoneItemHotkey();
    }
    public void SetNoneItemHotkey()
    {
        referenciaItem = null;
        transform.Find("Sprite").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        equipado = false;
        textoCantidad.text = "";
    }

    public void OnSceneLoad()
    {
        foreach(Item item in PlayerMovimiento.Player.inventarioPlayer.listaItem)
        {
            if(item.equipadoHotkey == Hotkey)
            {
                referenciaItem = item;
                spriteItem.sprite = item.spriteItem;
                spriteItem.color = new Color(1, 1, 1, 1);
                textoCantidad.text = item.cantidad.ToString();
                referenciaItem.equipadoHotkey = Hotkey;
                equipado = true;
                break;
            }
        }
    }

}
