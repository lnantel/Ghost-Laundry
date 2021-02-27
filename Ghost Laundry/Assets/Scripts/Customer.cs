using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Customer : MonoBehaviour
{
    public static Action<int, int, Customer> Pay;

    public int ticketNumber;
    public List<Garment> garments;
    public Basket basket;

    public float MaximumWaitingTime;
    public bool impatient;
    private float waitTimer;

    public int head;
    public int body;
    public int legs;

    public CustomerState state;

    public CustomerSpot spot;

    public float speed;

    private GameObject laundromatBasketPrefab;
    private LaundromatBasket basketOnCounter;
    private LaundromatBag bagOnCounter;

    private void Start() {
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");

        //Generate random customer
        head = UnityEngine.Random.Range(0, 3);
        body = UnityEngine.Random.Range(0, 3);
        legs = UnityEngine.Random.Range(0, 3);

        ticketNumber = CustomerManager.instance.GetTicketNumber();

        waitTimer = 0;

        state = CustomerState.Queueing;

        //Generate random laundry
        basket = new Basket();
        garments = new List<Garment>();

        int garmentCount = UnityEngine.Random.Range(5, 8);
        for(int i = 0; i < garmentCount; i++) {
            Garment garment = Garment.GetRandomGarment();
            garment.customerID = ticketNumber;
            garments.Add(garment);
            basket.AddGarment(garment);
            if(garment is GarmentSock) {
                GarmentSock otherSock = new GarmentSock((GarmentSock)garment);
                basket.AddGarment(otherSock);
                garments.Add(otherSock);
                i++;
            }
        }

        //Upon arrival, a new Customer requests a spot in Queue from the CustomerManager
        CustomerManager.instance.AssignQueueSpot(this);
    }

    private void OnEnable() {
        CustomerManager.CustomerServed += OnCustomerServed;
        CustomerManager.SpotAssigned += OnSpotAssigned;
        PickUpCounter.BagReadyForPickUp += OnClothesReady;
    }

    private void OnDisable() {
        CustomerManager.CustomerServed -= OnCustomerServed;
        CustomerManager.SpotAssigned += OnSpotAssigned;
        PickUpCounter.BagReadyForPickUp -= OnClothesReady;
    }

    private void OnCustomerServed(LaundromatBasket laundromatBasket) {
        if(state == CustomerState.WaitingForService && laundromatBasket.GetInstanceID() == basketOnCounter.GetInstanceID()) {
            state = CustomerState.WaitingForClothes;
            CustomerManager.instance.AssignRandomWaitingSpot(this);
            impatient = false;
        }
    }

    private void OnSpotAssigned(CustomerSpot newSpot, Customer customer) {
        if(customer.GetInstanceID() == GetInstanceID()) {
            if(spot != null) spot.Free();
            spot = newSpot;
            newSpot.customer = this;
        }
    }

    private void OnClothesReady(LaundromatBag bag) {
        if(bag.ticketNumber == ticketNumber) {
            state = CustomerState.PickingUpBag;
            spot.Free();
            bagOnCounter = bag;
        }
    }

    private void PlaceBasketOnCounter() {
        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, spot.position + Vector3.up, transform.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
        basketOnCounter = laundromatBasket;
    }

    private void RemoveBasketFromCounter() {
        Destroy(basketOnCounter.gameObject);
        basketOnCounter = null;
    }

    private void PickUpBag() {
        int fee = 0;
        int tip = 0;

        //TODO: Better money logic
        if (bagOnCounter.launderedGarments == bagOnCounter.totalGarments) {
            fee = 15;
            tip = Mathf.CeilToInt(5.0f * ((float)bagOnCounter.perfectGarments / bagOnCounter.totalGarments));
        }

        Pay(fee, tip, this);
        Destroy(bagOnCounter.gameObject);
        bagOnCounter = null;
    }

    //Returns true when the customer has reached the given destination
    private bool MoveTowards(Vector3 destination) {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        return (transform.position - destination).magnitude < 0.1f;
    }

    private void Update() {
        switch (state) {
            case CustomerState.Queueing:
                MoveTowards(spot.position);
                break;
            case CustomerState.Arriving:
                if (MoveTowards(spot.position)) {
                    PlaceBasketOnCounter();
                    state = CustomerState.WaitingForService;
                }
                break;
            case CustomerState.WaitingForService:
                waitTimer += Time.deltaTime;
                if (waitTimer > 0.7f * MaximumWaitingTime) impatient = true;
                if (waitTimer >= MaximumWaitingTime) {
                    RemoveBasketFromCounter();
                    spot.Free();
                    state = CustomerState.Ragequitting;
                } 
                break;
            case CustomerState.WaitingForClothes:
                MoveTowards(spot.position);
                break;
            case CustomerState.PickingUpBag:
                if (MoveTowards(CustomerManager.instance.PickUpPosition.position)) {
                    PickUpBag();
                    state = CustomerState.Leaving;
                }
                break;
            case CustomerState.Leaving:
                if (MoveTowards(CustomerManager.instance.Entrance.position)) {
                    state = CustomerState.HasLeft;
                }
                break;
            case CustomerState.Ragequitting:
                if (MoveTowards(CustomerManager.instance.Entrance.position)) {
                    state = CustomerState.HasLeft;
                }
                break;
            case CustomerState.HasLeft:
                CustomerManager.CustomerLeft(this);
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
