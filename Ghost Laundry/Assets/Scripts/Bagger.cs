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
    public List<Garment> contents;

    private IEnumerator OutputCoroutine;

    public TutorialManager tutorialManager;

    private bool CustomerUIEnabled;

    private struct OutputData {
        public Customer customer;
        public List<Garment> customersGarments;
    }

    private List<OutputData> OutputQueue;

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("BaggerArea"); 
        base.Start();
        contents = new List<Garment>();
        laundromatBagPrefab = (GameObject)Resources.Load("LaundromatBag");
        OutputQueue = new List<OutputData>();
        CustomerUIEnabled = tutorialManager == null;
    }

    protected override void Interaction() {
        if (RequestCarriedBasket != null) RequestCarriedBasket();

        if (CustomerUIEnabled) {
            PlayerController.instance.enabled = false;
            laundryTaskArea.SetActive(true);
            LaundryTaskController.instance.gameObject.SetActive(true);
            LaundryTaskController.instance.activeWorkStation = this;
            TaskView.instance.PopUp(transform.position);
            LaundryTaskController.exitedTask += OnTaskExit;
            LaundryGarment.Released += OnLaundryGarmentReleased;
        }
    }

    public override bool InputBasket(Basket basket, int i) {
        AudioManager.instance.PlaySound(SoundName.DropGarmentEmb);
        foreach (Garment garment in basket.contents) {
            contents.Add(garment);
            if(garment is GarmentSock garmentSock && garment.Folded) {
                contents.Add(garmentSock.pairedSock);
            }
        }
        if (BasketInput != null) BasketInput();
        if (tutorialManager != null) TutorialCheckContentsForOutput();
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
        for(int i = 0; i < customers.Count; i++) {
            bool baggerContainsAll = true;
            if(customers[i].garments != null && customers[i].garments.Count > 0) {
                for (int j = 0; j < customers[i].garments.Count; j++) {
                    if (!contents.Contains(customers[i].garments[j])) baggerContainsAll = false;
                }
                if (baggerContainsAll) {
                    if (OutputCoroutine == null) {
                        OutputCoroutine = OutputBag(customers[i], customers[i].garments);
                        StartCoroutine(OutputCoroutine);
                    }
                    else {
                        //Add output to queue if it isn't already there
                        bool alreadyInQueue = false;
                        for (int k = 0; k < OutputQueue.Count; k++) {
                            if (OutputQueue[k].customer.ticketNumber == customers[i].ticketNumber) {
                                alreadyInQueue = true;
                                break;
                            }
                        }
                        if (!alreadyInQueue) {
                            OutputData output = new OutputData();
                            output.customer = customers[i];
                            output.customersGarments = customers[i].garments;
                            OutputQueue.Add(output);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator OutputBag(Customer customer, List<Garment> customersGarments) {

        yield return new WaitForLaundromatSeconds(0.2f);

        AudioManager.instance.PlaySound(SoundName.ProcessingEmb);

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
                if (garment.Pressed || garment.fabric.pressingRestrictions == PressingRestrictions.NoIroning)
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
