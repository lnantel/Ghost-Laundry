using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public Canvas HUDCanvas;

    private void OnEnable() {
        GameManager.ShowHUD += OnShowHUD;
        GameManager.HideHUD += OnHideHUD;
    }

    private void OnDisable() {
        GameManager.ShowHUD -= OnShowHUD;
        GameManager.HideHUD -= OnHideHUD;
    }

    private void OnShowHUD() {
        HUDCanvas.gameObject.SetActive(true);
    }

    private void OnHideHUD() {
        HUDCanvas.gameObject.SetActive(false);
    }
}
