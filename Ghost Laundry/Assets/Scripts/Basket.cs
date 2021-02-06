using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Basket : LaundryObject
{
    public static Action<Garment> TakeOutGarment;

    public List<Garment> contents;
    public int capacity;
    private int currentLoad;

    void Start()
    {
        contents = new List<Garment>();
    }

    private void OnEnable() {
        LaundryGarment.Released += OnGarmentDroppedInBasket;
    }

    private void OnDisable() {
        LaundryGarment.Released -= OnGarmentDroppedInBasket;
    }

    //Returns true if the garment was successfully added, false otherwise
    bool AddGarment(Garment garment) {
        if (HasSpaceFor(garment.size)) {
            contents.Add(garment);
            currentLoad += garment.size;
            return true;
        }
        return false;
    }

    Garment RemoveTopGarment() {
        if(contents.Count > 0) {
            Garment garment = contents[contents.Count - 1];
            contents.RemoveAt(contents.Count - 1);
            currentLoad -= garment.size;
            return garment;
        }
        return null;
    }

    List<Garment> RemoveAll() {
        List<Garment> temp = contents;
        contents = new List<Garment>();
        currentLoad = 0;
        return temp;
    }

    bool HasSpaceFor(int load) {
        return currentLoad + load <= capacity;
    }

    //Behaviour in laundry mode
    public override void OnGrab() {
        Garment garment = RemoveTopGarment();
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
            if (AddGarment(laundryGarment.garment)) {
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
