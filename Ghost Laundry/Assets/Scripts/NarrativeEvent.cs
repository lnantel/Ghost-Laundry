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
    private int hour = 0;
    [SerializeField]
    private int minute = 0;

    public int[] Time { get => GetTime(); }
    private int[] GetTime() {
        return new int[] { hour, minute };
    }

    [HideInInspector]
    public int EventTreeIndex;

    //The object to activate when the event starts
    public GameObject EventObject;

    //NextEvent is the index of the next event in the NarrativeEventTree array
    public int NextEventIndex;

    public bool Completed;
}
