using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashBoard : LaundryObject
{
    private WashTub washTub;
    private LaundryGarment heldLaundryGarment;
    private Vector2 lastPosition;

    private void Start() {
        washTub = GetComponentInParent<WashTub>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        LaundryGarment laundryGarment = collision.GetComponentInParent<LaundryGarment>();
        if (laundryGarment != null && IsHeld(laundryGarment) && (heldLaundryGarment == null || laundryGarment.GetInstanceID() != heldLaundryGarment.GetInstanceID())) {
            heldLaundryGarment = laundryGarment;
            lastPosition = laundryGarment.transform.position;
        }
        else if (laundryGarment != null && !IsHeld(laundryGarment) && (heldLaundryGarment != null && laundryGarment.GetInstanceID() == heldLaundryGarment.GetInstanceID())) {
            heldLaundryGarment = null;
        }
        else if (laundryGarment != null && IsHeld(laundryGarment) && (heldLaundryGarment != null && laundryGarment.GetInstanceID() == heldLaundryGarment.GetInstanceID())) {
            washTub.Scrub(laundryGarment.garment, Vector2.Distance(laundryGarment.transform.position, lastPosition));
            laundryGarment.UpdateAppearance();
            lastPosition = laundryGarment.transform.position;
        }
    }

    //Returns true is the laundryGarment is currently being held by the cursor.
    private bool IsHeld(LaundryGarment laundryGarment) {
        if(LaundryTaskController.instance.grabbedObject != null)
            return laundryGarment.GetInstanceID() == LaundryTaskController.instance.grabbedObject.GetInstanceID();
        return false;
    }
}
