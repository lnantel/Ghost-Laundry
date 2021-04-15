using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public Sound[] Sounds;

    private AudioPoolManager pool;

    private AudioSource source;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        source = GetComponent<AudioSource>();
        pool = GetComponentInChildren<AudioPoolManager>();
    }

    public void PlaySound(SoundName soundName) {
        Sound sound = FindSoundByName(soundName);

        pool.ActivateAudioSource(sound);
    }

    public AudioSourceController PlaySoundLoop(SoundName soundName, float duration = 0.0f) {
        Sound sound = FindSoundByName(soundName);
        if(duration == 0.0f) {
            //Loop until Stop() is called
            return pool.ActivateAudioSource(sound, true);
        }
        else {
            //Play for given duration
            return pool.ActivateAudioSource(sound, true, duration);
        }
    }

    //TODO: Remove
    public void PlaySoundAtPosition(SoundName soundName, Vector2 position, float volume = 1.0f) {
        Sound sound = FindSoundByName(soundName);
    }

    private Sound FindSoundByName(SoundName name) {
        for(int i = 0; i < Sounds.Length; i++) {
            if (Sounds[i].Name == name) return Sounds[i];
        }
        return null;
    }
}
