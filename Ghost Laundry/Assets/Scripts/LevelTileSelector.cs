﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelTileSelector : MonoBehaviour
{
    public static Action<int> DaySelected;
    public static Action<int> DayClicked;

    public int index;
    public bool selectable;
    private bool confirming;

    public GameObject popUp;
    public TextMeshPro TXT_Money;
    public TextMeshPro TXT_Rep;
    public TextMeshPro TXT_Day;

    public SpriteRenderer EventIcon1;
    public SpriteRenderer EventIcon2;
    public SpriteRenderer EventIcon3;

    public Sprite plusEventIcon;
    public Sprite unknownEventIcon;

    private bool initialized;

    private void OnEnable() {
        DaySelected += OnDaySelected;
        SelectionManager.ShowConfirmationWindow += OnShowConfirmationWindow;
        SelectionManager.HideConfirmationWindow += OnHideConfirmationWindow;

        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        while (TimeManager.instance == null)
            yield return null;
        yield return new WaitForSecondsRealtime(0.1f);

        int latestDay = 0;

        for(int i = 0; i < SaveManager.Data.Days.Count; i++) {
            int day = SaveManager.Data.Days[i].CurrentDay;
            if (day > latestDay) latestDay = day;
            if (day == index) {
                selectable = true;
            }
        }

        if (latestDay == index - 1) selectable = true;

        if (selectable) {
            bool lastDay = latestDay < index;

            if (!lastDay) {
                SaveData.DayData day = SaveManager.GetDayData(index);
                TXT_Day.text = "Day " + index;
                TXT_Money.text = (day.Money / 100.0f).ToString("N2");
                TXT_Rep.text = (day.ReputationHighScore / ReputationManager.instance.AmountPerStar).ToString("N0");
            }
            else {
                TXT_Day.text = "Day " + index;
                TXT_Money.text = ("?");
                TXT_Rep.text = ("?");
            }

            //Event Icons
            int[] characters = EventManager.instance.CharactersWithEventsOnDay(index);

            switch (characters.Length) {
                case 0:
                    EventIcon1.enabled = false;
                    EventIcon2.enabled = false;
                    EventIcon3.enabled = false;
                    break;
                case 1:
                    EventIcon1.enabled = true;
                    EventIcon2.enabled = false;
                    EventIcon3.enabled = false;
                    if (lastDay) {
                        EventIcon1.sprite = unknownEventIcon;
                    }
                    else {
                        EventIcon1.sprite = EventManager.instance.customerPortraits[characters[0]];
                    }
                    break;
                case 2:
                    EventIcon1.enabled = true;
                    EventIcon2.enabled = true;
                    EventIcon3.enabled = false;
                    EventIcon1.transform.position = new Vector3(EventIcon1.transform.position.x - 0.234f, EventIcon1.transform.position.y, EventIcon1.transform.position.z);
                    EventIcon2.transform.position = new Vector3(EventIcon1.transform.position.x + 0.234f, EventIcon1.transform.position.y, EventIcon1.transform.position.z);
                    if (lastDay) {
                        EventIcon1.sprite = unknownEventIcon;
                        EventIcon2.sprite = unknownEventIcon;
                    }
                    else {
                        EventIcon1.sprite = EventManager.instance.customerPortraits[characters[0]];
                        EventIcon2.sprite = EventManager.instance.customerPortraits[characters[1]];
                    }
                    break;
                case 3:
                    EventIcon1.enabled = true;
                    EventIcon2.enabled = true;
                    EventIcon3.enabled = true;
                    EventIcon2.transform.position = new Vector3(EventIcon1.transform.position.x - 0.468f, EventIcon1.transform.position.y, EventIcon1.transform.position.z);
                    EventIcon3.transform.position = new Vector3(EventIcon1.transform.position.x + 0.468f, EventIcon1.transform.position.y, EventIcon1.transform.position.z);
                    if (lastDay) {
                        EventIcon1.sprite = unknownEventIcon;
                        EventIcon2.sprite = unknownEventIcon;
                        EventIcon3.sprite = unknownEventIcon;
                    }
                    else {
                        EventIcon1.sprite = EventManager.instance.customerPortraits[characters[1]];
                        EventIcon2.sprite = EventManager.instance.customerPortraits[characters[0]];
                        EventIcon3.sprite = EventManager.instance.customerPortraits[characters[2]];
                    }
                    break;
                default:
                    EventIcon1.enabled = true;
                    EventIcon2.enabled = true;
                    EventIcon3.enabled = true;
                    EventIcon2.transform.position = new Vector3(EventIcon1.transform.position.x - 0.468f, EventIcon1.transform.position.y, EventIcon1.transform.position.z);
                    EventIcon3.transform.position = new Vector3(EventIcon1.transform.position.x + 0.468f, EventIcon1.transform.position.y, EventIcon1.transform.position.z);
                    if (lastDay) {
                        EventIcon1.sprite = unknownEventIcon;
                        EventIcon2.sprite = unknownEventIcon;
                    }
                    else {
                        EventIcon1.sprite = EventManager.instance.customerPortraits[characters[1]];
                        EventIcon2.sprite = EventManager.instance.customerPortraits[characters[0]];
                    }
                    EventIcon3.sprite = plusEventIcon;
                    break;
            }
        }

        initialized = true;
    }

    private void OnDisable() {
        DaySelected -= OnDaySelected;
        SelectionManager.ShowConfirmationWindow -= OnShowConfirmationWindow;
        SelectionManager.HideConfirmationWindow -= OnHideConfirmationWindow;
    }

    private void OnDaySelected(int day) {
        if (initialized) {
            if (day == index) {
                popUp.SetActive(true);
            }
            else {
                Animator animator = popUp.GetComponentInChildren<Animator>();
                if (animator != null && animator.isActiveAndEnabled) {
                    animator.SetTrigger("HidePopUp");
                }
            }
        }
    }

    private void OnMouseEnter() {
        if(initialized && selectable && !confirming)
            if(DaySelected != null) DaySelected(index);
    }

    private void OnMouseDown() {
        if(initialized && selectable && !confirming) {
            if (DayClicked != null) DayClicked(index);
            Debug.Log("Day clicked: " + index);
        }
    }

    private void OnShowConfirmationWindow() {
        confirming = true;
    }

    private void OnHideConfirmationWindow() {
        confirming = false;
    }
}
