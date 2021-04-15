using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    public bool Looping;
    public float LoopDuration;

    public AudioSource[] sources;

    public Sound sound { get => m_sound; set => SetSound(value); }
    private Sound m_sound;

    private IEnumerator loopCoroutine;

    private float tailDuration;

    private float maxVolume;

    private bool source0Queued;
    private bool source1Queued;

    private bool stopping;
    private float stopTimer;

    private void SetSound(Sound value) {
        m_sound = value;

        tailDuration = Mathf.Min(1.0f, 0.1f * m_sound.Clip.length);

        maxVolume = m_sound.Volume;


        for (int i = 0; i < sources.Length; i++) {
            sources[i].clip = m_sound.Clip;
            sources[i].volume = m_sound.Volume;
            sources[i].pitch = m_sound.Pitch;
        }
    }

    private void OnEnable() {
        stopping = false;
        stopTimer = 0.0f;
        sources[0].Play();
        source0Queued = false;
        if (Looping) {
            sources[1].PlayDelayed(m_sound.Clip.length - tailDuration);
            source1Queued = true;
            if(LoopDuration > 0.0f) {
                loopCoroutine = LoopCoroutine();
                StartCoroutine(loopCoroutine);
            }
        }
    }

    private void OnDisable() {
        if(loopCoroutine != null) {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
    }

    private IEnumerator LoopCoroutine() {
        yield return new WaitForLaundromatSeconds(LoopDuration);
        Stop();
    }

    public void Stop() {
        stopping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopping) {
            //Lerp volume to 0 over 1 second
            stopTimer += Time.deltaTime;
            for (int i = 0; i < sources.Length; i++) {
                if (sources[i].time >= sources[i].clip.length - tailDuration) {
                    sources[i].volume = Mathf.SmoothStep(maxVolume, 0.0f, stopTimer);
                }
                if (stopTimer >= 1.0f) sources[i].Stop();
            }
            if (!sources[0].isPlaying && !sources[1].isPlaying) gameObject.SetActive(false);
        }
        else {
            //When the sound clip is done playing, deactivate the object
            if (!sources[0].isPlaying && !sources[1].isPlaying && !Looping) gameObject.SetActive(false);

            //Alternate between the two AudioSources to loop the sound clip
            if (sources[0].isPlaying && source0Queued) source0Queued = false;
            if (sources[1].isPlaying && source1Queued) source1Queued = false;

            if (Looping && !source0Queued && !sources[0].isPlaying) {
                source0Queued = true;
                sources[0].PlayDelayed(sources[0].clip.length - tailDuration - sources[1].time);
            }

            if (Looping && !source1Queued && !sources[1].isPlaying) {
                source1Queued = true;
                sources[1].PlayDelayed(sources[1].clip.length - tailDuration - sources[0].time);
            }

            //Fade in or out the volume at the start and the tail of the clip
            for (int i = 0; i < sources.Length; i++) {
                if (Looping && sources[i].time >= sources[i].clip.length - tailDuration) {
                    //Lerp volume to 0
                    sources[i].volume = Mathf.SmoothStep(maxVolume, 0.0f, (sources[i].time - (sources[i].clip.length - tailDuration)) / tailDuration);
                }
                if (Looping && sources[i].time <= tailDuration) {
                    //Lerp volume to max
                    sources[i].volume = Mathf.SmoothStep(0.0f, maxVolume, sources[i].time / tailDuration);
                }
                else {
                    sources[i].volume = maxVolume;
                }
            }
        }
    }
}
