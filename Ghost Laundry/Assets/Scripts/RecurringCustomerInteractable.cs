using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecurringCustomerInteractable : Interactable
{
    public static Action<int> StartDialog;

    private RecurringCustomer recurringCustomer;

    protected override void Start() {
        base.Start();
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
