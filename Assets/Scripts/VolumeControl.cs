using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Start()
    {
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // Initialize sliders with current volume settings
        bgmSlider.value = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }

    void OnBGMVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
        AudioManager.Instance.SetBGMVolume(volume);
    }

    void OnSFXVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        AudioManager.Instance.SetSFXVolume(volume);
    }
}
