using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bagger : WorkStation
{
    public static Action BasketInput;
    public static Action BagOutput;

    public Transform bagSpawnPoint;

    private GameObject laundromatBagPrefab;
    private List<Garment> contents;

    private IEnumerator OutputCoroutine;

    private TutorialManager tutorialManager;

    private struct OutputData {
        public Customer customer;
        public List<Garment> customersGarments;
    }

    private List<OutputData> OutputQueue;

    protected override void Start() {
        base.Start();
        contents = new List<Garment>();
        laundromatBagPrefab = (GameObject)Resources.Load("LaundromatBag");
        OutputQueue = new List<OutputData>();
        if (TimeManager.instance.CurrentDay == 0) tutorialManager = FindObjectOfType<TutorialManager>();
    }

    protected override void Interaction() {
        if(RequestCarriedBasket != null) RequestCarriedBasket();
    }

    public override bool InputBasket(Basket basket) {
        AudioManager.instance.PlaySound(Sounds.DropGarmentEmb);
        foreach (Garment garment in basket.contents) {
            contents.Add(garment);
            if(garment is GarmentSock garmentSock && garment.Folded) {
                contents.Add(garmentSock.pairedSock);
            }
        }
        if (BasketInput != null) BasketInput();
        if (TimeManager.instance.CurrentDay == 0) TutorialCheckContentsForOutput();
        else CheckContentsForOutput();
        return true;
    }

    private void TutorialCheckContentsForOutput() {
        for(int i = 0; i < tutorialManager.tutorialCustomers.Count; i++) {
            bool baggerContainsAll = true;
            List<Garment> customersGarments = new List<Garment>();
            for(int j = 0; j < tutorialManager.tutorialCustomers[i].Count; j++) {
                if (!contents.Contains(tutorialManager.tutorialCustomers[i][j])) baggerContainsAll = false;
                else customersGarments.Add(tutorialManager.tutorialCustomers[i][j]);
            }

            if (baggerContainsAll) {
                if (OutputCoroutine == null) {
                    OutputCoroutine = OutputBag(null, customersGarments);
                    StartCoroutine(OutputCoroutine);
                }
                else {
                    //Add output to queue if it isn't already there
                    bool alreadyInQueue = false;
                    for (int k = 0; k < OutputQueue.Count; k++) {
                        if (OutputQueue[k].customersGarments[0].customerID == i) {
                            alreadyInQueue = true;
                            break;
                        }
                    }
                    if (!alreadyInQueue) {
                        OutputData output = new OutputData();
                        output.customer = null;
                        output.customersGarments = customersGarments;
                        OutputQueue.Add(output);
                    }
                }
            }
        }
    }

    private void CheckContentsForOutput() {
        List<Customer> customers = CustomerManager.instance.customersInLaundromat;
        foreach(Customer customer in customers) {
            List<Garment> customersGarments = new List<Garment>();
            int garmentCount = 0;
            foreach(Garment garment in contents) {
                if(garment.customerID == customer.ticketNumber) {
                    customersGarments.Add(garment);
                    garmentCount++;
                }
            }
            if(garmentCount != 0 && garmentCount == customer.garments.Count) {
                if(OutputCoroutine == null) {
                    OutputCoroutine = OutputBag(customer, customersGarments);
                    StartCoroutine(OutputCoroutine);
                }
                else {
                    //Add output to queue if it isn't already there
                    bool alreadyInQueue = false;
                    for(int i = 0; i < OutputQueue.Count; i++) {
                        if(OutputQueue[i].customer.ticketNumber == customer.ticketNumber) {
                            alreadyInQueue = true;
                            break;
                        }
                    }
                    if (!alreadyInQueue) {
                        OutputData output = new OutputData();
                        output.customer = customer;
                        output.customersGarments = customersGarments;
                        OutputQueue.Add(output);
                    }
                }
            }
        }
    }

    private IEnumerator OutputBag(Customer customer, List<Garment> customersGarments) {

        yield return new WaitForLaundromatSeconds(0.2f);

        AudioManager.instance.PlaySound(Sounds.ProcessingEmb,0.4f);

        yield return new WaitForLaundromatSeconds(1.0f);
        
        //Remove all of this customer's garments from the bagger
        int launderedGarments = 0;
        int perfectGarments = 0;
        int ruinedGarments = 0;

        foreach (Garment garment in customersGarments) {
            if (garment.Ruined)
                ruinedGarments++;
            else if (garment.Dry && garment.Clean && garment.Folded) {
                launderedGarments++;
                if (garment.Pressed)
                    perfectGarments++;
            }
            contents.Remove(garment);
        }

        //Spawn a LaundromatBag
        LaundromatBag bag = Instantiate(laundromatBagPrefab, bagSpawnPoint.position, bagSpawnPoint.rotation).GetComponent<LaundromatBag>();
        bag.contents = customersGarments;
        if(customer == null) {
            bag.customerID = customersGarments[0].customerID;
        }
        else
            bag.customerID = customer.ticketNumber;
        bag.totalGarments = customersGarments.Count;
        bag.launderedGarments = launderedGarments;
        bag.perfectGarments = perfectGarments;
        bag.ruinedGarments = ruinedGarments;

        if (BagOutput != null) BagOutput();

        //Check if there is another bag to output in the queue
        if(OutputQueue.Count > 0) {
            OutputData output = OutputQueue[0];
            OutputQueue.RemoveAt(0);
            OutputCoroutine = OutputBag(output.customer, output.customersGarments);
            StartCoroutine(OutputCoroutine);
        }
        else {
            OutputCoroutine = null;
        }
    }
}
