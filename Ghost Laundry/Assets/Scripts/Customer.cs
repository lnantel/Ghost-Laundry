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

    public Transform counter;
    public Transform door;
    public Transform waitingSpot;
    public Transform basketSpot;

    public float speed;
    private Vector3 currentDestination;

    private GameObject laundromatBasketPrefab;

    private void Start() {
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");

        //Generate random customer
        head = Random.Range(0, 3);
        body = Random.Range(0, 3);
        legs = Random.Range(0, 3);

        ticketNumber = GameManager.instance.GetTicketNumber();

        waitTimer = 0;

        state = CustomerState.Arriving;

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
    }

    private void PlaceBasketOnCounter() {
        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, basketSpot.position, basketSpot.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
    }

    private void RemoveBasketFromCounter() {

    }

    //Returns true when the customer has reached the given destination
    private bool MoveTowards(Vector3 destination) {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        return (transform.position - destination).magnitude < 0.1f;
    }

    private void Update() {
        switch (state) {
            case CustomerState.Arriving:
                if (MoveTowards(counter.position)) {
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
                MoveTowards(waitingSpot.position);
                //TODO: Detect when it's time to go pick up bag
                break;
            case CustomerState.PickingUpBag:
                if (MoveTowards(counter.position)) {
                    RemoveBasketFromCounter();
                    state = CustomerState.Leaving;
                }
                break;
            case CustomerState.Leaving:
                if (MoveTowards(door.position)) {
                    state = CustomerState.HasLeft;
                }
                break;
            case CustomerState.Ragequitting:
                if (MoveTowards(door.position)) {
                    state = CustomerState.HasLeft;
                }
                break;
            case CustomerState.HasLeft:
                Destroy(gameObject);
                break;
        }
    }
}

public enum CustomerState {
    Arriving,
    WaitingForService,
    WaitingForClothes,
    PickingUpBag,
    Leaving,
    Ragequitting,
    HasLeft
}
