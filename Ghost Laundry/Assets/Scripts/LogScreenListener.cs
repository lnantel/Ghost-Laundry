using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScreenListener : MonoBehaviour
{
    public Canvas logCanvas;

    private void OnEnable() {
        GameManager.ShowLog += OnShowLog;
        GameManager.HideLog += OnHideLog;
    }

    private void OnDisable() {
        GameManager.ShowLog -= OnShowLog;
        GameManager.HideLog -= OnHideLog;
    }

    private void OnShowLog() {
        logCanvas.gameObject.SetActive(true);
    }

    private void OnHideLog() {
        logCanvas.gameObject.SetActive(false);
    }
}
