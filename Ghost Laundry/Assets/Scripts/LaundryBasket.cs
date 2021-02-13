using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryBasket : LaundryObject
{
    public static Action<Garment> TakeOutGarment;
    public static Action<LaundryBasket> OpenBasketView;

    public Basket basket;
    public GameObject basketView;

    private GameObject laundryGarmentPrefab;
    private List<LaundryGarment> laundryGarments;

    public Sprite[] tags;
    public SpriteRenderer tagSprite;

    private void Start() {
        if (basket == null) {
            basket = new Basket();
        }
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarment");
        laundryGarments = new List<LaundryGarment>();
        tagSprite.sprite = tags[basket.tag];
    }

    private void OnEnable() {
        LaundryGarment.Released += OnGarmentDroppedInBasket;
        OpenBasketView += OnOtherOpenBasketView;
    }

    private void OnDisable() {
        LaundryGarment.Released -= OnGarmentDroppedInBasket;
        OpenBasketView -= OnOtherOpenBasketView;
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
        if (!basketView.activeSelf) {
            if (GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position)) {
                if (basket.AddGarment(laundryGarment.garment)) {
                    Destroy(laundryGarment.gameObject);
                }
                else {
                    BasketIsFull();
                }
            }
        }
        else {
            bool alreadyInBasket = laundryGarments.Contains(laundryGarment);
            bool withinBasketView = basketView.GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position);

            if (!alreadyInBasket && withinBasketView) {
                if (basket.AddGarment(laundryGarment.garment, laundryGarment.transform.position - transform.position)) {
                    laundryGarment.transform.parent = transform;
                    laundryGarment.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    laundryGarments.Add(laundryGarment);
                }
                else {
                    BasketIsFull();
                    //TODO: If the basket is full, the Garment "bounces" out of BasketView.
                }
            }
            else if(alreadyInBasket && !withinBasketView) {
                basket.RemoveGarment(laundryGarment.garment);
                laundryGarments.Remove(laundryGarment);
                laundryGarment.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
        
    }

    void BasketIsFull() {
        Debug.Log("Basket is full");
    }

    public override void Drag(Vector2 cursorPosition) {
        //LaundryBaskets are not draggable
    }

    public override void OnInspect() {
        //Show contents of the basket in a pop-up window.
        //Garments can be grabbed, inspected, and generally behave the same as they would in the laundry view.
        //Any garments inside the pop-up are inside the basket, and vice-versa.
        
        //Enable BasketView. 
        if (!basketView.activeSelf) {
            EnableBasketView();
        }
        
        //When closing: Save all new Garment positions, minimize and disable the BasketView.
        else if (basketView.activeSelf) {
            DisableBasketView();
        }
    }

    private void EnableBasketView() {
        basketView.SetActive(true);
        OpenBasketView(this);
        //Instantiate all Garments on top of it in their given positions on the Basket object. 
        laundryGarments = new List<LaundryGarment>();

        for (int i = 0; i < basket.contents.Count; i++) {
            LaundryGarment laundryGarment = Instantiate(laundryGarmentPrefab, basket.positions[i] + transform.position, transform.rotation, transform).GetComponent<LaundryGarment>();
            laundryGarment.GetComponent<SpriteRenderer>().sortingOrder = 2;
            laundryGarments.Add(laundryGarment);
        }
    }

    private void DisableBasketView() {
        basket.contents = new List<Garment>();
        basket.currentLoad = 0;
        basket.positions = new List<Vector3>();
        for (int i = 0; i < laundryGarments.Count; i++) {
            basket.contents.Add(laundryGarments[i].garment);
            basket.currentLoad += laundryGarments[i].garment.size;
            basket.positions.Add(laundryGarments[i].transform.localPosition);
        }

        foreach (LaundryGarment obj in laundryGarments) {
            Destroy(obj.gameObject);
        }

        basketView.SetActive(false);
    }

    private void OnOtherOpenBasketView(LaundryBasket other) {
        if (this != other && basketView.activeSelf)
            DisableBasketView();
    }

    public override void OnInteract() {
        //Cycle through tags
        basket.tag = (basket.tag + 1) % tags.Length;

        tagSprite.sprite = tags[basket.tag];
    }
}
