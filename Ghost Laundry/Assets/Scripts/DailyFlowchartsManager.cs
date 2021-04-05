using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DailyFlowchartsManager : MonoBehaviour
{
    public static DailyFlowchartsManager instance;

    public Flowchart[] flowcharts;
    public Canvas dialogCanvas;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() {
        TimeManager.StartOfDay += StartDialog;
    }

    private void OnDisable() {
        TimeManager.StartOfDay -= StartDialog;
    }

    public void StartDialog(int day) {
        if(day > 0 && day <= flowcharts.Length && flowcharts[day - 1] != null) {
            flowcharts[day - 1].gameObject.SetActive(true);
            flowcharts[day - 1].ExecuteBlock("Day" + day);
            dialogCanvas.gameObject.SetActive(true);
            GameManager.instance.OnDialogStart();
        }
    }

    public void OnDialogEnd() {
        for (int i = 0; i < flowcharts.Length; i++) {
            if(flowcharts[i] != null) flowcharts[i].gameObject.SetActive(false);
        }
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        dialogCanvas.gameObject.SetActive(false);
    }
}