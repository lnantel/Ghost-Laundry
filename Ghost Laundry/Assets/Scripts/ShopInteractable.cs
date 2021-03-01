using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopInteractable : Interactable
{
    public static Action<int> BoughtItem;

    public int DetergentRefillPrice;

    public override void Interact() {
        //Spend $price to buy a refill of detergent
        if(DetergentManager.instance.CurrentAmount < DetergentManager.instance.MaxAmount) {
            DetergentManager.instance.Refill();
            if (BoughtItem != null) BoughtItem(DetergentRefillPrice);
        }
        else {
            Debug.Log("Detergent already full");
        }
    }
}
