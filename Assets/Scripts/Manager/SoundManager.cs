using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource[] sfxSource;

    public float BGMVolme { get { return bgmSource.volume; } set { bgmSource.volume = value; } }
    public float SFXVolme { get { return sfxSource[0].volume; } set { for (int i = 0; i < sfxSource.Length; i++) sfxSource[i].volume = value; } }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying == false)
            return;

        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        for (int i = 0; i < sfxSource.Length; i++)
        {
            if (sfxSource[i].isPlaying)
                continue;

            sfxSource[i].PlayOneShot(clip);
            break;
        }
    }

    public void StopSFX()
    {
        for (int i = 0; i < sfxSource.Length; i++)
        {
            if (sfxSource[i].isPlaying == false)
                continue;

            sfxSource[i].Stop();
        }
    }

    public void FadeOutSFX()
    {
        StartCoroutine(StopSFXRoutine());
    }

    IEnumerator StopSFXRoutine()
    {
        float rate = 0;
        float startVol = SFXVolme;
        while (rate < 0.5f)
        {
            rate += Time.deltaTime;
            float curVol = Mathf.Lerp(startVol, 0, rate * 2);
            SFXVolme = curVol;
            yield return null;
        }
    }
}
