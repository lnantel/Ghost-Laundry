using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] sounds;

    private AudioSource source;

    private void Start() {
        source = GetComponent<AudioSource>();
    }

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void PlaySound(Sounds sound, float volume = 1.0f) {
        source.clip = sounds[(int) sound];
        source.volume = SettingsManager.instance.SFXVolume * volume * 0.2f;
        source.PlayOneShot(sounds[(int)sound], SettingsManager.instance.SFXVolume * volume * 0.2f);
    }

    public void PlaySoundAtPosition(Sounds sound, Vector2 position, float volume = 1.0f) {
        AudioSource.PlayClipAtPoint(sounds[(int)sound], position, SettingsManager.instance.SFXVolume * volume);
    }
}
