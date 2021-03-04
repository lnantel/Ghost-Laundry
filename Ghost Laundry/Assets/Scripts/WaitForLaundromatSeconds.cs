using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForLaundromatSeconds : CustomYieldInstruction
{
    private float timer;
    private float time;

    public override bool keepWaiting {
        get {
            timer += TimeManager.instance.deltaTime;
            return timer < time;
        }
    }

    public WaitForLaundromatSeconds(float seconds) {
        this.timer = 0.0f;
        this.time = seconds;
    }
}
