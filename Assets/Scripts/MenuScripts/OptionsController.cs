using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour
{
    public AudioMixer soundMixer;
    public AudioMixer musicMixer;
    public void SetSoundsVolume(float volume)
    {
        soundMixer.SetFloat("soundsVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicVolume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
