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

    [HideInInspector]
    public int EventTreeIndex;

    //The flowchart (dialogue tree) associated with the event
    public GameObject flowchart;

    //NarrativeEventListener
    public GameObject ListenerPrefab;

    //NextEvent is the index of the next event in the ChildEvents array
    public int NextEventIndex;

    public bool Completed;
}
