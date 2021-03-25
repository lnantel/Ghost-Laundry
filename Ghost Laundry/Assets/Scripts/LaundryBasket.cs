using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryBasket : LaundryObject
{
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
        if(basket != null) tagSprite.sprite = tags[basket.tag];
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
        OpenBasketView += OnOtherOpenBasketView;
        PlacedInBasketView += OnPlacedInBasketView;
    }

    private void OnDisable() {
        if (basketView.activeSelf) DisableBasketView();
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
        OpenBasketView -= OnOtherOpenBasketView;
        PlacedInBasketView -= OnPlacedInBasketView;
    }

    public override void OnGrab() {
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

    void GrabEmpty() {
        animator.SetTrigger("BasketEmpty");
    }

    void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        if (basketView.activeSelf) {
            //If BasketView is open
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
        else {
            //If BasketView is closed
            //Wait for a frame, in case an overlapping BasketView captures the garment first
            StartCoroutine(DelayedAddToBasket(laundryGarment));
        }
    }

    private bool placedInBasketView;
    private int placedInBasketViewID;

    private void OnPlacedInBasketView(LaundryGarment laundryGarment) {
        placedInBasketView = true;
        placedInBasketViewID = laundryGarment.GetInstanceID();
        StartCoroutine(ResetPlacedInBasketViewFlag());
    }

    private IEnumerator DelayedAddToBasket(LaundryGarment laundryGarment) {
        //Wait for a frame, in case an overlapping BasketView captures the garment first
        yield return new WaitForSeconds(0);
        if(laundryGarment != null) {
            if (!(placedInBasketView && laundryGarment.GetInstanceID() == placedInBasketViewID)) {
                if (GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position)) {
                    if (basket.AddGarment(laundryGarment.garment)) {
                        AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                        Destroy(laundryGarment.gameObject);
                        animator.SetTrigger("BasketInput");
                    }
                    else {
                        BasketIsFull();
                    }
                }
            }
        }
    }

    private IEnumerator ResetPlacedInBasketViewFlag() {
        yield return new WaitForSeconds(0);
        yield return new WaitForSeconds(0);
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
        basketCollider.enabled = false;
        OpenBasketView(this);
        //Instantiate all Garments on top of it in their given positions on the Basket object. 
        laundryGarments = new List<LaundryGarment>();

        for (int i = 0; i < basket.contents.Count; i++) {
            LaundryGarment laundryGarment = basket.contents[i].CreateLaundryGarment(basket.positions[i] + transform.position, transform.rotation, transform);
            laundryGarments.Add(laundryGarment);
        }
    }

    private void DisableBasketView() {
        basket.contents = new List<Garment>();
        basket.positions = new List<Vector3>();
        for (int i = 0; i < laundryGarments.Count; i++) {
            basket.contents.Add(laundryGarments[i].garment);
            basket.positions.Add(laundryGarments[i].transform.localPosition);
        }

        foreach (LaundryGarment obj in laundryGarments) {
            Destroy(obj.gameObject);
        }

        basketView.SetActive(false);
        basketCollider.enabled = true;
    }

    private void OnOtherOpenBasketView(LaundryBasket other) {
        if (this != other && basketView.activeSelf)
            DisableBasketView();
    }

    public override void OnInteract() {
        //Cycle through tags
        basket.tag = (basket.tag + 1) % tags.Length;
        tagSprite.sprite = tags[basket.tag];
        if(TagChanged != null) TagChanged();
    }
}
