using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopInteractable : Interactable
{
    public static Action<int> BoughtItem;
    public static Action LockShop;
    public static Action UnlockShop;

    public int DetergentRefillPrice;

    protected override void OnEnable() {
        base.OnEnable();
        LockShop += Lock;
        UnlockShop += Unlock;
    }

    protected override void OnDisable() {
        base.OnDisable();
        LockShop -= Lock;
        UnlockShop -= Unlock;
    }

    protected override void Interaction() {
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
