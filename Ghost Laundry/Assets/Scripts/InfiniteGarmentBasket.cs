using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteGarmentBasket : LaundryBasket
{
    public override void OnGrab() {
        if (basketCollider.enabled) {
            if (TakeOutGarment != null)
                TakeOutGarment(Garment.GetRandomGarment());
        }
    }

    public override void OnInspect() {
        //Not inspectable
    }
}
