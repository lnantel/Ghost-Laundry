using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventA : NarrativeEventListener {
    private bool perfectLaundry = false;

    public override void NextEvent() {
        if(perfectLaundry)
            narrativeEvent.NextEventIndex = 2;
        else
            narrativeEvent.NextEventIndex = 1;
    }

    private void Start() {
        GameObject laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");

        //TODO: Generate the basket for this event. For now it's a single random garment
        RecurringCustomer customer = CustomerManager.instance.GetRecurringCustomer(characterIndex);
        Garment garment = Garment.GetRandomGarment();
        garment.customerID = customerID;
        customer.garments.Add(garment);
        if (garment is GarmentSock) {
            GarmentSock otherSock = new GarmentSock((GarmentSock)garment);
            customer.garments.Add(otherSock);
        }

        Basket basket = new Basket();
        foreach (Garment g in customer.garments)
            basket.AddGarment(g);

        customer.basket = basket;
    }

    protected override void OnLaundryCompleted(LaundromatBag bag) {
        if(customerID == bag.customerID) {
            Debug.Log("Event A OnLaundryCompleted called");
            if (bag.totalGarments == bag.perfectGarments) {
                perfectLaundry = true;
            }
            NextEvent();
        }
    }
}
