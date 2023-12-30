using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] Slider music, sfx;

    private void OnEnable()
    {
        music.value = AudioManager.Instance.AudioConfig.MusicVolume;
        sfx.value = AudioManager.Instance.AudioConfig.MusicVolume;
        music.onValueChanged.AddListener(ChangeMusicVolume);
        sfx.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void OnDisable()
    {
        music.onValueChanged.RemoveListener(ChangeMusicVolume);
        sfx.onValueChanged.RemoveListener(ChangeSFXVolume);
    }

    void ChangeMusicVolume(float value)
    {
        AudioManager.Instance.ChangeMusicVolume(value);
    }

    void ChangeSFXVolume(float value)
    {
        AudioManager.Instance.ChangeSFXVolume(value);
    }

    public void SaveChange()
    {
        AudioManager.Instance.SaveAudioConfig();
    }
}
