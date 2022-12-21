using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public class Diccionario<TKey, TValue>
    {
        public Diccionario()
        {
        }

        public Diccionario(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key;
        public TValue Value;
    }
    [System.Serializable]
    public class Consumible<TKey, TValue>
    {
        public Consumible()
        {
        }

        public Consumible(TKey key, TValue value)
        {
            Stat = key;
            Valor = value;
        }

        public TKey Stat;
        public TValue Valor;
    }
    public enum ConsumibleType
    {
        fuerza,
        destreza,
        constitucion,
        inteligencia,
        agilidad,
        hp,
        mana,
        velocidad,
        salto,
        cd,
    }
    public enum ItemUpgrades
    {
        armor,
        ataque,
        salto,
        cd,
        soulmana,
        vida,
    }
    public enum ItemType
    {
        Weapon,
        Consumible,
        Armor,
        Coin,
        Gems,
        Material,
    }
    public enum ArmorType
    {
        Helmet,
        Necklace,
        Ring,
        Armor,
        Gauntlets,
        Legs,
        Boots,
        None,
    }

    public ItemType itemType;
    public ArmorType armorType;
    [SerializeField] public Diccionario<PlayerStats.stats, float>[] atributos;
    [SerializeField] public Diccionario<ItemUpgrades, float>[] caracteristicas;
    [SerializeField] public Consumible<ConsumibleType, float> efectoConsumibleStats;
    [SerializeField] public string descripcion;
    public int cantidad;
    public string nombreItem;
    public bool acumulable;
    public Sprite spriteItem;
    public bool equipado;
    private TextMeshPro textoCantidad;
    public int slotPos;
    public int equipadoPos;
    public int equipadoHotkey;

    public void Start()
    {
        if (GetComponent<SpriteRenderer>() != null) spriteItem = GetComponent<SpriteRenderer>().sprite;
        if (transform.Find("textoCant") != null)
        {
            textoCantidad = transform.Find("textoCant").GetComponent<TextMeshPro>();
            if (cantidad > 1) textoCantidad.SetText("x" + cantidad);
            else textoCantidad.SetText("");

        }
    }

    public int SlotPos { get => slotPos; set => slotPos = value; }

    public Sprite GetSprite()
    {
        return spriteItem;
    }
    public Item GetItem()
    {
        return this;
    }
    public void SetItem(ItemType itemType, int cantidad, string nombreItem, bool acumulable, Sprite spriteItem, bool equipado, ArmorType armorType, string descripcion)
    {
        this.itemType = itemType;
        this.cantidad = cantidad;
        this.nombreItem = nombreItem;
        this.acumulable = acumulable;
        this.spriteItem = spriteItem;
        this.equipado = equipado;
        this.armorType = armorType;
        this.descripcion = descripcion;
    }

    public void SetItem(ItemType itemType, int cantidad, string nombreItem, bool acumulable, Sprite spriteItem, bool equipado, string descripcion)
    {
        this.itemType = itemType;
        this.cantidad = cantidad;
        this.nombreItem = nombreItem;
        this.acumulable = acumulable;
        this.spriteItem = spriteItem;
        this.equipado = equipado;
        this.armorType = ArmorType.None;
        this.descripcion = descripcion;
    }
}
