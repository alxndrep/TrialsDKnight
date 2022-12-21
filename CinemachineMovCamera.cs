using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinemachineMovCamera : MonoBehaviour
{
    public static CinemachineMovCamera Instance;
    private CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float tiempoMovimiento;
    private float tiempoMovimientototal;
    private float intensidadInicial;
    private void Awake()
    {
        if (CinemachineMovCamera.Instance == null)
        {
            CinemachineMovCamera.Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
        cinemachineBrain = GetComponent<CinemachineBrain>();
       // cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        //cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
        ChangeCamera();
        //SceneManager.sceneLoaded += onSceneLoaded;
        cinemachineVirtualCamera = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        if (GameObject.FindGameObjectWithTag("Player")) cinemachineVirtualCamera.m_Follow = PlayerMovimiento.Player.gameObject.transform;
        if (GameObject.Find("BordeCamara") != null) GameObject.Find("Cinemachine").GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = GameObject.Find("BordeCamara").GetComponent<PolygonCollider2D>();
    }
    void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(this != null)
        {
            cinemachineVirtualCamera = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
            if(GameObject.FindGameObjectWithTag("Player")) cinemachineVirtualCamera.m_Follow = PlayerMovimiento.Player.gameObject.transform;
            if(GameObject.Find("BordeCamara") != null) GameObject.Find("Cinemachine").GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = GameObject.Find("BordeCamara").GetComponent<PolygonCollider2D>();
        }
    }
    public void MoverCamera(float intensidad, float frecuencia, float tiempo)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensidad;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frecuencia;
        intensidadInicial = intensidad;
        tiempoMovimientototal = tiempo;
        tiempoMovimiento = tiempo;
    }

    private void Update()
    {
        if(tiempoMovimiento > 0)
        {
            tiempoMovimiento -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensidadInicial, 0, 1 - (tiempoMovimiento/tiempoMovimientototal));
        }
    }

    public void ChangeCamera()
    {
        cinemachineVirtualCamera = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        tiempoMovimiento = -1;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }
    public void ChangeCamera(string camera)
    {
        cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<AudioListener>().enabled = false;
        GameObject cm = GameObject.Find(camera);
        cinemachineVirtualCamera = cm.GetComponent<CinemachineVirtualCamera>();
        cm.GetComponent<AudioListener>().enabled = true;
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        tiempoMovimiento = -1;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }
}
