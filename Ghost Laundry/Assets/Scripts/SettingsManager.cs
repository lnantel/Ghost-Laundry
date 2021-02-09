using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager instance;

    public float MouseSensitivity;
    public float MusicVolume;
    public float SFXVolume;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void OnEnable() {
        if (PlayerPrefs.HasKey(Settings.MouseSensitivity.ToString()))
            MouseSensitivity = PlayerPrefs.GetFloat(Settings.MouseSensitivity.ToString());
        else
            MouseSensitivity = 0.5f;

        if (PlayerPrefs.HasKey(Settings.MusicVolume.ToString()))
            MusicVolume = PlayerPrefs.GetFloat(Settings.MusicVolume.ToString());
        else
            MusicVolume = 0.5f;

        if (PlayerPrefs.HasKey(Settings.SFXVolume.ToString()))
            SFXVolume = PlayerPrefs.GetFloat(Settings.SFXVolume.ToString());
        else
            SFXVolume = 0.5f;
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat(Settings.MouseSensitivity.ToString(), MouseSensitivity);
        PlayerPrefs.SetFloat(Settings.MusicVolume.ToString(), MusicVolume);
        PlayerPrefs.SetFloat(Settings.SFXVolume.ToString(), SFXVolume);
        PlayerPrefs.Save();
    }

    public void SetMouseSensitivity(float value) {
        MouseSensitivity = value;
    }

    public void SetMusicVolume(float value) {
        MusicVolume = value;
    }

    public void SetSFXVolume(float value) {
        SFXVolume = value;
    }
}

public enum Settings {
    MouseSensitivity,
    MusicVolume,
    SFXVolume
}