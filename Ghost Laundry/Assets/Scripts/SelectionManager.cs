using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionManager : MonoBehaviour
{
    public Transform ghost;

    public Transform[] Levels;

    public float ghostSpeed;

    private Animator ghostAnimator;

    private int selectedDay;

    private void OnEnable() {
        LevelTileSelector.DaySelected += OnDaySelected;
        LevelTileSelector.DayClicked += OnDayClicked;
    }

    private void OnDisable() {
        LevelTileSelector.DaySelected -= OnDaySelected;
        LevelTileSelector.DayClicked -= OnDayClicked;
    }

    private void Start() {
        int CurrentDay = TimeManager.instance.CurrentDay;

        for (int i = 0; i < Levels.Length; i++) {
            if (i < CurrentDay) Levels[i].gameObject.GetComponent<LevelTileAnimator>().Flip();
        }

        selectedDay = CurrentDay;
    }

    private void OnDaySelected(int day) {
        selectedDay = day;
        ghost.position = Levels[selectedDay].position;
    }

    private void OnDayClicked(int day) {
        SaveManager.LoadDay(day);
        GameManager.instance.LaunchGame();
    }
}
