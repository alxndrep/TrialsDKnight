using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public static PlayerSFX PSFX;

    [SerializeField] AudioSource audioSRC;
    [SerializeField] AudioSource FootStepsSRC;
    [SerializeField] public AudioClip[] hitSFX;
    [SerializeField] public AudioClip[] jumpSFX;
    [SerializeField] public AudioClip[] SlideAndRollsSFX;
    [SerializeField] public AudioClip[] swordHitSFX;
    [SerializeField] public AudioClip[] swordSlashSFX;
    [SerializeField] public AudioClip sword_kill;
    [SerializeField] public AudioClip player_death;
    [SerializeField] public AudioClip level_up;
    [SerializeField] public AudioClip[] footsteps_stonedirt;
    [SerializeField] public AudioClip[] guiActions;

    private void Awake()
    {
        if (PlayerSFX.PSFX == null)
        {
            PlayerSFX.PSFX = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    public void playSoundFX(AudioClip[] soundFX, int numberSFX)
    {
        audioSRC.PlayOneShot(soundFX[numberSFX]);
    }
    public void playSoundFX(AudioClip soundFX)
    {
        audioSRC.PlayOneShot(soundFX);
    }

    public void Footsteps()
    {
        int random = (int)Random.Range(0, 3);
        FootStepsSRC.PlayOneShot(footsteps_stonedirt[random]);
    }
}

