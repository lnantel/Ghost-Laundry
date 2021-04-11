﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class OllieEvent2 : MonoBehaviour
{
    public Flowchart flowchart;

    private OllieEventManager ollieEventManager;

    private OllieFemur bone;

    private RecurringCustomer ollie;

    private void Start() {
        ollieEventManager = GetComponentInParent<OllieEventManager>();
    }

    private void OnEnable() {
        Customer.BagPickedUp += OnBagPickedUp;
        RecurringCustomerInteractable.StartDialog += OnInteract;
        CustomerManager.instance.SpawnRecurringCustomer();
    }

    private void OnDisable() {
        Customer.BagPickedUp -= OnBagPickedUp;
        RecurringCustomerInteractable.StartDialog -= OnInteract;
    }

    private void OnInteract(int treeIndex) {
        if (treeIndex == 0) {
            ollie = CustomerManager.instance.GetRecurringCustomer(0);
            GenerateLaundry();
            StartDialog("Event 2");
        }
    }

    private void GenerateLaundry() {
        Basket basket = new Basket();

        bone = new OllieFemur();

        Fabric cotton = new Fabric(FabricType.Cotton);

        basket.AddGarment(bone);
        basket.AddGarment(new GarmentUnderwear(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentUnderwear(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentShirt(cotton, GarmentColor.Teal));
        basket.AddGarment(new GarmentTop(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentTop(cotton, GarmentColor.Salmon));

        ollie.basket = basket;

        ollie.garments = new List<Garment>();

        for (int i = 0; i < basket.contents.Count; i++) {
            ollie.garments.Add(basket.contents[i]);
        }
    }

    private void SafetyResult() {
        ollieEventManager.SafetyPoints++;
        StartDialog("Event 2 A");
    }

    private void DangerResult() {
        ollieEventManager.SafetyPoints--;
        StartDialog("Event 2 B");
    }

    private void NeutralResult() {
        OnEventEnd();
    }

    public void StartDialog(string blockName) {
        flowchart.gameObject.SetActive(true);
        flowchart.ExecuteBlock(blockName);
        EventManager.instance.dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    //End block Event 2 with call to OnDialogEnd
    //End blocks Event 2 A + Event 1 B with a call to OnEventEnd
    public void OnDialogEnd() {
        flowchart.gameObject.SetActive(false);
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
        ollie.OnEndDialog(0);
    }

    public void OnEventEnd() {
        flowchart.gameObject.SetActive(false);
        StartCoroutine(DisableDialogCanvas(true));
        GameManager.instance.OnDialogEnd(0);
    }

    private IEnumerator DisableDialogCanvas(bool endEvent = false) {
        yield return null;
        EventManager.instance.dialogCanvas.gameObject.SetActive(false);
        if (endEvent) EventManager.instance.EventEnd();
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        if (ollie != null && bag.customerID == ollie.ticketNumber) {
            if (bone.Ruined) SafetyResult();
            else if (bone.Clean && bone.Dry) DangerResult();
            else NeutralResult();
        }
    }
}
