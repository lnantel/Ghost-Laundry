using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager instance;

    public AudioMixer audioMixer;

    public float MouseSensitivity { get => m_MouseSensitivity; set => SetMouseSensitivity(value); }
    public float MusicVolume { get => m_MusicVolume; set => SetMusicVolume(value); }
    public float SFXVolume { get => m_SFXVolume; set => SetSFXVolume(value); }
    public bool NoFailMode { get => m_NoFailMode; set => SetNoFailMode(value); }

    private float m_MouseSensitivity;
    private float m_MusicVolume;
    private float m_SFXVolume;
    private bool m_NoFailMode;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        if (PlayerPrefs.HasKey(Settings.MouseSensitivity.ToString()))
            MouseSensitivity = PlayerPrefs.GetFloat(Settings.MouseSensitivity.ToString());
        else
            MouseSensitivity = 0.5f;

        if (PlayerPrefs.HasKey(Settings.MusicVolume.ToString()))
            MusicVolume = PlayerPrefs.GetFloat(Settings.MusicVolume.ToString());
        else
            MusicVolume = 0.25f;

        if (PlayerPrefs.HasKey(Settings.SFXVolume.ToString()))
            SFXVolume = PlayerPrefs.GetFloat(Settings.SFXVolume.ToString());
        else
            SFXVolume = 0.5f;

        if (PlayerPrefs.HasKey(Settings.NoFailMode.ToString()))
            NoFailMode = PlayerPrefs.GetInt(Settings.NoFailMode.ToString()) == 1;
        else
            NoFailMode = false;
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat(Settings.MouseSensitivity.ToString(), MouseSensitivity);
        PlayerPrefs.SetFloat(Settings.MusicVolume.ToString(), MusicVolume);
        PlayerPrefs.SetFloat(Settings.SFXVolume.ToString(), SFXVolume);
        PlayerPrefs.SetInt(Settings.NoFailMode.ToString(), NoFailMode ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMouseSensitivity(float value) {
        m_MouseSensitivity = value;
    }

    public void SetMusicVolume(float value) {
        m_MusicVolume = value;
        audioMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value) {
        m_SFXVolume = value;
        audioMixer.SetFloat("EffectsVol", Mathf.Log10(value) * 20);
    }

    public void SetNoFailMode(bool value) {
        m_NoFailMode = value;
    }
}

public enum Settings {
    MouseSensitivity,
    MusicVolume,
    SFXVolume,
    NoFailMode
}