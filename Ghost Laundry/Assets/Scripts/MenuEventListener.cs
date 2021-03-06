﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuEventListener : MonoBehaviour
{
    private void OnEnable() {
    }

    private void OnDisable() {
    }

    public void OnContinue() {
        GameManager.instance.LaunchGame();
    }

    public void OnNewGame() {
        GameManager.instance.LaunchNewGame();
    }

    public void OnLoadGame() {
        GameManager.instance.LoadGame();
    }

    public void OnResume() {
        GameManager.instance.Resume();
    }

    public void OnShowSettings() {
        if(GameManager.ShowSettings != null) GameManager.ShowSettings();
    }

    public void OnHideSettings() {
        if (GameManager.HideSettings != null) GameManager.HideSettings();
    }

    public void OnShowLog() {
        if (GameManager.ShowLog != null) GameManager.ShowLog();
    }

    public void OnHideLog() {
        if (GameManager.HideLog != null) GameManager.HideLog();
    }

    public void OnMainMenu() {
        GameManager.instance.GoToMainMenu();
    }

    public void OnQuit() {
        Application.Quit();
    }

    public void OnRetry() {
        GameManager.instance.OnRetry();
    }

    public void OnNextDay() {
        GameManager.instance.OnNextDay();
    }

    public void OnCredits() {
        GameManager.instance.OnCredits();
    }
}
