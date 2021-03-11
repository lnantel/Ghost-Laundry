using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentPants : Garment {
    public GarmentPants(Fabric fabric, Color color, bool clean = false, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, clean, humidity, pressed, folded, shrunk, burned, dyed, torn, melted) {
        size = 2;
        foldingSteps = 3;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentPants");
    }
}
