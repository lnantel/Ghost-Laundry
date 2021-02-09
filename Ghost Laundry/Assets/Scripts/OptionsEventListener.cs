using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsEventListener : MonoBehaviour
{
    public Slider MouseSensitivitySlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;

    private void OnEnable() {
        MouseSensitivitySlider.value = SettingsManager.instance.MouseSensitivity;
        MusicVolumeSlider.value = SettingsManager.instance.MusicVolume;
        SFXVolumeSlider.value = SettingsManager.instance.SFXVolume;

        MouseSensitivitySlider.onValueChanged.AddListener(SettingsManager.instance.SetMouseSensitivity);
        MusicVolumeSlider.onValueChanged.AddListener(SettingsManager.instance.SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SettingsManager.instance.SetSFXVolume);
    }

    private void OnDisable() {
        MouseSensitivitySlider.onValueChanged.RemoveAllListeners();
        MusicVolumeSlider.onValueChanged.RemoveAllListeners();
        SFXVolumeSlider.onValueChanged.RemoveAllListeners();
    }
}
