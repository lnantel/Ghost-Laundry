using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachineWorkStation : WorkStation
{
    protected override void Start() {
        HasGravity = true;
        base.Start();
    }
}
