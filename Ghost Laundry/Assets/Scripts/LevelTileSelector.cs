using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelTileSelector : MonoBehaviour
{
    public static Action<int> DaySelected;
    public static Action<int> DayClicked;

    public int index;
    public bool selected;
    public bool selectable;

    public GameObject popUp;
    public TextMeshPro TXT_Money;
    public TextMeshPro TXT_Rep;
    public TextMeshPro TXT_Time;

    private void OnEnable() {
        DaySelected += OnDaySelected;
        selectable = TimeManager.instance.CurrentDay >= index;

        if (selectable) {
            TXT_Money.text = (SaveManager.Data.Days[index - 1].Money / 100.0f).ToString("N2");
            TXT_Rep.text = (SaveManager.Data.Days[index - 1].Reputation / 200.0f).ToString("N0");

            int hours = Mathf.FloorToInt(SaveManager.Data.Days[index - 1].Playtime / 3600.0f);
            int minutes = Mathf.FloorToInt((SaveManager.Data.Days[index - 1].Playtime - hours * 3600.0f) / 60.0f);
            int seconds = Mathf.FloorToInt(SaveManager.Data.Days[index - 1].Playtime - hours * 3600.0f - minutes * 60.0f);

            TXT_Time.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }
    }

    private void OnDisable() {
        DaySelected -= OnDaySelected;
    }

    private void OnDaySelected(int day) {
        if(day == index) {
            selected = true;
            popUp.SetActive(true);
        }
        else {
            selected = false;
            popUp.GetComponentInChildren<Animator>().SetTrigger("HidePopUp");
        }
    }

    private void OnMouseEnter() {
        if(selectable)
            if(DaySelected != null) DaySelected(index);
    }

    private void OnMouseDown() {
        if(selectable)
            if (DayClicked != null) DayClicked(index);
    }
}
