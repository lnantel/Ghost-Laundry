using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WorkStation : Interactable, ITrackable
{
    public static Action<LaundryGarment> LaundryGarmentReleased;
    public static Action BasketSlotsChanged;
    public static Action RequestCarriedBasket;

    public bool HasGravity;
    public BasketSlot[] basketSlots;

    [HideInInspector]
    public GameObject laundryTaskArea;

    public List<LaundryGarment> containedLaundryGarments;

    protected GameObject areaPrefab;

    protected int basketCapacity;

    protected GameObject laundryBasketPrefab;

    protected override void Start() {
        base.Start();
        
        laundryBasketPrefab = (GameObject)Resources.Load("LaundryBasket");
        basketCapacity = basketSlots.Length;

        containedLaundryGarments = new List<LaundryGarment>();

        if (areaPrefab == null) areaPrefab = (GameObject)Resources.Load("LaundryTaskArea");
        laundryTaskArea = Instantiate(areaPrefab, new Vector3(300.0f, 0.0f, 0.0f), Quaternion.identity, transform);

        laundryTaskArea.SetActive(false);
        StartCoroutine(Initialize());
    }

    public virtual IEnumerator Initialize() {
        yield return null;
        LaundryBasket[] laundryBaskets = laundryTaskArea.GetComponentsInChildren<LaundryBasket>();
        foreach (LaundryBasket basket in laundryBaskets)
            AddBasket(basket);
        if(BasketSlotsChanged != null) BasketSlotsChanged();

        for (int i = 0; i < basketCapacity; i++) {
            AddBasket(new Basket(), i);
        }
    }

    protected override void Interaction() {
        //if player is carrying a basket, attempt to input it to the workstation
        //if input fails, cancel interaction
        if(RequestCarriedBasket != null) RequestCarriedBasket();

        PlayerController.instance.enabled = false;
        laundryTaskArea.SetActive(true);
        LaundryTaskController.instance.gameObject.SetActive(true);
        LaundryTaskController.instance.activeWorkStation = this;
        TaskView.instance.PopUp(transform.position);
        LaundryTaskController.exitedTask += OnTaskExit;
        LaundryGarment.Released += OnLaundryGarmentReleased;
    }

    protected void UpdateContainedLaundryGarments() {
        containedLaundryGarments = new List<LaundryGarment>();
        LaundryGarment[] laundryGarments = GetComponentsInChildren<LaundryGarment>();
        for (int i = 0; i < laundryGarments.Length; i++) {
            if (!containedLaundryGarments.Contains(laundryGarments[i])) {
                containedLaundryGarments.Add(laundryGarments[i]);
            }
        }
    }

    protected virtual void OnTaskExit() {
        LaundryTaskController.exitedTask -= OnTaskExit;
        LaundryGarment.Released -= OnLaundryGarmentReleased;

        PlayerController.instance.enabled = true;
        TaskView.instance.Minimize(transform.position, laundryTaskArea);
        UpdateContainedLaundryGarments();
    }

    public virtual Basket OutputBasket(int basketSlotIndex) {
        //if (BasketCount() > basketSlotIndex) {
        //    //Get basket
        //    Basket basket = null;
        //    if (basketSlots[basketSlotIndex].laundryBasket != null && !basketSlots[basketSlotIndex].Locked) {
        //        basket = basketSlots[basketSlotIndex].laundryBasket.basket;
        //    }

        //    //Destroy it
        //    Destroy(basketSlots[basketSlotIndex].laundryBasket.gameObject);
        //    basketSlots[basketSlotIndex].laundryBasket = null;

        //    //return the basket
        //    if (BasketSlotsChanged != null) BasketSlotsChanged();
        //    return basket;
        //}
        //return null;

        //Returns an output basket
        //Empties the corresponding basket in the TableArea but does not destroy it
        if (basketSlots[basketSlotIndex].laundryBasket != null && basketSlots[basketSlotIndex].laundryBasket.basket.contents.Count > 0 && !basketSlots[basketSlotIndex].Locked) {
            Basket outputBasket = new Basket();
            outputBasket.contents = basketSlots[basketSlotIndex].laundryBasket.basket.contents;
            outputBasket.positions = basketSlots[basketSlotIndex].laundryBasket.basket.positions;
            outputBasket.tag = basketSlots[basketSlotIndex].laundryBasket.basket.tag;

            basketSlots[basketSlotIndex].laundryBasket.basket.RemoveAll();
            if (BasketSlotsChanged != null) BasketSlotsChanged();
            return outputBasket;
        }
        return null;
    }

    public virtual bool InputBasket(Basket basket, int i) {
        ////if the workstation has space for a basket
        //if (BasketCount() < basketCapacity) {
        //    return AddBasket(basket, index);
        //}
        //return false;

        //Returns true if the input basket is empty, and replaces it with 'basket'
        //If the input basket is not empty, returns false
        if (basketSlots[i].laundryBasket != null && basketSlots[i].laundryBasket.basket.contents.Count > 0 && !basketSlots[i].Locked) return false;
        else {
            if (basketSlots[i].laundryBasket != null) basketSlots[i].laundryBasket.basket = basket;
            if (BasketSlotsChanged != null) BasketSlotsChanged();
            return true;
        }
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
            if (basketSlots[i].laundryBasket == null && !basketSlots[i].Locked) {
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
    protected virtual bool AddBasket(Basket basket, int i) {
        //for (int i = 0; i < basketSlots.Length; i++) {
            if(basketSlots[i].laundryBasket == null && !basketSlots[i].Locked) {
                LaundryBasket laundryBasket = Instantiate(laundryBasketPrefab, 
                    laundryTaskArea.transform.position + basketSlots[i].spawnPoint, 
                    laundryTaskArea.transform.rotation, laundryTaskArea.transform).GetComponent<LaundryBasket>();
                laundryBasket.basket = basket;
                basketSlots[i].laundryBasket = laundryBasket;
                if (BasketSlotsChanged != null) BasketSlotsChanged();
                return true;
            }
        //}
        return false;
    }

    protected virtual int BasketCount() {
        int count = 0;
        for(int i = 0; i < basketSlots.Length; i++) {
            if (basketSlots[i].laundryBasket != null) count++;
        }
        return count;
    }

    public List<Garment> GetAllContainedGarments() {
        List<Garment> containedGarments = new List<Garment>();

        if (laundryTaskArea.activeSelf) {
            UpdateContainedLaundryGarments();
        }

        for (int i = 0; i < containedLaundryGarments.Count; i++) {
            if (!containedGarments.Contains(containedLaundryGarments[i].garment)) {
                containedGarments.Add(containedLaundryGarments[i].garment);
            }
        }


        List<Garment> garmentsInCustomContainers = GetCustomContainerGarments();

        for(int i = 0; i < garmentsInCustomContainers.Count; i++) {
            containedGarments.Add(garmentsInCustomContainers[i]);
        }

        return containedGarments;
    }

    //Override this to return a list of garments contained by machines, the workstation itself, etc.
    protected virtual List<Garment> GetCustomContainerGarments() {
        return new List<Garment>();
    }

    public bool ContainsAGarmentInBasket(params Garment[] garments) {
        if (ContainsAGarment(garments)) return true;
        for (int i = 0; i < basketSlots.Length; i++) {
            if (basketSlots[i].laundryBasket != null && basketSlots[i].laundryBasket.ContainsAGarment(garments)) return true;
        }
        return false;
    }

    public bool ContainsAllGarmentsInBasket(params Garment[] garments) {
        for (int i = 0; i < garments.Length; i++) {
            if (!ContainsAGarmentInBasket(garments[i])) {
                return false;
            }
        }
        return true;
    }

    public bool ContainsAGarment(params Garment[] garments) {
        List<Garment> containedGarments = GetAllContainedGarments();
        for (int i = 0; i < garments.Length; i++) {
            if (containedGarments.Contains(garments[i]))
                return true;
        }
        return false;
    }

    public bool ContainsAllGarments(params Garment[] garments) {
        List<Garment> containedGarments = GetAllContainedGarments();
        for (int i = 0; i < garments.Length; i++) {
            if (!containedGarments.Contains(garments[i]))
                return false;
        }
        return true;
    }

    public bool ContainsTrackedGarment() {
        if(CustomerTracker.TrackedCustomer != null) {
            return ContainsAGarment(CustomerTracker.TrackedCustomer.garments.ToArray());
        }
        return false;
    }

    //public override void Lock() {
    //    base.Lock();
    //    for(int i = 0; i < basketSlots.Length; i++) {
    //        if (basketSlots[i] != null) basketSlots[i].Lock();
    //    }
    //}

    //public override void Unlock() {
    //    base.Unlock();
    //    for (int i = 0; i < basketSlots.Length; i++) {
    //        if(basketSlots[i] != null) basketSlots[i].Unlock();
    //    }
    //}
}
