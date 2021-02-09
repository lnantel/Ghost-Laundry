using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventListener : MonoBehaviour
{
    public void OnPlay() {
        GameManager.instance.LaunchGame();
    }

    public void OnResume() {
        GameManager.instance.Resume();
    }

    public void OnShowOptions() {
        GameManager.instance.ShowOptions();
    }

    public void OnHideOptions() {
        GameManager.instance.HideOptions();
    }

    public void OnMainMenu() {
        GameManager.instance.GoToMainMenu();
    }

    public void OnQuit() {
        Application.Quit();
    }
}
