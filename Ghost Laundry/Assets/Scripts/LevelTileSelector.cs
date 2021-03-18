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
            popUp.SetActive(false);
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
