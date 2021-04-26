using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsEventListener : MonoBehaviour
{
    public Canvas settingsCanvas;

    public Slider MouseSensitivitySlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public Toggle NoFailModeToggle;

    private void OnEnable() {
        MouseSensitivitySlider.value = SettingsManager.instance.MouseSensitivity;
        MusicVolumeSlider.value = SettingsManager.instance.MusicVolume;
        SFXVolumeSlider.value = SettingsManager.instance.SFXVolume;
        NoFailModeToggle.isOn = SettingsManager.instance.NoFailMode;

        MouseSensitivitySlider.onValueChanged.AddListener(SettingsManager.instance.SetMouseSensitivity);
        MusicVolumeSlider.onValueChanged.AddListener(SettingsManager.instance.SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SettingsManager.instance.SetSFXVolume);
        NoFailModeToggle.onValueChanged.AddListener(SettingsManager.instance.SetNoFailMode);

        GameManager.ShowSettings += OnShowSettings;
        GameManager.HideSettings += OnHideSettings;
    }

    private void OnDisable() {
        MouseSensitivitySlider.onValueChanged.RemoveAllListeners();
        MusicVolumeSlider.onValueChanged.RemoveAllListeners();
        SFXVolumeSlider.onValueChanged.RemoveAllListeners();
        NoFailModeToggle.onValueChanged.RemoveAllListeners();

        GameManager.ShowSettings -= OnShowSettings;
        GameManager.HideSettings -= OnHideSettings;
    }

    private void OnShowSettings() {
        settingsCanvas.gameObject.SetActive(true);
    }

    private void OnHideSettings() {
        settingsCanvas.gameObject.SetActive(false);
    }
}
