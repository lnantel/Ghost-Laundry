using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventListener : MonoBehaviour
{
    private void OnEnable() {
    }

    private void OnDisable() {
    }

    public void OnPlay() {
        GameManager.instance.LaunchGame();
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

    public void OnMainMenu() {
        GameManager.instance.GoToMainMenu();
    }

    public void OnQuit() {
        Application.Quit();
    }
}
