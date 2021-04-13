using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    public bool Looping;

    public AudioSource[] sources;

    private float tailDuration;

    private float maxVolume;

    private bool source0Queued;
    private bool source1Queued;

    private void OnEnable() {
        tailDuration = Mathf.Min(1.0f, 0.1f * sources[0].clip.length);
        maxVolume = sources[0].volume;
        sources[0].Play();
        source0Queued = false;
        if (Looping) {
            sources[1].clip = sources[0].clip;
            sources[1].volume = sources[0].volume;
            sources[1].pitch = sources[0].pitch;
            sources[1].PlayDelayed(sources[0].clip.length - tailDuration);
            source1Queued = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //When the sound clip is done playing, deactivate the object
        if (!sources[0].isPlaying && !sources[1].isPlaying && !Looping) gameObject.SetActive(false);

        //Alternate between the two AudioSources to loop the sound clip
        if(Looping && !source0Queued && !sources[0].isPlaying && sources[1].isPlaying) {
            source1Queued = false;
            source0Queued = true;
            sources[0].PlayDelayed(sources[0].clip.length - tailDuration - sources[1].time);
        }

        if (Looping && !source1Queued && !sources[1].isPlaying && sources[0].isPlaying) {
            source1Queued = true;
            source0Queued = false;
            sources[1].PlayDelayed(sources[1].clip.length - tailDuration - sources[0].time);
        }

        //Fade in or out the volume at the start and the tail of the clip
        for(int i = 0; i < sources.Length; i++) {
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
