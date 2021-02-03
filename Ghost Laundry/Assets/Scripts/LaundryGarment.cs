using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryGarment : LaundryObject
{
    public Garment garment;

    private void Start() {
        if(garment == null) {
            garment = new Garment(new Fabric("Silk"), Color.white, false, true);
        }
    }

    public override void OnInteract() {
        garment.Fold();
        Debug.Log("Garment fold step: " + garment.currentFoldingStep);
    }

    public override void OnInspect() {
        Debug.Log("Displaying tag: ");
        Debug.Log("Fabric: " + garment.fabric.name);
    }
}
