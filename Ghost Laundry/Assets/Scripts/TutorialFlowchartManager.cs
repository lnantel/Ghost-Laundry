using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System;

public class TutorialFlowchartManager : MonoBehaviour
{
    public static TutorialFlowchartManager instance;

    public Flowchart[] flowcharts;
    public Canvas dialogCanvas;

    private string[] letters = { "", " A", " B", " C", " D", " E", " F" };

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartDialog(int step, int substep = 0) {
        flowcharts[step - 1].gameObject.SetActive(true);
        flowcharts[step - 1].ExecuteBlock("Step " + step + letters[substep]);
        dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    public void OnDialogEnd() {
        for(int i = 0; i < flowcharts.Length; i++) {
            flowcharts[i].gameObject.SetActive(false);
        }
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        dialogCanvas.gameObject.SetActive(false);
    }
}
