using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance; 

    public static Action<int> StartOfDay;
    public static Action<int> EndOfDay;

    public int CurrentDay;
    public int RealTimeMinutesPerDay;

    private float timer;
    private int RealTimeSecondsPerDay;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        RealTimeSecondsPerDay = RealTimeMinutesPerDay * 60;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= RealTimeSecondsPerDay) {
            EndDay();
        }
        TimeOfDay();
    }

    public float RemainingTime() {
        return RealTimeSecondsPerDay - timer;
    }

    public int[] TimeOfDay() {
        float ratio = timer / RealTimeSecondsPerDay;
        float RealTimeSecondsPerHour = RealTimeSecondsPerDay / 12.0f;
        int hour = Mathf.FloorToInt(timer / RealTimeSecondsPerHour);
        int minute = Mathf.FloorToInt(((timer / RealTimeSecondsPerHour) - hour) * 60);
        Debug.Log(hour + ":" + minute.ToString("D2"));
        return new int[] { hour + 12, minute };
    }

    public void StartDay() {
        if(StartOfDay != null) StartOfDay(CurrentDay);
    }

    public void EndDay() {
        if (StartOfDay != null) EndOfDay(CurrentDay);
    }
}
