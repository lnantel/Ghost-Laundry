using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentTop : Garment
{
    public GarmentTop(Fabric fabric, Color color, bool clean = false, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, clean, humidity, pressed, folded, shrunk, burned, dyed, torn, melted) {
        size = 1;
        foldingSteps = 3;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentTop");
    }
}
