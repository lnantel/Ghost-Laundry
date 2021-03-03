using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public Canvas pauseCanvas;

    private void OnEnable() {
        GameManager.PauseGame += ShowPauseMenu;
        GameManager.ResumeGame += HidePauseMenu;
    }

    private void OnDisable() {
        GameManager.PauseGame -= ShowPauseMenu;
        GameManager.ResumeGame -= HidePauseMenu;
    }

    private void ShowPauseMenu() {
        pauseCanvas.gameObject.SetActive(true);
    }

    private void HidePauseMenu() {
        pauseCanvas.gameObject.SetActive(false);
    }
}
