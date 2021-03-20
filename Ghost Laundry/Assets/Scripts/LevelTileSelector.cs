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

    public Sprite[] characterEventIcons;
    public Sprite plusEventIcon;
    public Sprite unknownEventIcon;

    private void OnEnable() {
        DaySelected += OnDaySelected;
        selectable = TimeManager.instance.CurrentDay >= index;
        SelectionManager.ShowConfirmationWindow += OnShowConfirmationWindow;
        SelectionManager.HideConfirmationWindow += OnHideConfirmationWindow;

        if (selectable) {
            TXT_Day.text = "Day " + index;
            TXT_Money.text = (SaveManager.Data.Days[index - 1].Money / 100.0f).ToString("N2");
            TXT_Rep.text = (SaveManager.Data.Days[index - 1].Reputation / 200.0f).ToString("N0");

            //Event Icons
            int[] characters = EventManager.instance.CharactersWithEventsOnDay(index);
            bool lastDay = SaveManager.Data.Days.Count == index;

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
                        EventIcon1.sprite = characterEventIcons[characters[0]];
                    }
                    break;
                case 2:
                    EventIcon1.enabled = true;
                    EventIcon2.enabled = true;
                    EventIcon3.enabled = false;
                    EventIcon1.transform.position = new Vector3(EventIcon1.transform.position.x - 0.234f, EventIcon1.transform.position.y,EventIcon1.transform.position.z);
                    EventIcon2.transform.position = new Vector3(EventIcon1.transform.position.x + 0.234f, EventIcon1.transform.position.y,EventIcon1.transform.position.z);
                    if (lastDay) {
                        EventIcon1.sprite = unknownEventIcon;
                        EventIcon2.sprite = unknownEventIcon;
                    }
                    else {
                        EventIcon1.sprite = characterEventIcons[characters[0]];
                        EventIcon2.sprite = characterEventIcons[characters[1]];
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
                        EventIcon1.sprite = characterEventIcons[characters[1]];
                        EventIcon2.sprite = characterEventIcons[characters[0]];
                        EventIcon3.sprite = characterEventIcons[characters[2]];
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
                        EventIcon1.sprite = characterEventIcons[characters[1]];
                        EventIcon2.sprite = characterEventIcons[characters[0]];
                    }
                    EventIcon3.sprite = plusEventIcon;
                    break;
            }
        }
    }

    private void OnDisable() {
        DaySelected -= OnDaySelected;
        SelectionManager.ShowConfirmationWindow -= OnShowConfirmationWindow;
        SelectionManager.HideConfirmationWindow -= OnHideConfirmationWindow;
    }

    private void OnDaySelected(int day) {
        if(day == index) {
            popUp.SetActive(true);
        }
        else {
            popUp.GetComponentInChildren<Animator>().SetTrigger("HidePopUp");
        }
    }

    private void OnMouseEnter() {
        if(selectable && !confirming)
            if(DaySelected != null) DaySelected(index);
    }

    private void OnMouseDown() {
        if(selectable && !confirming)
            if (DayClicked != null) DayClicked(index);
    }

    private void OnShowConfirmationWindow() {
        confirming = true;
    }

    private void OnHideConfirmationWindow() {
        confirming = false;
    }
}