﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action EndDialog;

    //Each character has a tree at a given index in this array
    public NarrativeEvent[] EventTrees;

    [HideInInspector]
    public List<NarrativeEvent> EventsToday;

    private NarrativeEvent currentEvent;

    private void Start() {
        EventsToday = new List<NarrativeEvent>();
    }

    private void OnEnable() {
        TimeManager.StartOfDay += OnDayStart;
        TimeManager.EndOfDay += OnDayEnd;
        TimeManager.TimeOfDay += OnTime;
    }

    private void OnDisable() {
        TimeManager.StartOfDay -= OnDayStart;
        TimeManager.EndOfDay -= OnDayEnd;
        TimeManager.TimeOfDay -= OnTime;
    }

    private void OnDayStart(int day) {
        //Find what events happen today and add them to a list
        //For each character, find the next event; if it is today, add it to the list
        foreach(NarrativeEvent tree in EventTrees) {
            NarrativeEvent nextEvent = tree.GetNextEvent();
            if (nextEvent != null && nextEvent.Day == day)
                EventsToday.Add(nextEvent);
        }
    }

    private void OnTime(int[] currentTime) {
        //Check today's events. If it is time for one of them,
        foreach(NarrativeEvent eventToday in EventsToday) {
            if(eventToday.Time[0] == currentTime[0] && eventToday.Time[1] == currentTime[1]) {
                //TODO:
                //spawn the correct customer through CustomerManager
                //(ignoring reputation and laundromat capacity)
            }
        }
    }

    private void OnDayEnd(int day) {
        //Mark all of today's events as completed, even if they are not done (or started!)
        foreach(NarrativeEvent eventToday in EventsToday) {
            eventToday.Completed = true;
        }

        //Clear today's list of events
        EventsToday.Clear();
    }

    public void OnDialogStart(int characterIndex) {
        //Find customer's next event
        NarrativeEvent nextEvent = EventTrees[characterIndex].GetNextEvent();

        //Enable flowchart
        nextEvent.flowchart.gameObject.SetActive(true);

        //Spawn NarrativeEventListener
        NarrativeEventListener listener = Instantiate(nextEvent.ListenerPrefab).GetComponent<NarrativeEventListener>();

        //Link it to the correct NarrativeEvent
        listener.narrativeEvent = nextEvent;
        currentEvent = nextEvent;
    }

    public void EventEnd() {
        //Mark the event as completed
        if (currentEvent != null) {
            currentEvent.Completed = true;
            currentEvent.flowchart.gameObject.SetActive(false);
        }

        if (EndDialog != null) EndDialog();
    }
}
