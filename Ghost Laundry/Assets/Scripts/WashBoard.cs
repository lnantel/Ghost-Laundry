using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashBoard : LaundryObject
{
    public ParticleSystem bubbles;

    public Color maxDirtyColor;
    public Color minDirtyColor;
    public Color cleanColor;

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
            ParticleSystem.EmissionModule emission = bubbles.emission;
            emission.enabled = washTub.Scrub(laundryGarment.garment, Vector2.Distance(laundryGarment.transform.position, lastPosition));
            

            ParticleSystem.MainModule main = bubbles.main;
            if (!laundryGarment.garment.Clean)
                main.startColor = Color.Lerp(maxDirtyColor, minDirtyColor, laundryGarment.garment.Cleanliness);
            else
                main.startColor = cleanColor;

            bubbles.transform.position = laundryGarment.transform.position;

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
