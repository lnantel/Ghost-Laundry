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
        locked = true;
        recurringCustomer = GetComponent<RecurringCustomer>();
    }

    public override void Interact() {
        if (!locked) {
            StartDialog(recurringCustomer.EventTreeIndex);
        }
    }

    private void Update() {
        if(recurringCustomer.state == CustomerState.WaitingForService) {
            locked = false;
        }
        else {
            locked = true;
        }
    }
}
