using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicController : MonoBehaviour
{
    public static Action TrackDone;

    public MusicTrack Track { get => m_Track; set => SetMusicTrack(value); }
    private MusicTrack m_Track;

    private AudioSource source;

    private IEnumerator transitionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Stop() {
        TransitionToTrack(null);
    }

    private void SetMusicTrack(MusicTrack track) {
        if (source.isPlaying) {
            TransitionToTrack(track);
        }else if(track != null) {
            m_Track = track;
            source.clip = track.Clip;
            source.volume = track.Volume;
            source.pitch = track.Pitch;
            StartTrack();
        }
    }

    private void StartTrack() {
        source.enabled = true;
        source.Play();
    }

    private void TransitionToTrack(MusicTrack track) {
        if(transitionCoroutine != null) {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = TransitionToTrackCoroutine(track);
        StartCoroutine(transitionCoroutine);
    }

    private IEnumerator TransitionToTrackCoroutine(MusicTrack track) {
        //Fade out currently playing track
        while(source.isPlaying && source.volume > 0.0f) {
            source.volume = Mathf.MoveTowards(source.volume, 0.0f, Time.deltaTime / 2.0f);
            yield return null;
        }
        source.Stop();

        //If transitioning to a non null track, start it
        SetMusicTrack(track);
        transitionCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (source.enabled && !source.isPlaying) OnTrackDone(); 
    }

    private void OnTrackDone() {
        source.enabled = false;
        if (TrackDone != null) TrackDone();
    }
}
