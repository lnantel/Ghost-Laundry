using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecurringCustomerInteractable : Interactable
{
    public static Action<int> StartDialog;

    private RecurringCustomer recurringCustomer;

    protected override void Start() {
        popUpPrefab = (GameObject)Resources.Load("DialogPopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position, transform.rotation, transform);
        popUpInstance.SetActive(false);
        popUpAnimator = popUpInstance.GetComponentInChildren<Animator>();
        Locked = true;
        recurringCustomer = GetComponent<RecurringCustomer>();
    }

    protected override void Interaction() {
        if (!Locked) {
            StartDialog(recurringCustomer.EventTreeIndex);
        }
    }

    private void Update() {
        if(recurringCustomer.state == CustomerState.WaitingForService) {
            Locked = false;
        }
        else {
            Locked = true;
        }
    }
}
