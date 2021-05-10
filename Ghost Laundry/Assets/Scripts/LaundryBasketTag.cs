using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryBasketTag : LaundryObject
{
    LaundryBasket laundryBasket;

    // Start is called before the first frame update
    void Start()
    {
        laundryBasket = GetComponentInParent<LaundryBasket>();
    }

    public override void OnInteract() {
        base.OnInteract();
        laundryBasket.CycleTag();
    }

    public override void OnGrab() {
        laundryBasket.OnGrab();
    }

    public override void OnInspect() {
        laundryBasket.OnInspect();
    }
}
