using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    private void GoToDay(int day) {
        SaveManager.Save();
        TimeManager.instance.CurrentDay = day - 1;
        GameManager.instance.OnNextDay();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            ToastManager.instance.SayLine("Test line");
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            ToastManager.instance.SayLine("Second test line");
        }

        //Press M to gain 10$
        if (Input.GetKeyDown(KeyCode.M) && MoneyManager.instance != null) {
            MoneyManager.instance.ModifyCurrentAmount(1000);
        }

        //Press R to gain 200 Rep
        if(Input.GetKeyDown(KeyCode.R) && ReputationManager.instance != null) {
            ReputationManager.instance.ModifyCurrentAmount((int) ReputationManager.instance.AmountPerStar, PlayerStateManager.instance.transform.position);
        }

        //Press T to lose 200 Rep
        if(Input.GetKeyDown(KeyCode.T) && ReputationManager.instance != null) {
            ReputationManager.instance.ModifyCurrentAmount(-(int)ReputationManager.instance.AmountPerStar, PlayerStateManager.instance.transform.position);
        }

        //Press Backspace to skip to the next day
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            GoToDay(TimeManager.instance.CurrentDay + 1);
        }

        //Press a numpad number to go to that day. Slash = 10, * = 11, - = 12.
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            GoToDay(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            GoToDay(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            GoToDay(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            GoToDay(3);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            GoToDay(4);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            GoToDay(5);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6)) {
            GoToDay(6);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            GoToDay(7);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            GoToDay(8);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            GoToDay(9);
        }

        if (Input.GetKeyDown(KeyCode.KeypadDivide)) {
            GoToDay(10);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
            GoToDay(11);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            GoToDay(12);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            GoToDay(13);
        }
    }
}
