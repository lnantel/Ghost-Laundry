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

        RecurringCustomer customer = CustomerManager.instance.GetRecurringCustomer(characterIndex);
        Garment garment = new OllieFemur();
        garment.customerID = customerID;
        customer.garments.Add(garment);

        int garmentCount = 6;
        for (int i = 0; i < garmentCount; i++) {
            garment = Garment.GetRandomGarment();
            garment.customerID = customer.ticketNumber;
            customer.garments.Add(garment);
            if (garment is GarmentSock) {
                GarmentSock otherSock = new GarmentSock((GarmentSock)garment);
                customer.garments.Add(otherSock);
                i++;
            }
        }

        //Shuffle clothing to separate socks
        int n = customer.garments.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Garment value = customer.garments[k];
            customer.garments[k] = customer.garments[n];
            customer.garments[n] = value;
        }

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
