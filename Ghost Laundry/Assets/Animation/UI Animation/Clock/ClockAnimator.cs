using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimator : MonoBehaviour
{
    private Animator animator;
    private int lastTimeRemaining;
    private bool tick;

    private void Start() {
        animator = GetComponent<Animator>();
        lastTimeRemaining = 0;
    }

    private void Update() {
        float timeRemaining = TimeManager.instance.RemainingTime();

        animator.SetFloat("RemainingTime", timeRemaining);

        if(timeRemaining < 60.0f) {
            if((int)timeRemaining != lastTimeRemaining) {
                if (tick) {
                    AudioManager.instance.PlaySound(SoundName.Tick);
                }
                else {
                    AudioManager.instance.PlaySound(SoundName.Tock);
                }
                tick = !tick;
            }
        }

        lastTimeRemaining = (int)timeRemaining;
    }
}
