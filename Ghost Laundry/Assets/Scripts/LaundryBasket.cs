﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryBasket : LaundryObject, ITrackable {
    public static Action<Garment> TakeOutGarment;
    public static Action<LaundryBasket> OpenBasketView;
    public static Action TagChanged;
    public static Action<LaundryGarment> PlacedInBasketView;

    public Basket basket;
    public GameObject basketView;
    protected Collider2D basketCollider;

    private GameObject laundryGarmentPrefab;
    private List<LaundryGarment> laundryGarments;

    public Sprite[] tags;
    public SpriteRenderer tagSprite;

    public bool Locked { get; private set; }

    private Animator animator;

    private void Start() {
        if (basket == null) {
            basket = new Basket();
        }
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarment");
        laundryGarments = new List<LaundryGarment>();
        basketCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable() {
        if (basket != null) tagSprite.sprite = tags[basket.tag];
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
        OpenBasketView += OnOtherOpenBasketView;
        PlacedInBasketView += OnPlacedInBasketView;
    }

    private void OnDisable() {
        //if (basketView.activeSelf) DisableBasketView();
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
        OpenBasketView -= OnOtherOpenBasketView;
        PlacedInBasketView -= OnPlacedInBasketView;
    }

    public void Lock() {
        Locked = true;
        if (basketView.activeSelf) {
            DisableBasketView();
        }
    }

    public void Unlock() {
        Locked = false;
    }

    public override void OnGrab() {
        if (!Locked) {
            if (basketCollider.enabled) {
                Garment garment = basket.RemoveTopGarment();
                if (garment != null && TakeOutGarment != null) {
                    TakeOutGarment(garment);
                    animator.SetTrigger("BasketOutput");
                }
                else
                    GrabEmpty();
            }
        }
    }

    void GrabEmpty() {
        animator.SetTrigger("BasketEmpty");
    }

    void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        if (!Locked) {
            //If BasketView is open
            if (basketView.activeSelf) {
                bool alreadyInBasket = laundryGarments.Contains(laundryGarment);
                bool withinBasketView = basketView.GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position);

                if (alreadyInBasket && withinBasketView) {
                    Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
                    rb.gravityScale = 0.0f;
                    rb.velocity = Vector3.zero;
                    AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                    if (PlacedInBasketView != null) PlacedInBasketView(laundryGarment);
                }
                else if (!alreadyInBasket && withinBasketView) {
                    if (basket.AddGarment(laundryGarment.garment, laundryGarment.transform.position - transform.position)) {
                        laundryGarment.transform.parent = transform;
                        Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
                        rb.gravityScale = 0.0f;
                        rb.velocity = Vector3.zero;
                        AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                        laundryGarments.Add(laundryGarment);
                    }
                    else {
                        BasketIsFull();
                    }
                    if (PlacedInBasketView != null) PlacedInBasketView(laundryGarment);
                }
                else if (alreadyInBasket && !withinBasketView) {
                    basket.RemoveGarment(laundryGarment.garment);
                    laundryGarments.Remove(laundryGarment);
                }
            }
            //If BasketView is closed
            //Wait for a frame, in case an overlapping BasketView captures the garment first
            else {
                StartCoroutine(DelayedAddToBasket(laundryGarment));
            }
        }
    }

    private bool placedInBasketView;
    private int placedInBasketViewID;

    //When a LaundryGarment is placed in a BasketView, remember it
    private void OnPlacedInBasketView(LaundryGarment laundryGarment) {
        placedInBasketView = true;
        placedInBasketViewID = laundryGarment.GetInstanceID();
    }

    private IEnumerator DelayedAddToBasket(LaundryGarment laundryGarment) {
        //Wait for a frame, in case an overlapping BasketView captures the garment first
        yield return new WaitForSeconds(0);
        //If the garment was captured by a BasketView, ignore it; otherwise, add it to the basket
        if (laundryGarment != null && laundryGarment.gameObject.activeSelf) {
            if (!(placedInBasketView && laundryGarment.GetInstanceID() == placedInBasketViewID)) {
                if (basketCollider.bounds.Contains(laundryGarment.transform.position)) {
                    if (basket.AddGarment(laundryGarment.garment)) {
                        AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                        laundryGarment.ReturnToPool();
                        animator.SetTrigger("BasketInput");
                    }
                    else {
                        BasketIsFull();
                    }
                }
            }
        }

        //Reset 'placed in basket view' variables
        placedInBasketView = false;
        placedInBasketViewID = 0;
    }

    void BasketIsFull() {
        animator.SetTrigger("BasketFull");
    }

    public override void Drag(Vector2 cursorPosition) {
        //LaundryBaskets are not draggable
    }

    public override void OnInspect() {
        //Show contents of the basket in a pop-up window.
        //Garments can be grabbed, inspected, and generally behave the same as they would in the laundry view.
        //Any garments inside the pop-up are inside the basket, and vice-versa.

        //Enable BasketView. 
        if (!Locked) {
            if (!basketView.activeSelf) {
                EnableBasketView();
            }

            //When closing: Save all new Garment positions, minimize and disable the BasketView.
            else if (basketView.activeSelf) {
                DisableBasketView();
            }
        }
    }

    private void EnableBasketView() {
        basketCollider.enabled = false;
        basketView.SetActive(true);
        OpenBasketView(this);
        //Instantiate all Garments on top of it in their given positions on the Basket object. 
        laundryGarments = new List<LaundryGarment>();

        for (int i = 0; i < basket.contents.Count; i++) {
            LaundryGarment laundryGarment = basket.contents[i].CreateLaundryGarment(basket.positions[i] + transform.position, transform.rotation, transform);
            laundryGarments.Add(laundryGarment);
        }
    }

    public void DisableBasketView() {
        StartCoroutine(DisableBasketViewCoroutine());
    }

    public IEnumerator DisableBasketViewCoroutine() {
        basket.contents = new List<Garment>();
        basket.positions = new List<Vector3>();
        for (int i = 0; i < laundryGarments.Count; i++) {
            basket.contents.Add(laundryGarments[i].garment);
            basket.positions.Add(laundryGarments[i].transform.localPosition);
        }

        foreach (LaundryGarment obj in laundryGarments) {
            obj.ReturnToPool();
        }

        laundryGarments.Clear();

        yield return new WaitForSeconds(0);

        basketView.SetActive(false);
        basketCollider.enabled = true;

    }

    private void OnOtherOpenBasketView(LaundryBasket other) {
        if (this != other && basketView.activeSelf)
            DisableBasketView();
    }

    public void CycleTag() {
        //Cycle through tags
        basket.tag = (basket.tag + 1) % tags.Length;
        tagSprite.sprite = tags[basket.tag];
        if(TagChanged != null) TagChanged();
    }

    public bool ContainsTrackedGarment() {
        return basket.ContainsTrackedGarment();
    }

    public bool ContainsAGarment(params Garment[] garments) {
        for(int i = 0; i < garments.Length; i++) {
            if (basket.contents.Contains(garments[i])) return true;
        }
        return false;
    }
}
