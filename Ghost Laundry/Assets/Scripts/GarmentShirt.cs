using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentShirt : Garment
{
    public GarmentShirt(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool ruined = false) : base(fabric, color, clean, dry, pressed, folded, ruined) {
        size = 2;
        foldingSteps = 8;
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
