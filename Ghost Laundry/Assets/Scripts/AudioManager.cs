using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public MusicTrack[] Music;
    public Sound[] Sounds;

    public AudioPoolManager EffectsPool;
    public MusicController musicController;

    public MusicTrackType CurrentPlaylist { get => m_CurrentPlaylist; set => PlayMusic(value); }
    private MusicTrackType m_CurrentPlaylist;
    private int lastTrackIndex;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void OnEnable() {
        MusicController.TrackDone += SetNextTrack;
    }

    private void OnDisable() {
        MusicController.TrackDone -= SetNextTrack;
    }

    public void PlaySound(SoundName soundName) {
        Sound sound = FindSoundByName(soundName);

        EffectsPool.ActivateAudioSource(sound);
    }

    public void PlayMusic(MusicTrackType type) {
        m_CurrentPlaylist = type;
        MusicTrack track = FindNextTrackOfType(type);
        musicController.Track = track;
    }

    private void SetNextTrack() {
        musicController.Track = FindNextTrackOfType(m_CurrentPlaylist);
    }

    public void StopMusic() {
        musicController.Stop();
    }

    public AudioSourceController PlaySoundLoop(SoundName soundName, float duration = 0.0f) {
        Sound sound = FindSoundByName(soundName);
        if(duration == 0.0f) {
            //Loop until Stop() is called
            return EffectsPool.ActivateAudioSource(sound, true);
        }
        else {
            //Play for given duration
            return EffectsPool.ActivateAudioSource(sound, true, duration);
        }
    }

    private Sound FindSoundByName(SoundName name) {
        for(int i = 0; i < Sounds.Length; i++) {
            if (Sounds[i].Name == name) return Sounds[i];
        }
        return null;
    }

    private MusicTrack FindNextTrackOfType(MusicTrackType type) {
        for(int i = 0; i < Music.Length; i++) {
            if (Music[(lastTrackIndex + 1 + i) % Music.Length].Type == type) {
                int trackIndex = (lastTrackIndex + 1 + i) % Music.Length;
                lastTrackIndex = trackIndex;
                return Music[trackIndex];
            }
        }
        return null;
    }
}
