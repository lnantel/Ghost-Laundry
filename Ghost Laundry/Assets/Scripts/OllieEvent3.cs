using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System;

public class OllieEvent3 : MonoBehaviour
{
    public static Action EventStarted;

    public Flowchart flowchart;

    private OllieEventManager ollieEventManager;

    private RecurringCustomer ollie;

    private bool helmetBought;

    private void Start() {
        ollieEventManager = GetComponentInParent<OllieEventManager>();
    }

    private void OnEnable() {
        Customer.BagPickedUp += OnBagPickedUp;
        ollie = CustomerManager.instance.GetRecurringCustomer(0);
        GenerateLaundry();
        StartDialog("Event 3");
        ollie.GiveBasketToPlayer();
    }

    private void OnDisable() {
        Customer.BagPickedUp -= OnBagPickedUp;
    }

    private void GenerateLaundry() {
        Basket basket = new Basket();
        Fabric cotton = new Fabric(FabricType.Cotton);

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
        StartDialog("Event 3 A");
    }

    private void DangerResult() {
        ollieEventManager.SafetyPoints--;
        StartDialog("Event 3 B");
    }

    public void StartDialog(string blockName) {
        flowchart.gameObject.SetActive(true);
        flowchart.ExecuteBlock(blockName);
        EventManager.instance.dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
        if (EventStarted != null) EventStarted();
    }

    private void OnHelmetBought() {
        helmetBought = true;
    }

    //End block Event 3 with call to OnDialogEnd
    //End blocks Event 3 A + Event 3 B with a call to OnEventEnd
    public void OnDialogEnd() {
        flowchart.gameObject.SetActive(false);
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
    }

    public void OnEventEnd() {
        OnDialogEnd();
        EventManager.instance.EventEnd();
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        EventManager.instance.dialogCanvas.gameObject.SetActive(false);
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        if (bag.customerID == ollie.ticketNumber) {
            if (helmetBought) SafetyResult();
            else DangerResult();
        }
    }
}
