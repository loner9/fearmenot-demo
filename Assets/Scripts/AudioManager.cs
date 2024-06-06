using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioMixer mainAudioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBGMVolume(float volume)
    {
        mainAudioMixer.SetFloat("BGMVolume", volume == 0 ? -80 : Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mainAudioMixer.SetFloat("SFXVolume", volume == 0 ? -80 : Mathf.Log10(volume) * 20);
    }
}
