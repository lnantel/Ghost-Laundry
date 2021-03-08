using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Customer : MonoBehaviour
{
    public static Action<int, int, Customer> Pay;
    public static Action Ragequit;
    public static Action<LaundromatBag> BagPickedUp;

    public int ticketNumber;
    public List<Garment> garments;
    public Basket basket;

    public float MaximumWaitingTime;
    public bool impatient;
    protected float waitTimer;

    public Sprite[] silhouettes;

    public CustomerState state;

    public CustomerSpot spot;

    public float speed;

    protected GameObject laundromatBasketPrefab;
    protected LaundromatBasket basketOnCounter;
    protected LaundromatBag bagOnCounter;
    protected Animator animator;

    protected virtual void Start() {
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");
        animator = GetComponentInChildren<Animator>();

        //Generate random customer
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = silhouettes[UnityEngine.Random.Range(0, silhouettes.Length)];

        ticketNumber = CustomerManager.instance.GetTicketNumber();

        waitTimer = 0;

        state = CustomerState.Queueing;

        //Generate random laundry
        basket = new Basket();
        garments = new List<Garment>();

        int garmentCount = UnityEngine.Random.Range(5, 9);
        for(int i = 0; i < garmentCount; i++) {
            Garment garment = Garment.GetRandomGarment();
            garment.customerID = ticketNumber;
            garments.Add(garment);
            if(garment is GarmentSock) {
                GarmentSock otherSock = new GarmentSock((GarmentSock)garment);
                garments.Add(otherSock);
                i++;
            }
        }

        //Shuffle clothing to separate socks
        int n = garments.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Garment value = garments[k];
            garments[k] = garments[n];
            garments[n] = value;
        }

        foreach (Garment garment in garments)
            basket.AddGarment(garment);

        //Upon arrival, a new Customer requests a spot in Queue from the CustomerManager
        CustomerManager.instance.AssignQueueSpot(this);
    }

    protected virtual void OnEnable() {
        CustomerManager.CustomerServed += OnCustomerServed;
        CustomerManager.SpotAssigned += OnSpotAssigned;
        PickUpCounter.BagReadyForPickUp += OnClothesReady;
    }

    protected virtual void OnDisable() {
        CustomerManager.CustomerServed -= OnCustomerServed;
        CustomerManager.SpotAssigned += OnSpotAssigned;
        PickUpCounter.BagReadyForPickUp -= OnClothesReady;
    }

    protected virtual void OnCustomerServed(LaundromatBasket laundromatBasket) {
        if(state == CustomerState.WaitingForService && basketOnCounter != null && laundromatBasket.GetInstanceID() == basketOnCounter.GetInstanceID()) {
            state = CustomerState.WaitingForClothes;
            CustomerManager.instance.AssignRandomWaitingSpot(this);
            impatient = false;
        }
    }

    protected virtual void OnSpotAssigned(CustomerSpot newSpot, Customer customer) {
        if(customer.GetInstanceID() == GetInstanceID()) {
            if(spot != null) spot.Free();
            spot = newSpot;
            newSpot.customer = this;
        }
    }

    protected virtual void OnClothesReady(LaundromatBag bag) {
        if(bag.customerID == ticketNumber) {
            state = CustomerState.PickingUpBag;
            spot.Free();
            bagOnCounter = bag;
        }
    }

    protected virtual void PlaceBasketOnCounter() {
        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, spot.position + Vector3.up * 1.3f, transform.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
        basketOnCounter = laundromatBasket;
    }

    protected virtual void RemoveBasketFromCounter() {
        Destroy(basketOnCounter.gameObject);
        basketOnCounter = null;
    }

    protected virtual void PickUpBag() {
        BagPickedUp(bagOnCounter);
        Destroy(bagOnCounter.gameObject);
        bagOnCounter = null;
    }

    //Returns true when the customer has reached the given destination
    protected virtual bool MoveTowards(Vector3 destination) {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * TimeManager.instance.deltaTime);
        return (transform.position - destination).magnitude < 0.1f;
    }

    protected virtual void Queueing() {
        MoveTowards(spot.position);
    }

    protected virtual void Arriving() {
        if (MoveTowards(spot.position)) {
            PlaceBasketOnCounter();
            state = CustomerState.WaitingForService;
        }
    }

    protected virtual void WaitingForService() {
        waitTimer += TimeManager.instance.deltaTime;
        if (waitTimer > 0.7f * MaximumWaitingTime) {
            impatient = true;
            animator.SetBool("Impatient", impatient);
        }
        if (waitTimer >= MaximumWaitingTime) {
            RemoveBasketFromCounter();
            spot.Free();
            state = CustomerState.Ragequitting;
        }
    }

    protected virtual void WaitingForClothes() {
        impatient = false;
        animator.SetBool("Impatient", impatient);
        MoveTowards(spot.position);
    }

    protected virtual void PickingUpBag() {
        if (MoveTowards(CustomerManager.instance.PickUpPosition.position)) {
            PickUpBag();
            state = CustomerState.Leaving;
        }
    }

    protected virtual void Leaving() {
        if (MoveTowards(CustomerManager.instance.Entrance.position)) {
            state = CustomerState.HasLeft;
        }
    }

    protected virtual void Ragequitting() {
        if (MoveTowards(CustomerManager.instance.Entrance.position)) {
            state = CustomerState.HasLeft;
            if (Ragequit != null) Ragequit();
        }
    }

    protected virtual void HasLeft() {
        CustomerManager.CustomerLeft(this);
    }

    protected virtual void Update() {
        switch (state) {
            case CustomerState.Queueing:
                Queueing();
                break;
            case CustomerState.Arriving:
                Arriving();
                break;
            case CustomerState.WaitingForService:
                WaitingForService();
                break;
            case CustomerState.WaitingForClothes:
                WaitingForClothes();
                break;
            case CustomerState.PickingUpBag:
                PickingUpBag();
                break;
            case CustomerState.Leaving:
                Leaving();
                break;
            case CustomerState.Ragequitting:
                Ragequitting();
                break;
            case CustomerState.HasLeft:
                HasLeft();
                break;
        }
    }
}

public enum CustomerState {
    Queueing,
    Arriving,
    WaitingForService,
    WaitingForClothes,
    PickingUpBag,
    Leaving,
    Ragequitting,
    HasLeft
}
