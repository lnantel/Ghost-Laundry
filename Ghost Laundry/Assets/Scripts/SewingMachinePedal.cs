using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewingMachinePedal : LaundryObject
{
    private SewingMachine sewingMachine;

    private void Start() {
        sewingMachine = GetComponentInParent<SewingMachine>();
    }

    public override void Drag(Vector2 cursorPosition) {
        sewingMachine.Sew();
    }
}
