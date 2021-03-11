using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentDress : Garment
{
    public GarmentDress(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, clean, dry, pressed, folded, shrunk, burned, dyed, torn, melted) {
        size = 2;
        foldingSteps = 2;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentDress");
    }
}
