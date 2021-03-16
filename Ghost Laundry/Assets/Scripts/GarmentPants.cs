using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentPants : Garment {
    public GarmentPants(Fabric fabric, Color color, float cleanliness = 0.0f, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, cleanliness, humidity, pressed, folded, shrunk, burned, dyed, torn, melted) {
        size = 2;
        foldingSteps = 3;
        clotheslinePegs = 2;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentPants");
    }
}
