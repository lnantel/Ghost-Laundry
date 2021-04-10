using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System;

public class OllieEndings : MonoBehaviour
{
    public static Action UpdateDecorations;

    private OllieEventManager ollieEventManager;
    private RecurringCustomer ollie;
    private Flowchart ending;

    private void Start() {
        ollieEventManager = GetComponentInParent<OllieEventManager>();
        ending = ollieEventManager.GetEnding();
        if (ollieEventManager.SafetyPoints != -3)
            CustomerManager.instance.SpawnRecurringCustomer();
        else {
            GetSkull();
            StartDialog();
        }
    }

    private void OnEnable() {
        RecurringCustomerInteractable.StartDialog += OnInteract;
    }

    private void OnDisable() {
        RecurringCustomerInteractable.StartDialog -= OnInteract;
    }

    private void OnInteract(int treeIndex) {
        if (treeIndex == 0) {
            ollie = CustomerManager.instance.GetRecurringCustomer(0);
            StartDialog();
        }
    }

    public void StartDialog() {
        ending.gameObject.SetActive(true);
        EventManager.instance.dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    public void OnEventEnd() {
        if(ollie != null) ollie.OnEndDialog(0);
        ending.gameObject.SetActive(false);
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        EventManager.instance.dialogCanvas.gameObject.SetActive(false);
        EventManager.instance.EventEnd();
    }


    public void GetCap() {
        EventManager.instance.currentEvent.NextEventIndex = 1;
        if (UpdateDecorations != null) UpdateDecorations();
    }

    public void GetSkateboard() {
        EventManager.instance.currentEvent.NextEventIndex = 2;
        if (UpdateDecorations != null) UpdateDecorations();
    }

    public void GetSkull() {
        EventManager.instance.currentEvent.NextEventIndex = 3;
        if (UpdateDecorations != null) UpdateDecorations();
    }
}
