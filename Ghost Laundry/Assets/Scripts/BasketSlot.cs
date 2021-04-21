using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasketSlot
{
    public LaundryBasket laundryBasket;
    public Vector3 spawnPoint;

    [HideInInspector]
    public bool Locked { get; private set; }

    public BasketSlot(LaundryBasket laundryBasket, Vector3 spawnPoint) {
        this.laundryBasket = laundryBasket;
        this.spawnPoint = spawnPoint;
        Locked = false;
    }

    public void Lock() {
        Locked = true;
        if(laundryBasket != null) laundryBasket.Lock();
    }

    public void Unlock() {
        Locked = false;
        if (laundryBasket != null) laundryBasket.Unlock();
    }
}
