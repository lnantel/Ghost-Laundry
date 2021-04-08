using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public static Action StartDialog;
    public static Action<int> EndDialog;

    public Canvas dialogCanvas;

    public OllieEventManager ollieEventManager;

    //Each character has a tree at a given index in this array
    public string[] customerNames;
    public Sprite[] customerPortraits;
    public NarrativeEventTree[] EventTrees;

    [HideInInspector]
    public List<NarrativeEvent> EventsToday;

    public NarrativeEvent currentEvent;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        EventsToday = new List<NarrativeEvent>();

        for(int i = 0; i < EventTrees.Length; i++) {
            for (int j = 0; j < EventTrees[i].tree.Length; j++)
                EventTrees[i].tree[j].EventTreeIndex = i;
        }
    }

    private void OnEnable() {
        TimeManager.StartOfDay += OnDayStart;
        TimeManager.EndOfDay += OnDayEnd;
        TimeManager.TimeOfDay += OnTime;

        RecurringCustomerInteractable.StartDialog += OnEventStart;
    }

    private void OnDisable() {
        TimeManager.StartOfDay -= OnDayStart;
        TimeManager.EndOfDay -= OnDayEnd;
        TimeManager.TimeOfDay -= OnTime;

        RecurringCustomerInteractable.StartDialog -= OnEventStart;
    }

    private void OnDayStart(int day) {
        //Find what events happen today and add them to a list
        //For each character, find the next event; if it is today, add it to the list
        foreach(NarrativeEventTree tree in EventTrees) {
            NarrativeEvent nextEvent = tree.GetNextEvent();
            if (nextEvent != null && nextEvent.Day == day)
                EventsToday.Add(nextEvent);
        }
    }

    public int[] CharactersWithEventsOnDay(int day) {
        List<int> result = new List<int>();
        for(int i = 0; i < EventTrees.Length; i++){
            if (EventTrees[i].HasEventOnDay(day))
                result.Add(i);
        }
        return result.ToArray();
    }

    private void OnTime(int[] currentTime) {
        //Check today's events. If it is time for one of them,
        foreach(NarrativeEvent eventToday in EventsToday) {
            if(eventToday.Time[0] == currentTime[0] && eventToday.Time[1] == currentTime[1]) {
                //TODO:
                //spawn the correct customer through CustomerManager
                //(ignoring reputation and laundromat capacity)
                CustomerManager.instance.SpawnRecurringCustomer();
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

    public void OnEventStart(int characterIndex) {
        //Find customer's next event
        NarrativeEvent nextEvent = EventTrees[characterIndex].GetNextEvent();

        //Enable flowchart
        if(nextEvent != null) {
            nextEvent.EventObject.SetActive(true);
            currentEvent = nextEvent;
        }
    }

    public void EventEnd() {
        //Mark the event as completed
        if (currentEvent != null) {
            currentEvent.Completed = true;
            currentEvent.EventObject.SetActive(false);
            if (EndDialog != null) EndDialog(currentEvent.EventTreeIndex);
        }
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        dialogCanvas.gameObject.SetActive(false);
    }

    public bool OlliesCap() {
        return EventTrees[0].IsCompleted() && EventTrees[0].EndingObtained() == 1;
    }

    public bool OlliesSkateboard() {
        return EventTrees[0].IsCompleted() && EventTrees[0].EndingObtained() == 2;
    }

    public bool OlliesSkull() {
        return EventTrees[0].IsCompleted() && EventTrees[0].EndingObtained() == 3;
    }
}
