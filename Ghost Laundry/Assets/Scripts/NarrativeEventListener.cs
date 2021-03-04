using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NarrativeEventListener : MonoBehaviour
{
    public NarrativeEvent narrativeEvent;

    public int characterIndex;
    public int customerID;

    private void OnEnable() {
        Customer.BagPickedUp += OnLaundryCompleted;
    }

    private void OnDisable() {
        Customer.BagPickedUp -= OnLaundryCompleted;
    }

    //Evaluate the bag's contents and determine nextEvent
    protected abstract void OnLaundryCompleted(LaundromatBag bag);

    //Sets narrativeEvent.NextEventIndex to the relative index of the next event in the EventManager array
    //The next event's absolute index is the index of the current NarrativeEvent + this.NextEvent()
    //Sets a default value in case of the laundry not being completed
    public abstract void NextEvent();
}
