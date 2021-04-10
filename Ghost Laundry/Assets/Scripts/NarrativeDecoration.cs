using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeDecoration : MonoBehaviour
{
    public GameObject OllieSkull;
    public GameObject OllieSkateboard;

    private void UpdateDecorations() {
        if (EventManager.instance != null) {
            OllieSkull.SetActive(EventManager.instance.OlliesSkull());
            OllieSkateboard.SetActive(EventManager.instance.OlliesSkateboard());
        }
    }

    private void OnEnable() {
        OllieEndings.UpdateDecorations += UpdateDecorations;
        SaveManager.LoadingComplete += UpdateDecorations;
    }

    private void OnDisable() {
        OllieEndings.UpdateDecorations -= UpdateDecorations;
        SaveManager.LoadingComplete -= UpdateDecorations;
    }
}
