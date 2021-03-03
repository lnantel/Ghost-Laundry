using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[System.Serializable]
public class NarrativeEvent
{
    //The time at which the customer enters the laundromat
    public int Day;

    [SerializeField]
    private int hour;
    [SerializeField]
    private int minute;

    public int[] Time { get => GetTime(); }
    private int[] GetTime() {
        return new int[] { hour, minute };
    }

    //The flowchart (dialogue tree) associated with the event
    public Flowchart flowchart;

    //NarrativeEventListener
    public GameObject ListenerPrefab;

    //Child events
    public NarrativeEvent[] ChildEvents;
    //NextEvent is the index of the next event in the ChildEvents array
    public int NextEventIndex;

    public NarrativeEvent GetNextEvent() {
        if (Completed && ChildEvents.Length > 0)
            return ChildEvents[NextEventIndex].GetNextEvent();
        else if (Completed)
            return null;
        else
            return this;
    }

    public bool Completed;
}
