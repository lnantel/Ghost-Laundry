using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWorkstation : WorkStation
{
    protected override void Start() {
        HasGravity = true;
        base.Start();
    }

    public override void Initialize() {
        base.Initialize();
        for (int i = 0; i < basketCapacity; i++) {
            LaundryBasket basket = Instantiate(laundryBasketPrefab, laundryTaskArea.transform.position + basketSlots[i], transform.rotation, laundryTaskArea.transform).GetComponent<LaundryBasket>();
            basket.basket = new Basket();
            containedBaskets.Add(basket);
        }
    }

    public override bool InputBasket(Basket basket) {
        //Returns true if the input basket is empty, and replaces it with 'basket'
        //If the input basket is not empty, returns false
        if (containedBaskets[0].basket.contents.Count > 0) return false;
        else {
            containedBaskets[0].basket = basket;
            return true;
        }
    }

    public override Basket OutputBasket() {
        //Returns an output basket
        //Empties the corresponding basket in the TableArea but does not destroy it
        for (int i = 1; i < basketCapacity; i++) {
            if (containedBaskets[i].basket.contents.Count > 0) {
                Basket outputBasket = new Basket();
                outputBasket.contents = containedBaskets[i].basket.contents;
                outputBasket.positions = containedBaskets[i].basket.positions;
                outputBasket.tag = containedBaskets[i].basket.tag;

                containedBaskets[i].basket.RemoveAll();

                return outputBasket;
            }
        }
        return null;
    }

    public override bool ContainsBasket() {
        //Returns true if any of the output baskets contains at least one item
        for(int i = 1; i < basketCapacity; i++) {
            if (containedBaskets[i].basket.contents.Count > 0) return true;
        }
        return false;
    }
}
