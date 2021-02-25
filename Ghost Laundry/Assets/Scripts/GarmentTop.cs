using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentTop : Garment
{
    public GarmentTop(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool ruined = false) : base(fabric, color, clean, dry, pressed, folded, ruined) {
        size = 1;
        foldingSteps = 3;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentTop");
    }
}
