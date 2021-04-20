﻿using System.Collections;
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
            AddBasket(new Basket(), i);
        }
    }

    public override bool InputBasket(Basket basket, int i) {
        //Returns true if the input basket is empty, and replaces it with 'basket'
        //If the input basket is not empty, returns false
        if (basketSlots[i].laundryBasket != null && basketSlots[i].laundryBasket.basket.contents.Count > i && !basketSlots[i].Locked) return false;
        else {
            if (basketSlots[i].laundryBasket != null) basketSlots[i].laundryBasket.basket = basket;
            if (BasketSlotsChanged != null) BasketSlotsChanged();
            return true;
        }
    }

    public override Basket OutputBasket(int basketSlotIndex) {
        //Returns an output basket
        //Empties the corresponding basket in the TableArea but does not destroy it
        //for (int i = 1; i < basketCapacity; i++) {
            if (basketSlots[basketSlotIndex].laundryBasket != null && basketSlots[basketSlotIndex].laundryBasket.basket.contents.Count > 0 && !basketSlots[basketSlotIndex].Locked) {
                Basket outputBasket = new Basket();
                outputBasket.contents = basketSlots[basketSlotIndex].laundryBasket.basket.contents;
                outputBasket.positions = basketSlots[basketSlotIndex].laundryBasket.basket.positions;
                outputBasket.tag = basketSlots[basketSlotIndex].laundryBasket.basket.tag;

                basketSlots[basketSlotIndex].laundryBasket.basket.RemoveAll();
                if (BasketSlotsChanged != null) BasketSlotsChanged();
                return outputBasket;
            }
        //}

        //If no output baskets contain anything, check if the input basket does
        //if (basketSlots[0].laundryBasket != null && basketSlots[0].laundryBasket.basket.contents.Count > 0 && !basketSlots[0].Locked) {
        //    Basket outputBasket = new Basket();
        //    outputBasket.contents = basketSlots[0].laundryBasket.basket.contents;
        //    outputBasket.positions = basketSlots[0].laundryBasket.basket.positions;
        //    outputBasket.tag = basketSlots[0].laundryBasket.basket.tag;

        //    basketSlots[0].laundryBasket.basket.RemoveAll();
        //    if (BasketSlotsChanged != null) BasketSlotsChanged();
        //    return outputBasket;
        //}

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
