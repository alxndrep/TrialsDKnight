using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundFX;

    public void ClickSound()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        }
        if (audioSource == null)
        {
            if (GetComponent<Button>().interactable) GetComponent<AudioSource>().PlayOneShot(soundFX[0]);
        }
        else
        {
            if (audioSource.gameObject.activeSelf) if (GetComponent<Button>().interactable) audioSource.PlayOneShot(soundFX[0]);
            else { if (GetComponent<Button>().interactable) GetComponent<AudioSource>().PlayOneShot(soundFX[0]); }
        }
    }
    public void OverSound()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        }
        if (audioSource == null)
        {
            if (GetComponent<Button>().interactable) GetComponent<AudioSource>().PlayOneShot(soundFX[1]);
        }
        else
        {
            if (audioSource.gameObject.activeSelf) if (GetComponent<Button>().interactable) audioSource.PlayOneShot(soundFX[1]);
            else { if (GetComponent<Button>().interactable) GetComponent<AudioSource>().PlayOneShot(soundFX[1]); }
        }
    }
}
