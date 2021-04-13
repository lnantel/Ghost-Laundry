using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] Sounds;

    private AudioSource source;

    private void Start() {
        source = GetComponent<AudioSource>();
    }

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void PlaySound(SoundName soundName, float volume = 1.0f) {
        Sound sound = FindSoundByName(soundName);

        source.pitch = sound.Pitch;
        source.PlayOneShot(sound.Clip, SettingsManager.instance.SFXVolume * sound.Volume * 0.5f);
    }

    public void PlaySoundAtPosition(SoundName soundName, Vector2 position, float volume = 1.0f) {
        Sound sound = FindSoundByName(soundName);

        AudioSource.PlayClipAtPoint(sound.Clip, position, SettingsManager.instance.SFXVolume * volume);
    }

    private Sound FindSoundByName(SoundName name) {
        for(int i = 0; i < Sounds.Length; i++) {
            if (Sounds[i].Name == name) return Sounds[i];
        }
        return null;
    }
}
