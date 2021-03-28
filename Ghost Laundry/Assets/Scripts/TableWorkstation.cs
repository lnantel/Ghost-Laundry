using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableWorkstation : WorkStation
{
    [HideInInspector]
    public bool FoldingLocked;

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("TableArea");
        HasGravity = true;
        base.Start();
    }

    public override IEnumerator Initialize() {
        yield return base.Initialize();
        for (int i = 0; i < basketCapacity; i++) {
            AddBasket(new Basket());
        }
    }

    public override bool InputBasket(Basket basket) {
        //Returns true if the input basket is empty, and replaces it with 'basket'
        //If the input basket is not empty, returns false
        if (basketSlots[0].laundryBasket != null && basketSlots[0].laundryBasket.basket.contents.Count > 0 && !basketSlots[0].Locked) return false;
        else {
            if (basketSlots[0].laundryBasket != null) basketSlots[0].laundryBasket.basket = basket;
            if (BasketSlotsChanged != null) BasketSlotsChanged();
            return true;
        }
    }

    public override Basket OutputBasket() {
        //Returns an output basket
        //Empties the corresponding basket in the TableArea but does not destroy it
        for (int i = 1; i < basketCapacity; i++) {
            if (basketSlots[i].laundryBasket != null && basketSlots[i].laundryBasket.basket.contents.Count > 0 && !basketSlots[i].Locked) {
                Basket outputBasket = new Basket();
                outputBasket.contents = basketSlots[i].laundryBasket.basket.contents;
                outputBasket.positions = basketSlots[i].laundryBasket.basket.positions;
                outputBasket.tag = basketSlots[i].laundryBasket.basket.tag;

                basketSlots[i].laundryBasket.basket.RemoveAll();
                if (BasketSlotsChanged != null) BasketSlotsChanged();
                return outputBasket;
            }
        }

        //If no output baskets contain anything, check if the input basket does
        if (basketSlots[0].laundryBasket != null && basketSlots[0].laundryBasket.basket.contents.Count > 0 && !basketSlots[0].Locked) {
            Basket outputBasket = new Basket();
            outputBasket.contents = basketSlots[0].laundryBasket.basket.contents;
            outputBasket.positions = basketSlots[0].laundryBasket.basket.positions;
            outputBasket.tag = basketSlots[0].laundryBasket.basket.tag;

            basketSlots[0].laundryBasket.basket.RemoveAll();
            if (BasketSlotsChanged != null) BasketSlotsChanged();
            return outputBasket;
        }

        return null;
    }

    public override bool ContainsBasket() {
        //Returns true if any basket contains any garment
        for(int i = 0; i < basketSlots.Length; i++) {
            if(basketSlots[i].laundryBasket != null && basketSlots[i].laundryBasket.basket.contents.Count > 0) {
                return true;
            }
        }
        return false;
    }
}
