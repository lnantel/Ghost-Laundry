using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class OllieEndings : MonoBehaviour
{
    private OllieEventManager ollieEventManager;

    private Flowchart ending;

    private void Start() {
        ollieEventManager = GetComponentInParent<OllieEventManager>();
        ending = ollieEventManager.GetEnding();
        StartDialog();
    }

    public void StartDialog() {
        ending.gameObject.SetActive(true);
        EventManager.instance.dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    public void OnEventEnd() {
        ending.gameObject.SetActive(false);
        StartCoroutine(DisableDialogCanvas());
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        EventManager.instance.dialogCanvas.gameObject.SetActive(false);
        GameManager.instance.OnDialogEnd(0);
        EventManager.instance.EventEnd();
    }


    public void GetCap() {
        EventManager.instance.currentEvent.NextEventIndex = 1;
    }

    public void GetSkateboard() {
        EventManager.instance.currentEvent.NextEventIndex = 2;
    }

    public void GetSkull() {
        EventManager.instance.currentEvent.NextEventIndex = 3;
    }
}
