using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bagger : WorkStation
{
    public Transform bagSpawnPoint;

    private GameObject laundromatBagPrefab;
    private List<Garment> contents;

    private IEnumerator OutputCoroutine;

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
    }

    public override void Interact() {
        if(RequestCarriedBasket != null) RequestCarriedBasket();
    }

    public override bool InputBasket(Basket basket) {
        foreach (Garment garment in basket.contents) {
            contents.Add(garment);
            AudioManager.instance.PlaySound(Sounds.DropGarmentEmb);
        }
        CheckContentsForOutput();
        return true;
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
                    if (garment is GarmentSock && ((GarmentSock)garment).currentFoldingStep == 1) garmentCount++; //paired socks count twice!

                }
            }
            if(garmentCount != 0 && garmentCount == customer.garments.Count) {
                if(OutputCoroutine == null) {
                    OutputCoroutine = OutputBag(customer, customersGarments);
                    StartCoroutine(OutputCoroutine);
                }
                else {
                    OutputData output = new OutputData();
                    output.customer = customer;
                    output.customersGarments = customersGarments;
                    OutputQueue.Add(output);
                }
            }
        }
    }

    private IEnumerator OutputBag(Customer customer, List<Garment> customersGarments) {

        yield return new WaitForLaundromatSeconds(0.2f);

        AudioManager.instance.PlaySound(Sounds.ProcessingEmb);

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
        bag.customerID = customer.ticketNumber;
        bag.totalGarments = customersGarments.Count;
        bag.launderedGarments = launderedGarments;
        bag.perfectGarments = perfectGarments;
        bag.ruinedGarments = ruinedGarments;
        
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
