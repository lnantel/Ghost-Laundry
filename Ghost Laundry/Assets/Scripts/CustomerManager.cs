using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    public static Action<Customer> CustomerSpawned;
    public static Action<LaundromatBasket> CustomerServed; //Called when the player picks up a customer's laundry basket from the counter
    public static Action<Customer> CustomerLeft;
    public static Action<CustomerSpot, Customer> SpotAssigned;

    public Transform CustomerSpawnPoint;
    public Transform Entrance;
    public Transform PickUpPosition;
    public CustomerSpot[] CounterSpots;
    public CustomerSpot[] WaitingSpots;
    public CustomerSpot[] QueueSpots;

    //TODO: Link this to Reputation
    public float CustomerSpawnDelay;

    public int CustomerCapacity;
    public List<Customer> customersInLaundromat;

    private GameObject customerPrefab;
    private float customerSpawningTimer;
    private int currentTicketNumber = 1;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        customersInLaundromat = new List<Customer>();
        customerPrefab = (GameObject)Resources.Load("Customer");
        ResetTicketNumber();
        SpawnCustomer();
        customerSpawningTimer = 0;

        //Assign a unique ID to each CustomerSpot, for comparison purposes
        int id = 0;
        foreach (CustomerSpot spot in CounterSpots)
            spot.ID = id++;
        foreach (CustomerSpot spot in WaitingSpots)
            spot.ID = id++;
        foreach (CustomerSpot spot in QueueSpots)
            spot.ID = id++;
    }

    private void OnEnable() {
        CustomerLeft += OnCustomerLeft;
        CustomerSpot.Freed += OnCustomerSpotFreed;
    }

    private void OnDisable() {
        CustomerLeft -= OnCustomerLeft;
        CustomerSpot.Freed -= OnCustomerSpotFreed;
    }

    public int GetTicketNumber() {
        return currentTicketNumber++;
    }

    private void SpawnCustomer() {
        if(customersInLaundromat.Count < CustomerCapacity) {
            Customer customer = Instantiate(customerPrefab, CustomerSpawnPoint.position, CustomerSpawnPoint.rotation).GetComponent<Customer>();
            customersInLaundromat.Add(customer);
            customerSpawningTimer = 0;
            if (CustomerSpawned != null) CustomerSpawned(customer);
        }
    }

    public void ResetTicketNumber() {
        currentTicketNumber = 1;
    }

    private void OnCustomerLeft(Customer customer) {
        customersInLaundromat.Remove(customer);
        Destroy(customer.gameObject);
    }

    private void Update() {
        customerSpawningTimer += Time.deltaTime;
        if (customerSpawningTimer >= CustomerSpawnDelay)
            SpawnCustomer();

        //If any counter spot is unclaimed, assign it to the first customer in the queue
        for(int i = 0; i < CounterSpots.Length; i++) {
            if (!CounterSpots[i].Claimed) {
                Customer customer = GetFirstCustomerInQueue();
                if(customer != null && SpotAssigned != null) {
                    SpotAssigned(CounterSpots[i], customer);
                    //TODO: Signal to customer that it has a counter spot
                    customer.state = CustomerState.Arriving;
                }
            }
        }
    }

    private Customer GetFirstCustomerInQueue() {
        for(int i = 0; i < QueueSpots.Length; i++) {
            if (QueueSpots[i].Claimed) return QueueSpots[i].customer;
        }
        return null;
    }

    public void AssignQueueSpot(Customer customer) {
        for(int i = 0; i < QueueSpots.Length; i++) {
            if (!QueueSpots[i].Claimed && SpotAssigned != null) {
                SpotAssigned(QueueSpots[i], customer);
                break;
            }
        }
    }

    public void AssignRandomWaitingSpot(Customer customer) {
        //TODO: Make the waiting spot random!
        for (int i = 0; i < WaitingSpots.Length; i++) {
            if (!WaitingSpots[i].Claimed && SpotAssigned != null) {
                SpotAssigned(WaitingSpots[i], customer);
                break;
            }
        }
    }

    private void OnCustomerSpotFreed(CustomerSpot spot) {
        //If the spot is in QueueSpots:
        //  assign the spot to the next customer in queue
        for(int i = 0; i < QueueSpots.Length - 1; i++) {
            if (QueueSpots[i].Equals(spot)) {
                if (QueueSpots[i + 1].Claimed && SpotAssigned != null) SpotAssigned(spot, QueueSpots[i + 1].customer);
            }
        }
    }
}
