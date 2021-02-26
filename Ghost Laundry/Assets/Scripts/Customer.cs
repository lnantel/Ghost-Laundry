using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //public string firstname;
    //public string lastname;
    public int ticketNumber;
    public Basket basket;

    public float MaximumWaitingTime;
    public bool impatient;
    private float waitTimer;

    public int head;
    public int body;
    public int legs;

    public CustomerState state;

    //TODO: Get these from the CustomerManager instead
    //public Transform counter;
    //public Transform door;
    //public Transform waitingSpot;
    //public Transform basketSpot;

    public CustomerSpot spot;

    public float speed;

    private GameObject laundromatBasketPrefab;
    private LaundromatBasket basketOnCounter;

    private void Start() {
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");

        //Generate random customer
        head = Random.Range(0, 3);
        body = Random.Range(0, 3);
        legs = Random.Range(0, 3);

        ticketNumber = CustomerManager.instance.GetTicketNumber();

        waitTimer = 0;

        state = CustomerState.Queueing;

        //Generate random laundry
        basket = new Basket();
        int garmentCount = Random.Range(5, 8);
        for(int i = 0; i < garmentCount; i++) {
            Garment garment = Garment.GetRandomGarment();
            garment.customerID = "#" + ticketNumber.ToString("D3");
            basket.AddGarment(garment);
            if(garment is GarmentSock) {
                basket.AddGarment(new GarmentSock((GarmentSock)garment));
                i++;
            }
        }

        //Upon arrival, a new Customer requests a spot in Queue from the CustomerManager
        CustomerManager.instance.AssignQueueSpot(this);
    }

    private void OnEnable() {
        CustomerManager.CustomerServed += OnCustomerServed;
        CustomerManager.SpotAssigned += OnSpotAssigned;
    }

    private void OnDisable() {
        CustomerManager.CustomerServed -= OnCustomerServed;
        CustomerManager.SpotAssigned += OnSpotAssigned;
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
            spot.Free();
            spot = newSpot;
            newSpot.customer = this;
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
                    state = CustomerState.Ragequitting;
                } 
                break;
            case CustomerState.WaitingForClothes:
                MoveTowards(spot.position);
                //TODO: Detect when it's time to go pick up bag
                break;
            case CustomerState.PickingUpBag:
                if (MoveTowards(spot.position)) {
                    RemoveBasketFromCounter();
                    state = CustomerState.Leaving;
                }
                break;
            case CustomerState.Leaving:
                if (MoveTowards(spot.position)) {
                    state = CustomerState.HasLeft;
                }
                break;
            case CustomerState.Ragequitting:
                if (MoveTowards(spot.position)) {
                    state = CustomerState.HasLeft;
                }
                break;
            case CustomerState.HasLeft:
                if(CustomerManager.CustomerLeft != null) CustomerManager.CustomerLeft();
                Destroy(gameObject);
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
