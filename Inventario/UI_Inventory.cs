using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public static UI_Inventory UI_Inventario;
    public Inventario inventario;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    [SerializeField] private ArmorSetSlot[] ArmorSlots;

    private void Awake()
    {
        if (UI_Inventory.UI_Inventario == null)
        {
            UI_Inventory.UI_Inventario = this;
        }
        else Destroy(this.gameObject);
        itemSlotContainer = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").Find("InventarioHUD").Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }
    public void Start()
    {
        UI_Inventario = this;
        itemSlotContainer = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").Find("InventarioHUD").Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        for(int i = 0; i< itemSlotContainer.childCount; i++)
        {
            ArmorSlots[i] = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").Find("InventarioHUD").Find("ArmorSetContainer").GetChild(i).GetComponent<ArmorSetSlot>();
        }
        EquipInventoryItems();

    }

    public void SetInventario(Inventario inventario)
    {
        this.inventario = inventario;
        inventario.OnItemListChanged += Inventario_OnItemListChanged;
        ActualizarInventarioItems();
    }
     
    private void Inventario_OnItemListChanged(object sender, EventArgs e)
    {
        ActualizarInventarioItems();
    }
    public void ActualizarInventarioItems()
    {
        itemSlotContainer = GameObject.Find("CanvasGUI").transform.Find("PlayerInventario").Find("InventarioHUD").Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        int posicionItem = 0;
        float itemSlotCellSize = 60f;
        foreach(Item item in inventario.GetListaItem())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);

            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            itemSlotRectTransform.GetComponent<Item>().SetItem(item.itemType, item.cantidad, item.nombreItem, item.acumulable, item.spriteItem, item.equipado, item.armorType, item.descripcion);
            itemSlotRectTransform.GetComponent<Item>().SlotPos = posicionItem;
            itemSlotRectTransform.GetComponent<Item>().equipadoPos = item.equipadoPos;
            itemSlotRectTransform.GetComponent<Item>().equipadoHotkey = item.equipadoHotkey;
            image.sprite = item.GetSprite();
            item.SlotPos = posicionItem;
            TextMeshProUGUI cantidadTexto = itemSlotRectTransform.Find("cantidadTexto").GetComponent<TextMeshProUGUI>();

            itemSlotRectTransform.Find("equiped").gameObject.SetActive(item.equipado);

            if(item.cantidad > 1) cantidadTexto.SetText("x" + item.cantidad);
            else cantidadTexto.SetText("");

            x++;
            posicionItem++;
            if(x > 5)
            {
                x = 0;
                y++;
            }
        }
        PlayerInventario.Instance.Hotkeys[0].GetComponent<HotkeyScript>().CambiarCantidadItem();
        PlayerInventario.Instance.Hotkeys[1].GetComponent<HotkeyScript>().CambiarCantidadItem();
    }


    public void CloseInventory()
    {
        PlayerMovimiento.Player.playerSFX.playSoundFX(PlayerMovimiento.Player.playerSFX.guiActions, 0);
        PlayerInventario.Instance.activadoInventario = false;
        PlayerInventario.Instance.inventario.SetActive(false);
        Time.timeScale = 1f;
        ToggleStats(false);
        ToggleSkills(false);
    }
    public void ToggleStats()
    {
        MovePanelSkills(false);
        MovePanelStats(!PlayerInventario.Instance.activadoStats);
        MoveInventarioHUD(PlayerInventario.Instance.activadoStats, 0);
    }
    public void ToggleStats(bool estado)
    {
        MovePanelStats(estado);
        MoveInventarioHUD(estado, 0);
    }
    public void ToggleSkills()
    {
        MovePanelStats(false);
        MovePanelSkills(!PlayerInventario.Instance.activadoSkills);
        MoveInventarioHUD(PlayerInventario.Instance.activadoSkills, 1);
    }
    public void ToggleSkills(bool estado)
    {
        MovePanelSkills(estado);
        MoveInventarioHUD(estado, 0);
    }
    public void MoveInventarioHUD(bool estado, int pos)
    {
        if(!estado) LeanTween.moveX(PlayerInventario.Instance.InventarioHUD.GetComponent<RectTransform>(), 422, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
        else if(estado & pos == 0) LeanTween.moveX(PlayerInventario.Instance.InventarioHUD.GetComponent<RectTransform>(), 683, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
        else if (estado & pos == 1) LeanTween.moveX(PlayerInventario.Instance.InventarioHUD.GetComponent<RectTransform>(), 158, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
    }
    public void MovePanelStats(bool estado)
    {
        PlayerInventario.Instance.activadoStats = estado;
        if (estado)
        {
            PlayerInventario.Instance.PanelStats.GetComponent<CanvasGroup>().blocksRaycasts = true;
            LeanTween.moveX(PlayerInventario.Instance.PanelStats.GetComponent<RectTransform>(), 81, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.moveX(PlayerInventario.Instance.PanelStats.GetComponent<RectTransform>(), 487, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
            PlayerInventario.Instance.PanelStats.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
    public void MovePanelSkills(bool estado)
    {
        PlayerInventario.Instance.activadoSkills = estado;
        if (estado)
        {
            PlayerInventario.Instance.PanelSkills.GetComponent<CanvasGroup>().blocksRaycasts = true;
            LeanTween.moveX(PlayerInventario.Instance.PanelSkills.GetComponent<RectTransform>(), 684, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
        }
        else
        {
            PlayerInventario.Instance.PanelSkills.GetComponent<CanvasGroup>().blocksRaycasts = false;
            LeanTween.moveX(PlayerInventario.Instance.PanelSkills.GetComponent<RectTransform>(), 374, 0.5f).setEase(LeanTweenType.easeOutExpo).setIgnoreTimeScale(true);
        }
    }

    public void SaveAndExit()
    {
        // GUARDO LA INFORMACION
        PlayerInventario.Instance.activadoInventario = false;
        PlayerInventario.Instance.inventario.SetActive(false);
        Time.timeScale = 1f;
        ToggleStats(false);
        ToggleSkills(false);
        if (!SceneManager.GetActiveScene().name.Equals("Tutorial")) PlayerMovimiento.Player.UltimoEscenario = SceneManager.GetActiveScene().name;
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("MainCamera").transform.Find("BlackScreen").gameObject);
        }
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("MainMenu");
    }

    public void EquipInventoryItems()
    {
        Inventario inv = PlayerMovimiento.Player.inventarioPlayer;
        foreach (Item item in inv.listaItem)
        {
            if (!item.equipado) continue;
            else
            {
                int slotEquipado = item.equipadoPos;
                ArmorSlots[slotEquipado].transform.Find("imageItem").GetComponent<Image>().sprite = item.spriteItem;
                ArmorSlots[slotEquipado].transform.Find("imageItem").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                ArmorSlots[slotEquipado].slotItemEquipado = item.SlotPos;
                ArmorSlots[slotEquipado].equipado = true;
            }
        }
    }

}
