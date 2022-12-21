using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Self_Spell : MonoBehaviour
{
    [SerializeField] private AudioClip soundFX;
    [SerializeField] public float costoSoulMana;
    [SerializeField] public float enfriamientoCD;
    [SerializeField] public Item.Consumible<Item.ConsumibleType, float>[] efectoSpell;
    [SerializeField] public float duracionSpell;
    [SerializeField] public Sprite spriteSpell;
    private AudioSource audioSRC;
    private Animator animador;
    void Start()
    {
        animador = GetComponent<Animator>();
        audioSRC = GetComponent<AudioSource>();
        EjecutarSpell();
        PlaySound();
    }

    public void EjecutarSpell()
    {
        for (int i = 0; i < efectoSpell.Length; i++)
        {
            StartCoroutine(EfectoSpell(duracionSpell, efectoSpell[i].Stat, efectoSpell[i].Valor));
        }
    }
    IEnumerator EfectoSpell(float duracion, Item.ConsumibleType stat, float valor)
    {
        float valorAnterior = 0;
        switch (stat)
        {
            case Item.ConsumibleType.agilidad:
                valorAnterior = PlayerStats.Stats.agilidadMOD;
                PlayerStats.Stats.agilidadMOD += valor;
                break;
            case Item.ConsumibleType.constitucion:
                valorAnterior = PlayerStats.Stats.constitucionMOD;
                PlayerStats.Stats.constitucionMOD += valor;
                break;
            case Item.ConsumibleType.destreza:
                valorAnterior = PlayerStats.Stats.destrezaMOD;
                PlayerStats.Stats.destrezaMOD += valor;
                break;
            case Item.ConsumibleType.fuerza:
                valorAnterior = PlayerStats.Stats.fuerzaMOD;
                PlayerStats.Stats.fuerzaMOD += valor;
                break;
            case Item.ConsumibleType.hp:
                PlayerCombateCuerpo.Player.CurarHP(valor + PlayerStats.Stats.inteligenciaMOD * 0.33f);
                break;
            case Item.ConsumibleType.inteligencia:
                valorAnterior = PlayerStats.Stats.inteligenciaMOD;
                PlayerStats.Stats.inteligenciaMOD += valor;
                break;
            case Item.ConsumibleType.mana:
                PlayerCombateCuerpo.Player.CurarManaSoul(valor + PlayerStats.Stats.inteligenciaMOD * 0.33f);
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
        yield return new WaitForSeconds(duracion);
        // terminar efecto spell
        switch (stat)
        {
            case Item.ConsumibleType.agilidad:
                PlayerStats.Stats.agilidadMOD = valorAnterior;
                break;
            case Item.ConsumibleType.constitucion:
                PlayerStats.Stats.constitucionMOD = valorAnterior;
                break;
            case Item.ConsumibleType.destreza:
                PlayerStats.Stats.destrezaMOD = valorAnterior;
                break;
            case Item.ConsumibleType.fuerza:
                PlayerStats.Stats.fuerzaMOD = valorAnterior;
                break;
            case Item.ConsumibleType.inteligencia:
                PlayerStats.Stats.inteligenciaMOD = valorAnterior;
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
    }

    public void PlaySound()
    {
        audioSRC.PlayOneShot(soundFX);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
