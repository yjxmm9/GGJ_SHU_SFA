using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public GM gM;
    public AudioSource growingSource;


    public void PlayGrowingSFX()
    {
        growingSource.Play();
    }

    public AudioSource drinkSource;
    public AudioClip drinkSound;

    public void PlayDrinkSFX()
    {
        drinkSource.PlayOneShot(drinkSound);
    }

    public AudioSource OrganicSource;
    public AudioClip OrganicSounds;

    public void OrganicSFX()
    {
        OrganicSource.PlayOneShot(OrganicSounds);
    }

    public AudioSource UpGradeSource;
    public AudioClip UpGradeSounds;

    public void UpGradeSFX()
    {
        UpGradeSource.PlayOneShot(UpGradeSounds);
    }

    public AudioSource musicSource;


    public AudioSource PoisonSource;
    public AudioClip PoisonSound;
    public void PoisonSFX()
    {
        PoisonSource.PlayOneShot(PoisonSound);
    }

    public AudioSource DieSource;
    public AudioClip DieSound;
    public void DieSFX()
    {
        musicSource.volume = 0;
        DieSource.PlayOneShot(DieSound);
    }

    public void MusicBack()
    {
        musicSource.volume = 1;
    }


    public AudioSource SoilSource;
    public AudioClip SoilSound;
    public void SoilSFX()
    {
        SoilSource.PlayOneShot(SoilSound);
    }

    public AudioSource VictorSource;
    public AudioClip VictorSound;
    public AudioClip victorMusic;
    public void VictorSFX()
    {
        musicSource.volume = 0;
        VictorSource.PlayOneShot(victorMusic);
        VictorSource.PlayOneShot(VictorSound);
    }















    private void Update()
    {
        //hurtSource.volume = Mathf.Min(1f, gM.rootControler.poisonned);
    }
}
