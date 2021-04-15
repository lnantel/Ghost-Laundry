using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MusicTrack
{
    public MusicTrackType Type;
    public AudioClip Clip;
    [Range(0.0f, 10.0f)]
    public float Volume;
    [Range(-10.0f, 10.0f)]
    public float Pitch;

    public MusicTrack(MusicTrackType type, AudioClip clip, float volume = 1.0f, float pitch = 1.0f) {
        Type = type;
        Clip = clip;
        Volume = volume;
        Pitch = pitch;
    }
}

public enum MusicTrackType {
    MenuTrack,
    TutorialTrack,
    GameplayTrack
}
