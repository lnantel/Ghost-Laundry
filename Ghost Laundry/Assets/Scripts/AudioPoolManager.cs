using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPoolManager : PoolManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public AudioSourceController ActivateAudioSource(Sound sound, bool loop = false, float loopDuration = 0.0f) {
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf) {
                AudioSourceController controller = child.GetComponent<AudioSourceController>();
                controller.sound = sound;
                controller.Looping = loop;
                controller.LoopDuration = loopDuration;
                child.gameObject.SetActive(true);
                return controller;
            }
        }

        //If there are no inactive objects, return the first active object instead
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (child.gameObject.activeSelf) {
                child.gameObject.SetActive(false);
                AudioSourceController controller = child.GetComponent<AudioSourceController>();
                controller.sound = sound;
                controller.Looping = loop;
                controller.LoopDuration = loopDuration;
                child.gameObject.SetActive(true);
                return controller;
            }
        }

        //If there are no objects at all, return null
        Debug.LogError("Object pool is empty.");
        return null;
    }

    public void StopAllLoopingSounds() {
        for(int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (child.gameObject.activeSelf) {
                AudioSourceController controller = child.GetComponent<AudioSourceController>();
                if (controller.Looping) controller.Stop();
            }
        }
    }
}
