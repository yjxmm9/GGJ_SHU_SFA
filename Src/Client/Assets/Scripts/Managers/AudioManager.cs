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
    public AudioSource deathSource;
    public AudioClip die, rise, blossom, win, final;


    public AudioSource DieSource;
    public AudioClip DieSound;
    public void DieSFX()
    {
        musicSource.volume = 0;
        DieSource.PlayOneShot(DieSound);
    }

    public void EndSfx()
    {
        musicSource.volume = 0;
        deathSource.Stop();
        deathSource.PlayOneShot(win);
    }

    public void StartRise()
    {
        deathSource.Stop();

        deathSource.PlayOneShot(rise);
    }

    public void Blossom()
    {
        deathSource.Stop();

        deathSource.PlayOneShot(blossom);
    }

    public void BlossomEnd()
    {
        deathSource.Stop();

        deathSource.PlayOneShot(final);
    }



    public void Replay()
    {
        StartCoroutine(ShowDeathUIRoutine());
    }

    IEnumerator ShowDeathUIRoutine()
    {
        musicSource.volume = 0;
        float lerp = 0;
        while (lerp < 0.35f)
        {
            musicSource.volume = lerp;
            lerp += Time.deltaTime;
            yield return null;
        }

    }

    public void MuteAudio(bool mute)
    {
        growingSource.mute = mute;
        //noteSource.mute = mute;
        drinkSource.mute = mute;
        deathSource.mute = mute;
        hurtSource.mute = mute;
    }

    public void MuteMusic(bool mute)
    {
        musicSource.mute = mute;
    }
    public AudioSource hurtSource;

    public void HurtSfx()
    {
        hurtSource.volume = 1f;
        hurtSource.Play();
    }

    private void Update()
    {
        //hurtSource.volume = Mathf.Min(1f, gM.rootControler.poisonned);
    }
}
