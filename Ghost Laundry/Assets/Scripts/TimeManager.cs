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

    public bool TimeIsPassing;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() {
        GameManager.PauseGame += OnPause;
        GameManager.ResumeGame += OnResume;
    }

    private void OnDisable() {
        GameManager.PauseGame -= OnPause;
        GameManager.ResumeGame -= OnResume;
    }

    private void Start() {
        RealTimeSecondsPerDay = RealTimeMinutesPerDay * 60;
        TimeIsPassing = false;
        Time.timeScale = 0;
    }

    void Update()
    {
        if (TimeIsPassing) {
            timer += Time.deltaTime;
            if (timer >= RealTimeSecondsPerDay) {
                EndDay();
            }
            TimeOfDay();
        }
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
        TimeIsPassing = true;
        Time.timeScale = 1;
        if(StartOfDay != null) StartOfDay(CurrentDay);
        Debug.Log("DAY " + CurrentDay + " START");
    }

    public void EndDay() {
        TimeIsPassing = false;
        Time.timeScale = 0;
        if (EndOfDay != null) EndOfDay(CurrentDay);
        Debug.Log("DAY " + CurrentDay + " END");
    }

    public void NextDay() {
        CurrentDay++;
        timer = 0;
    }

    private void OnPause() {
        Time.timeScale = 0;
    }

    private void OnResume() {
        if (TimeIsPassing)
            Time.timeScale = 1;
    }
}
