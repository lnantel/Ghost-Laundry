using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] sounds;

    private float masterVolume = 1.0f;
    private AudioSource source;

    private void Start() {
        source = GetComponent<AudioSource>();
    }

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void PlaySound(Sounds sound, float volume) {
        source.clip = sounds[(int) sound];
        source.volume = masterVolume * volume;
        source.Play();
    }

    public void PlaySoundAtPosition(Sounds sound, Vector2 position, float volume) {
        AudioSource.PlayClipAtPoint(sounds[(int)sound], position, masterVolume * volume);
    }
}
