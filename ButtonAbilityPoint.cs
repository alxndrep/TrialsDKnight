using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonAbilityPoint : MonoBehaviour
{
    [SerializeField] public PlayerStats.stats Stat;
    public void AddAbilityPoint(int value)
    {
        PlayerStats.Stats.ModificarPuntoHabilidad(Stat, value);
    }
}
