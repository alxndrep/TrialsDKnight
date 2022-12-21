using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Light2D auraLight;
    [SerializeField] private ParticleSystem auraGlow;
    [SerializeField] private ParticleSystem fireParticles;
    public bool checkpointActived;

    void Start()
    {
        checkpointActived = false;
    }
    

    IEnumerator LightFade()
    {
        auraGlow.Play();
        fireParticles.Play();
        auraLight.enabled = true;
        float duration = 1.5f;//time you want it to run
        float interval = 0.1f;//interval time between iterations of while loop
        auraLight.intensity = 1f;
        while (duration >= 0.0f)
        {
            auraLight.intensity += 0.0375f;
            duration -= interval;
            yield return new WaitForSeconds(interval);//the coroutine will wait for 0.1 secs
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!checkpointActived)
            {
                checkpointActived = true;
                collision.gameObject.GetComponent<PlayerMovimiento>().ultimoCheckPoint = transform.position;
                StartCoroutine(LightFade());
            }
        }
    }
}
