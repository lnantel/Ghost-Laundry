using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance; 

    public static Action<int> StartOfDay;
    public static Action<int> EndOfDay;
    public static Action<int[]> TimeOfDay;

    public int CurrentDay;
    public int RealTimeMinutesPerDay;

    public float timeScale;
    public float deltaTime;

    private float timer;
    private int RealTimeSecondsPerDay;

    private int[] lastCurrentTime;

    public bool TimeIsPassing;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() {
        GameManager.PauseGame += OnPause;
        GameManager.ResumeGame += OnResume;
        GameManager.ShowDialog += OnShowDialog;
        GameManager.HideDialog += OnHideDialog;
    }

    private void OnDisable() {
        GameManager.PauseGame -= OnPause;
        GameManager.ResumeGame -= OnResume;
        GameManager.ShowDialog -= OnShowDialog;
        GameManager.HideDialog -= OnHideDialog;
    }

    private void Start() {
        RealTimeSecondsPerDay = RealTimeMinutesPerDay * 60;
        TimeIsPassing = false;
        timeScale = 0;
        lastCurrentTime = new int[] { 0, 0 };
    }

    void Update()
    {
        deltaTime = Time.deltaTime * timeScale;
        if (TimeIsPassing) {
            timer += deltaTime;
            int[] currentTime = CurrentTime();

            if (currentTime != null && (currentTime[0] != lastCurrentTime[0] || currentTime[1] != lastCurrentTime[1])) {
                if (TimeOfDay != null) TimeOfDay(currentTime);
                Debug.Log("It is " + currentTime[0] + ":" + currentTime[1].ToString("D2"));
            }

            if (timer >= RealTimeSecondsPerDay) {
                EndDay();
            }
            lastCurrentTime = currentTime;
        }
    }

    public float RemainingTime() {
        return RealTimeSecondsPerDay - timer;
    }

    public int[] CurrentTime() {
        float ratio = timer / RealTimeSecondsPerDay;
        float RealTimeSecondsPerHour = RealTimeSecondsPerDay / 12.0f;
        int hour = Mathf.FloorToInt(timer / RealTimeSecondsPerHour);
        int minute = Mathf.FloorToInt(((timer / RealTimeSecondsPerHour) - hour) * 60);
        return new int[] { hour + 12, minute };
    }

    public void StartDay() {
        TimeIsPassing = true;
        //Time.timeScale = 1;
        timeScale = 1;
        if(StartOfDay != null) StartOfDay(CurrentDay);
        Debug.Log("DAY " + CurrentDay + " START");
    }

    public void EndDay() {
        TimeIsPassing = false;
        //Time.timeScale = 0;
        timeScale = 0;
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
        Time.timeScale = 1;
    }

    private void OnShowDialog() {
        TimeIsPassing = false;
        timeScale = 0;
    }

    private void OnHideDialog() {
        TimeIsPassing = true;
        timeScale = 1;
    }
}
