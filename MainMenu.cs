using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu mainMenu;

    private ConfigControles configuracionControles;
    [SerializeField] private AudioClip[] soundsFX;
    [SerializeField] private GameObject OptionsMenu;
    private AudioSource audioSRC;
    [SerializeField] public Button[] mainButtons;
    public string UltimoEscenario;
    private void Start()
    {
        audioSRC = GetComponent<AudioSource>();
       // if(CanvasGUI.Instance != null) CanvasGUI.Instance.gameObject.SetActive(false);
        if (PlayerMovimiento.Player != null)
        {
            PlayerMovimiento.Player.gameObject.SetActive(false);
            if (string.IsNullOrEmpty(UltimoEscenario)) UltimoEscenario = "Trial Begin";
            UltimoEscenario = PlayerMovimiento.Player.UltimoEscenario;
        }
        else UltimoEscenario = "Trial Begin";
        if (string.IsNullOrEmpty(UltimoEscenario)) UltimoEscenario = "Trial Begin";
    }
    public void ButtonPlay()
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(UltimoEscenario);
    }
    public void ButtonOptions()
    {
        StartCoroutine(ButtonOptions(true));
    }

    public void ButtonCloseOptionsMenu()
    {
        StartCoroutine(ButtonOptions(false));
    }

    public IEnumerator ButtonOptions(bool state)
    {
        yield return new WaitForSeconds(0.3f);
        OptionsMenu.gameObject.SetActive(state);
        setInteractableButtons(!state);
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
    public void setInteractableButtons(bool state)
    {
        foreach (Button btn in mainButtons)
        {
            btn.interactable = state;
        }
    }
}
