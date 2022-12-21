using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigControles : MonoBehaviour
{
    public static ConfigControles Instance;
    [SerializeField] private int Configuracion;
    [SerializeField] public bool yaInicio = false;

    private void Awake()
    {
        if (ConfigControles.Instance == null)
        {
            ConfigControles.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    public Dictionary<string, KeyCode> getConfiguracion() { return this.controles[Configuracion]; }
    public void setConfiguracion(int config) { Configuracion = config; }

    private Dictionary<int, Dictionary<string, KeyCode>> controles = new Dictionary<int, Dictionary<string, KeyCode>>()
    {
        {0, new Dictionary<string, KeyCode>(){  {"Up", KeyCode.W},
                                                {"Left", KeyCode.A },
                                                {"Down", KeyCode.S },
                                                {"Right", KeyCode.D },
                                                {"Jump", KeyCode.J },
                                                {"Attack", KeyCode.K },
                                                {"Run", KeyCode.L },
                                                {"Inventory", KeyCode.Tab },
                                                {"Ability1", KeyCode.Alpha1 },
                                                {"Ability2", KeyCode.Alpha2 },
                                                {"Ability3", KeyCode.Alpha3 },
                                                {"Hotkey1", KeyCode.Q },
                                                {"Hotkey2", KeyCode.E },
                                                {"Use", KeyCode.F },
                                             }
        }
    };
}
