using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NarrativeEventListener : MonoBehaviour
{
    [HideInInspector]
    public NarrativeEvent narrativeEvent;

    [HideInInspector]
    public int characterIndex;

    [HideInInspector]
    public int customerID;

    private void OnEnable() {
        Customer.BagPickedUp += OnLaundryCompleted;
    }

    private void OnDisable() {
        Customer.BagPickedUp -= OnLaundryCompleted;
    }

    //Evaluate the bag's contents and determine nextEvent
    protected abstract void OnLaundryCompleted(LaundromatBag bag);

    //Sets narrativeEvent.NextEventIndex to the index of the next event in the NarrativeEventTree array
    public abstract void NextEvent();
}
