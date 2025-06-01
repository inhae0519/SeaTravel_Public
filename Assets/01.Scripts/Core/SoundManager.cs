using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource effectRopeSource;
    public float BgmValue => bgmSource.volume;
    public float EffectValue => effectSource.volume;
    
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip audioClip)
    {
        bgmSource.clip = audioClip;
        bgmSource.Play();
    }

    public void RoopEffect(AudioClip audioClip)
    {
        if(audioClip == effectRopeSource.clip)
            return;
        effectRopeSource.clip = audioClip;
        effectRopeSource.Play();
    }

    public void PlayEffect(AudioClip audioClip)
    {
        effectSource.PlayOneShot(audioClip);
    }

    public void EffectSoundSetting(Slider slider)
    {
        effectSource.volume = slider.value;
        effectRopeSource.volume = slider.value;
    }

    public void BgSoundSetting(Slider slider)
    {
        bgmSource.volume = slider.value;
    }
}
