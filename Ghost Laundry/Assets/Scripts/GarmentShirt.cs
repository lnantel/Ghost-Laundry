using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentShirt : Garment
{
    public GarmentShirt(Fabric fabric, Color color, bool clean = false, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, clean, humidity, pressed, folded, shrunk, burned, dyed, torn, melted) {
        size = 2;
        foldingSteps = 8;
        clotheslinePegs = 2;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentShirt");
    }

    public override void Fold() {
        if(currentFoldingStep >= 0 && currentFoldingStep <= 4) {
            AudioManager.instance.PlaySound(Sounds.ShirtButton1 + currentFoldingStep);
        }
        else {
            AudioManager.instance.PlaySound(Sounds.Fold1 + (currentFoldingStep - 4));
        }
        currentFoldingStep = (currentFoldingStep + 1) % (foldingSteps + 1);
    }
}
