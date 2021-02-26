using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bagger : WorkStation
{
    public Transform bagSpawnPoint;

    private GameObject laundromatBagPrefab;
    private List<Garment> contents;

    protected override void Start() {
        base.Start();
        contents = new List<Garment>();
        laundromatBagPrefab = (GameObject)Resources.Load("LaundromatBag");
    }

    public override void Interact() {
        if(RequestCarriedBasket != null) RequestCarriedBasket();
    }

    public override bool InputBasket(Basket basket) {
        foreach (Garment garment in basket.contents) {
            contents.Add(garment);
        }
        OutputBags();
        return true;
    }

    private void OutputBags() {
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
            if(garmentCount == customer.garments.Count) {
                //Remove all of this customer's garments from the bagger
                int launderedGarments = 0;
                int perfectGarments = 0;
                int ruinedGarments = 0;

                foreach (Garment garment in customersGarments) {
                    contents.Remove(garment);
                    if (garment.Ruined)
                        ruinedGarments++;
                    else if (garment.Dry && garment.Clean && garment.Folded) {
                        launderedGarments++;
                        if (garment.Pressed)
                            perfectGarments++;
                    }
                }

                //Spawn a LaundromatBag
                LaundromatBag bag = Instantiate(laundromatBagPrefab, bagSpawnPoint.position, bagSpawnPoint.rotation).GetComponent<LaundromatBag>();
                bag.ticketNumber = customer.ticketNumber;
                bag.totalGarments = customersGarments.Count;
                bag.launderedGarments = launderedGarments;
                bag.perfectGarments = perfectGarments;
                bag.ruinedGarments = ruinedGarments;
            }
        }
    }
}
