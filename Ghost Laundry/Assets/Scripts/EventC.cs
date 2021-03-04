using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventC : NarrativeEventListener
{
    public override void NextEvent() {
        narrativeEvent.NextEventIndex = 1;
    }

    private void Start() {
       
    }

    protected override void OnLaundryCompleted(LaundromatBag bag) {

    }
}
