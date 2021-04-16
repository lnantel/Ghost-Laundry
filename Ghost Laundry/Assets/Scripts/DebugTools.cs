using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    private void Update() {
        //Press M to gain 10$
        if(Input.GetKeyDown(KeyCode.M) && MoneyManager.instance != null) {
            MoneyManager.instance.ModifyCurrentAmount(1000);
        }

        //Press R to gain 200 Rep
        if(Input.GetKeyDown(KeyCode.R) && ReputationManager.instance != null) {
            ReputationManager.instance.ModifyCurrentAmount(200, PlayerStateManager.instance.transform.position);
        }

        //Press T to lose 200 Rep
        if(Input.GetKeyDown(KeyCode.T) && ReputationManager.instance != null) {
            ReputationManager.instance.ModifyCurrentAmount(-200, PlayerStateManager.instance.transform.position);
        }

        //Press Backspace to skip to the next day
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            TimeManager.instance.CurrentDay++;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        //Press a numpad number to go to that day. Slash = 10, * = 11, - = 12.
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            TimeManager.instance.CurrentDay = 0;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            TimeManager.instance.CurrentDay = 1;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            TimeManager.instance.CurrentDay = 2;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            TimeManager.instance.CurrentDay = 3;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            TimeManager.instance.CurrentDay = 4;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            TimeManager.instance.CurrentDay = 5;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6)) {
            TimeManager.instance.CurrentDay = 6;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            TimeManager.instance.CurrentDay = 7;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            TimeManager.instance.CurrentDay = 8;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            TimeManager.instance.CurrentDay = 9;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.KeypadDivide)) {
            TimeManager.instance.CurrentDay = 10;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
            TimeManager.instance.CurrentDay = 11;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            TimeManager.instance.CurrentDay = 12;
            SaveManager.Save();
            GameManager.instance.OnNextDay();
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            TimeManager.instance.CurrentDay = 13;
            SaveManager.Save();
            GameManager.instance.OnNextDay();

        }
    }
}
