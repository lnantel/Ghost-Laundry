using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OllieFemur : Garment
{
    public OllieFemur(bool clean = false, float humidity = 0.0f, bool pressed = false, bool folded = false, bool ruined = false) : base(new Fabric(FabricType.Bone), GarmentColor.White, clean, humidity, pressed, folded, ruined) {
        size = 1;
        foldingSteps = 0;
        laundryGarmentPrefab = (GameObject)Resources.Load("OllieFemur");
    }

    public override void Fold() {
        //Do nothing
    }
}
