using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventB : NarrativeEventListener
{
    public override void NextEvent() {
        narrativeEvent.NextEventIndex = 1;
    }

    private void Start() {
        
    }

    protected override void OnLaundryCompleted(LaundromatBag bag) {

    }
}
