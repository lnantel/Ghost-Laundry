using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryBasket : LaundryObject
{
    public static Action<Garment> TakeOutGarment;

    public Basket basket;

    private void Start() {
        if (basket == null) {
            basket = new Basket();
        }
    }

    private void OnEnable() {
        LaundryGarment.Released += OnGarmentDroppedInBasket;
    }

    private void OnDisable() {
        LaundryGarment.Released -= OnGarmentDroppedInBasket;
    }

    public override void OnGrab() {
        Garment garment = basket.RemoveTopGarment();
        if (garment != null && TakeOutGarment != null)
            TakeOutGarment(garment);
        else
            GrabEmpty();
    }

    void GrabEmpty() {
        Debug.Log("Basket is empty");
    }

    void OnGarmentDroppedInBasket(LaundryGarment laundryGarment) {
        if (GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position)) {
            if (basket.AddGarment(laundryGarment.garment)) {
                Destroy(laundryGarment.gameObject);
            }
            else {
                BasketIsFull();
            }
        }
    }

    void BasketIsFull() {
        Debug.Log("Basket is full");
    }

    public override void Drag(Vector2 cursorPosition) {
        
    }

}
