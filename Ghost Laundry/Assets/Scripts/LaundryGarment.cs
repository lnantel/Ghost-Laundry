using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryGarment : LaundryObject
{
    public Garment garment;

    private void Start() {
        if(garment == null) {
            garment = new Garment(new Fabric(), Color.white, false, true);
        }
    }

    public override void OnInteract() {
        garment.Fold();
        Debug.Log("Garment fold step: " + garment.currentFoldingStep);
    }
}
