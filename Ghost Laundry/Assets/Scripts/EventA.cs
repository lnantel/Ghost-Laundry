using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventA : NarrativeEventListener {
    private bool boneLaundered = false;

    public override void NextEvent() {
        if(boneLaundered)
            narrativeEvent.NextEventIndex = 1;
        else
            narrativeEvent.NextEventIndex = 2;
    }

    private void Start() {
        GameObject laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");

        //TODO: Generate the basket for this event. For now it's a single random garment
        RecurringCustomer customer = CustomerManager.instance.GetRecurringCustomer(characterIndex);
        Garment garment = new OllieFemur();
        garment.customerID = customerID;
        customer.garments.Add(garment);

        Basket basket = new Basket();
        foreach (Garment g in customer.garments)
            basket.AddGarment(g);

        customer.basket = basket;
    }

    protected override void OnLaundryCompleted(LaundromatBag bag) {
        if(customerID == bag.customerID) {
            foreach(Garment garment in bag.contents) {
                if(garment is OllieFemur) {
                    boneLaundered = garment.Clean && garment.Dry && garment.Folded;
                }
            }
            NextEvent();
        }
    }
}
