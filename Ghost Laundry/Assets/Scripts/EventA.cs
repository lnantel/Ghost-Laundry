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
        Garment bone = new OllieFemur();

        Basket basket = LaundryManager.GetRandomBasket();
        while (!basket.AddGarment(bone)) {
            //If there is no space in the basket for the bone, remove something
            Garment removed = basket.RemoveTopGarment();
            //If the removed garment is a sock, remove its match
            if(removed is GarmentSock) {
                foreach(Garment garment in basket.contents) {
                    if(garment is GarmentSock) {
                        if (garment.color.Equals(removed.color) && garment.fabric.Equals(removed.fabric)) {
                            basket.RemoveGarment(garment);
                            break;
                        }
                    }

                }
            }
        }

        List<Garment> olliesGarments = new List<Garment>();

        foreach(Garment garment in basket.contents) {
            garment.customerID = customerID;
            olliesGarments.Add(garment);
        }

        customer.basket = basket;
        customer.garments = olliesGarments;
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
