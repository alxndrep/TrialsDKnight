using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaMuerte : MonoBehaviour
{
    public static PantallaMuerte DeathScreen;
    private Animator animador;
    private AudioSource audioSRC;
    [SerializeField] private AudioClip[] soundsFX;
    private AudioSource mainMusic;
    private CanvasGroup canvas;
    private GameObject Cinemachine;

    private void Start()
    {
        mainMusic = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        canvas = GetComponent<CanvasGroup>();
        animador = GetComponent<Animator>();
        audioSRC = GetComponent<AudioSource>();
        Cinemachine = GameObject.Find("Cinemachine");
        SceneManager.sceneLoaded += onSceneLoaded;

    }
    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            if (GameObject.Find("EventSystem") != null) mainMusic = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        }
    }
    public void Setup()
    {
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
        animador.SetTrigger("Setup");
        mainMusic.Stop();
        audioSRC.PlayOneShot(soundsFX[Random.Range(1, 3)]);
    }

    public void RestartButton()
    {
        // Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
        // Destroy(GameObject.Find("CanvasGUI").gameObject);
        //foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        //{
        //    Destroy(o);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerCombateCuerpo.Player.Revivir();
        animador.SetTrigger("Volver");
        Cinemachine.SetActive(true);
        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;

    }
    public void MapButton()
    {
        //SceneManager.LoadScene("Map_Scene");
    }
    public void QuitButton()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
        SceneManager.LoadScene("MainMenu");

    }
}
