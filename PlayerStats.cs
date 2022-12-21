using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum stats
    {
        fuerza,
        destreza,
        constitucion,
        inteligencia,
        agilidad,
    }

    public static PlayerStats Stats;

    [Header("Stats Principales")]
    [SerializeField] public int nivel;
    public double experienciaActual;
    public double experienciaSgteNivel;
    [SerializeField] public float fuerza;
    [HideInInspector] public float fuerzaMOD;
    [SerializeField] public float destreza;
    [HideInInspector] public float destrezaMOD;
    [SerializeField] public float constitucion;
    [HideInInspector] public float constitucionMOD;
    [SerializeField] public float inteligencia;
    [HideInInspector] public float inteligenciaMOD;
    [SerializeField] public float agilidad;
    [HideInInspector] public float agilidadMOD;
    [SerializeField] public int puntosHabilidad;
    [SerializeField] public BarraStats barraExperiencia;

    [Header("Componentes")]
    [SerializeField] private GameObject efectoLevelUp;


    [Header("Habilidades y Poderes")]
    [SerializeField] GameObject[] EfectosPoderes;

    private void Awake()
    {
        if (PlayerStats.Stats == null)
        {
            PlayerStats.Stats = this;
        }
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        barraExperiencia = GameObject.Find("BarraExperiencia").GetComponent<BarraStats>();
        experienciaSgteNivel = (0.75f * (nivel * nivel)) + (40 * nivel) + 139.75;
        barraExperiencia.InicializarBarraDeStats((float)experienciaSgteNivel, (float) experienciaActual);
    }
    public void AgregarExperiencia(double expGanada)
    {
        experienciaActual += expGanada;
        barraExperiencia.CambiarStatActual((float)experienciaActual);
        StartCoroutine(ProcesarSubirNivel());
    }
    IEnumerator ProcesarSubirNivel()
    {
        while (experienciaActual >= experienciaSgteNivel)
        {
            nivel++;
            PlayerSFX.PSFX.playSoundFX(PlayerSFX.PSFX.level_up);
            Instantiate(efectoLevelUp, new Vector2(transform.position.x, transform.position.y - 0.45f), Quaternion.identity);
            puntosHabilidad += 2;
            experienciaSgteNivel += (nivel * nivel * nivel) + (335 * nivel) + 339.75;
            barraExperiencia.CambiarStatMaximo((float)experienciaSgteNivel);
            PlayerCombateCuerpo.Player.CurarHP(PlayerCombateCuerpo.Player.MaxHP_Jugador);
            PlayerCombateCuerpo.Player.CurarManaSoul(PlayerCombateCuerpo.Player.MaxSoulMana_Jugador);
            yield return new WaitForSeconds(1.5f);
        }
    }
    private float calcularSumaUpgradeArmorSet(Inventario inventario, stats upgrade)
    {
        float calculo = 0;
        foreach (Item item in inventario.GetListaItem())
        {
            if (item.equipado)
            {
                for (int i = 0; i < item.atributos.Length; i++)
                {
                    if (item.atributos[i].Key == upgrade)
                    {
                        calculo += item.atributos[i].Value;
                    }
                }
            }
        }
        return calculo;
    }

    public void ModificarPuntoHabilidad(stats tipostat, int cantidad)
    {
        switch (tipostat)
        {
            case stats.fuerza:
                puntosHabilidad -= cantidad;
                fuerza++;
                break;
            case stats.destreza:
                puntosHabilidad -= cantidad;
                destreza++;
                break;
            case stats.constitucion:
                puntosHabilidad -= cantidad;
                constitucion++;
                break;
            case stats.inteligencia:
                puntosHabilidad -= cantidad;
                inteligencia++;
                break;
            case stats.agilidad:
                puntosHabilidad -= cantidad;
                agilidad++;
                break;
        }
    }

    public void ActualizarStatsCombate()
    {
        fuerzaMOD = fuerza + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, stats.fuerza);
        destrezaMOD = destreza + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, stats.destreza);
        constitucionMOD = constitucion + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, stats.constitucion);
        inteligenciaMOD = inteligencia + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, stats.inteligencia);
        agilidadMOD = agilidad + calcularSumaUpgradeArmorSet(PlayerMovimiento.Player.inventarioPlayer, stats.agilidad);
    }
}
