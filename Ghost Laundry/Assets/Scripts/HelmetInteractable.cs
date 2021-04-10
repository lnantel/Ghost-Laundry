using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HelmetInteractable : Interactable
{
    public static Action<int> BoughtItem;
    public static Action LockShop;
    public static Action UnlockShop;

    public GameObject HelmetContainer;

    public int HelmetPrice;

    protected override void OnEnable() {
        base.OnEnable();
        LockShop += Lock;
        UnlockShop += Unlock;
        LockShop();

        OllieEvent3.EventStarted += EnableHelmetShop;
    }

    protected override void OnDisable() {
        base.OnDisable();
        LockShop -= Lock;
        UnlockShop -= Unlock;

        OllieEvent3.EventStarted -= EnableHelmetShop;
    }

    private void EnableHelmetShop() {
        UnlockShop();
        HelmetContainer.SetActive(true);
    }

    private void DisableHelmetShop() {
        LockShop();
        HelmetContainer.SetActive(false);
    }

    protected override void Interaction() {
        //Spend $price to buy a refill of detergent
        if (BoughtItem != null) BoughtItem(HelmetPrice);
        DisableHelmetShop();
    }
}
