using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WorkStation : Interactable
{
    public static Action<LaundryGarment> LaundryGarmentReleased;
    public static Action BasketSlotsChanged;

    public bool HasGravity;
    public BasketSlot[] basketSlots;

    [HideInInspector]
    public GameObject laundryTaskArea;

    protected int basketCapacity;

    protected GameObject laundryBasketPrefab;

    protected virtual void Start() {
        //containedBaskets = new List<LaundryBasket>();
        laundryBasketPrefab = (GameObject)Resources.Load("LaundryBasket");
        basketCapacity = basketSlots.Length;
    }

    public virtual void Initialize() {
        LaundryBasket[] laundryBaskets = laundryTaskArea.GetComponentsInChildren<LaundryBasket>();
        foreach (LaundryBasket basket in laundryBaskets)
            AddBasket(basket);
        if(BasketSlotsChanged != null) BasketSlotsChanged();
    }

    public override void Interact() {
        //TODO:
        //if player is carrying a basket, attempt to input it to the workstation
        //if input fails, cancel interaction

        PlayerController.instance.enabled = false;
        laundryTaskArea.SetActive(true);
        LaundryTaskController.instance.gameObject.SetActive(true);
        LaundryTaskController.instance.activeWorkStation = this;
        TaskView.instance.PopUp(transform.position);
        LaundryTaskController.exitedTask += OnTaskExit;
        LaundryGarment.Released += OnLaundryGarmentReleased;
    }

    protected virtual void OnTaskExit() {
        LaundryTaskController.exitedTask -= OnTaskExit;
        LaundryGarment.Released -= OnLaundryGarmentReleased;
        TaskView.instance.Minimize(transform.position);
        laundryTaskArea.SetActive(false);
        PlayerController.instance.enabled = true;
    }

    public virtual Basket OutputBasket() {
        //if the workstation contains at least one basket
        if (BasketCount() > 0) {
            //Get the last non-null LaundryBasket in BasketSlots
            int basketIndex = 0;
            Basket basket = null;
            for(int i = 0; i < basketSlots.Length; i++) {
                if(basketSlots[i].laundryBasket != null) {
                    basketIndex = i;
                    basket = basketSlots[i].laundryBasket.basket;
                }
            }

            //Destroy it
            Destroy(basketSlots[basketIndex].laundryBasket.gameObject);
            basketSlots[basketIndex].laundryBasket = null;

            //return the basket
            if (BasketSlotsChanged != null) BasketSlotsChanged();
            return basket;
        }
        return null;
    }

    public virtual bool InputBasket(Basket basket) {
        //if the workstation has space for a basket
        if (BasketCount() < basketCapacity) {
            return AddBasket(basket);
        }
        return false;
    }

    protected virtual void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        if(HasGravity)
            laundryGarment.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        else
            laundryGarment.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        if(LaundryGarmentReleased != null)
            LaundryGarmentReleased(laundryGarment);
    }

    public virtual bool ContainsBasket() {
        return BasketCount() > 0;
    }

    //Adds an existing LaundryBasket in the first free BasketSlot
    //Returns true if successful, false otherwise
    protected virtual bool AddBasket(LaundryBasket laundryBasket) {
        for(int i = 0; i < basketSlots.Length; i++) {
            if (basketSlots[i].laundryBasket == null) {
                basketSlots[i].laundryBasket = laundryBasket;
                laundryBasket.transform.parent = laundryTaskArea.transform;
                laundryBasket.transform.localPosition = basketSlots[i].spawnPoint;
                if (BasketSlotsChanged != null) BasketSlotsChanged();
                return true;
            }
        }
        return false;
    }

    //Adds a Basket to the first free BasketSlot and Instantiates a new LaundryBasket
    //Returns true if successful, false otherwise
    protected virtual bool AddBasket(Basket basket) {
        for (int i = 0; i < basketSlots.Length; i++) {
            if(basketSlots[i].laundryBasket == null) {
                LaundryBasket laundryBasket = Instantiate(laundryBasketPrefab, 
                    laundryTaskArea.transform.position + basketSlots[i].spawnPoint, 
                    laundryTaskArea.transform.rotation, laundryTaskArea.transform).GetComponent<LaundryBasket>();
                laundryBasket.basket = basket;
                basketSlots[i].laundryBasket = laundryBasket;
                if (BasketSlotsChanged != null) BasketSlotsChanged();
                return true;
            }
        }
        return false;
    }

    protected virtual int BasketCount() {
        int count = 0;
        for(int i = 0; i < basketSlots.Length; i++) {
            if (basketSlots[i].laundryBasket != null) count++;
        }
        return count;
    }
}
