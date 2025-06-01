using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoSingleton<VolumeManager>
{
    private Color defaultColor;
    private readonly int colorHash = Shader.PropertyToID("_TintColor");

    [SerializeField] private Volume _volume;
    [SerializeField] private VolumeProfile damagedProfile;
    private VolumeProfile defaultProfile;
    private Coroutine damagedRoutine;

    private void Awake()
    {
        defaultProfile = _volume.profile;
    }

    private void Start()
    {
        defaultColor = new Color(0.7f,0.7f, 0.7f, 1f);
        RenderSettings.skybox.SetColor(colorHash, defaultColor);
    }

    public void SkyChange(bool isRaining)
    {
        if (isRaining)
            RenderSettings.skybox.DOColor(Color.black, colorHash, 3f);
        else
            RenderSettings.skybox.DOColor(defaultColor, colorHash, 3f);
    }

    public void StartDamageVolume(float value)
    {
        damagedRoutine = StartCoroutine(DamageVolumeRoutine(value));
    }

    private IEnumerator DamageVolumeRoutine(float value)
    {
        _volume.profile = damagedProfile;   
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            DOTween.To(() => vignette.intensity.value, f => vignette.intensity.value = f
                , Mathf.Clamp(1 - value, 0f, 0.5f), 0.1f);
        }
        yield return new WaitForSeconds(0.2f);
        DOTween.To(() => vignette.intensity.value, f => vignette.intensity.value = f
            ,0, 0.1f);
    }
}
