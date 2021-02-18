using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasketSlot
{
    public LaundryBasket laundryBasket;
    public Vector3 spawnPoint;

    public BasketSlot(LaundryBasket laundryBasket, Vector3 spawnPoint) {
        this.laundryBasket = laundryBasket;
        this.spawnPoint = spawnPoint;
    }
}
