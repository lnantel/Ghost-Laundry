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

    public float SpawnTime;
    public float SpawnTimeReductionPerStar;

    public List<Customer> customersInLaundromat;

    //TODO: One for each specific customer
    private GameObject recurringCustomerPrefab;

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
        recurringCustomerPrefab = (GameObject)Resources.Load("RecurringCustomer");
        ResetTicketNumber();
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
        TimeManager.StartOfDay += OnStartOfDay;
    }

    private void OnDisable() {
        CustomerLeft -= OnCustomerLeft;
        CustomerSpot.Freed -= OnCustomerSpotFreed;
        TimeManager.StartOfDay -= OnStartOfDay;
    }

    public int GetTicketNumber() {
        return currentTicketNumber++;
    }

    private void SpawnCustomer() {
        if(customersInLaundromat.Count < 10) {
            Customer customer = Instantiate(customerPrefab, CustomerSpawnPoint.position, CustomerSpawnPoint.rotation).GetComponent<Customer>();
            customersInLaundromat.Add(customer);
            customerSpawningTimer = 0;
            AudioManager.instance.PlaySound(SoundName.CustomerArrives);
            if (CustomerSpawned != null) CustomerSpawned(customer);
        }
    }

    //TODO: Spawn different recurring customers!
    public void SpawnRecurringCustomer() {
        AudioManager.instance.PlaySound(SoundName.OllieArrives);
        Customer customer = Instantiate(recurringCustomerPrefab, CustomerSpawnPoint.position, CustomerSpawnPoint.rotation).GetComponent<Customer>();
        customersInLaundromat.Add(customer);
        customerSpawningTimer = 0;
        if (CustomerSpawned != null) CustomerSpawned(customer);
    }

    public RecurringCustomer GetRecurringCustomer(int characterIndex) {
        foreach(Customer customer in customersInLaundromat) {
            if(customer is RecurringCustomer) {
                return ((RecurringCustomer) customer);
            }
        }
        return null;
    }

    public void ResetTicketNumber() {
        currentTicketNumber = 1;
    }

    private void OnCustomerLeft(Customer customer) {
        customersInLaundromat.Remove(customer);
        Destroy(customer.gameObject);
    }

    //Spawn timer formula: 1 customer every X seconds - Y seconds per star
    private float CustomerSpawnRate() {
        return SpawnTime - (ReputationManager.instance.CurrentAmount / ReputationManager.instance.AmountPerStar) * SpawnTimeReductionPerStar;
    }

    private void Update() {
        customerSpawningTimer += TimeManager.instance.deltaTime;
        if (customerSpawningTimer >= CustomerSpawnRate() && TimeManager.instance.CurrentTime()[0] < 22) {
            if (TimeManager.instance.CurrentDay != 0)
                SpawnCustomer();
        }

        if (TimeManager.instance.TimeIsPassing && TimeManager.instance.CurrentTime()[0] >= 22 && customersInLaundromat.Count == 0) {
            TimeManager.instance.EndDay();
        }

        //If any counter spot is unclaimed, assign it to the first customer in the queue
        for (int i = 0; i < CounterSpots.Length; i++) {
            if (!CounterSpots[i].Claimed) {
                Customer customer = GetFirstCustomerInQueue();
                if(customer != null && SpotAssigned != null) {
                    SpotAssigned(CounterSpots[i], customer);
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

    private void OnStartOfDay(int day) {
        if (day != 0) {
            int numberOfCustomers = 2 + Mathf.FloorToInt(ReputationManager.instance.CurrentAmount / 200.0f);
            for(int i = 0; i < numberOfCustomers; i++) {
                SpawnCustomer();
            }
        }
    }
}
