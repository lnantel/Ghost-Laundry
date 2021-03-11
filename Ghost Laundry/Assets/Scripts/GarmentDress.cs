using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentDress : Garment
{
    public GarmentDress(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool ruined = false) : base(fabric, color, clean, dry, pressed, folded, ruined) {
        size = 2;
        foldingSteps = 2;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentDress");
    }
}
