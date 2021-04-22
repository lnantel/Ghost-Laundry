using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHeap : BasketHeap
{
    private WashingMachine washingMachine;
    private Dryer dryer;

    protected override void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Find a Dryer or WashingMachine to track capacity
        //Yes I know Dryer and WashingMachine should inherit from a parent Machine class since 
        //they're so similar and it would simplify the code a lot, but too late, and too bad!
        washingMachine = GetComponentInParent<WashingMachine>();
        dryer = GetComponentInParent<Dryer>();
    }

    protected override void OnEnable() {
        //Intentionally left blank
    }

    protected override void OnDisable() {
        //Intentionally left blank
    }

    protected override void BasketUpdate() {
        //Intentionally left blank
    }

    protected override void Update() {
        if (dryer != null && dryer.state == DryerState.DoorOpen && dryer.CurrentLoad() > 0 && dryer.Capacity > 1) {
            spriteRenderer.enabled = true;
            float ratio = (dryer.CurrentLoad() - 1.0f) / (dryer.Capacity - 1.0f);
            transform.localPosition = new Vector3(transform.localPosition.x, minY + (maxY - minY) * ratio, transform.localPosition.z);
        }
        else if (washingMachine != null && washingMachine.state == WashingMachineState.DoorOpen && washingMachine.CurrentLoad() > 0 && washingMachine.Capacity > 1) {
            spriteRenderer.enabled = true;
            float ratio = (washingMachine.CurrentLoad() - 1.0f) / (washingMachine.Capacity - 1.0f);
            transform.localPosition = new Vector3(transform.localPosition.x, minY + (maxY - minY) * ratio, transform.localPosition.z);
        }
        else {
            spriteRenderer.enabled = false;
        }
    }
}
