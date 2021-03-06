﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentShirt : Garment
{
    public GarmentShirt(Fabric fabric, Color color, float cleanliness = 0.0f, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, cleanliness, humidity, pressed, folded, shrunk, burned, dyed, torn, melted) {
        type = GarmentType.Shirt;
        size = 2;
        foldingSteps = 8;
        clotheslinePegs = 2;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentShirt");
    }

    public override void Fold() {
        if(currentFoldingStep >= 0 && currentFoldingStep <= 5) {
            AudioManager.instance.PlaySound(SoundName.ShirtButton1 + currentFoldingStep);
        }
        else {
            AudioManager.instance.PlaySound(SoundName.Fold1 + (currentFoldingStep - 5));
        }
        currentFoldingStep = (currentFoldingStep + 1) % (foldingSteps + 1);
    }
}
