using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryGarment : LaundryObject
{
    public static Action<LaundryGarment> Released;

    public Garment garment;

    private Rigidbody2D rb;
    private LaundryTag laundryTag;
    private bool lastInspectHeld;
    
    private void Start() {
        if(garment == null) {
            garment = new Garment(new Fabric("Silk"), Color.white, false, true);
        }

        rb = GetComponent<Rigidbody2D>();

        laundryTag = GetComponentInChildren<LaundryTag>();
    }

    private void LateUpdate() {
        if (!lastInspectHeld) laundryTag.Hide();
        else lastInspectHeld = false;
    }

    public LaundryGarment(Garment garment) {
        this.garment = garment;
    }

    public override void OnInteract() {
        garment.Fold();
        Debug.Log("Garment fold step: " + garment.currentFoldingStep);
    }

    public override void OnInspectHeld(Vector2 position) {
        if(laundryTag != null) {
            laundryTag.Show();
            laundryTag.transform.position = position;
            lastInspectHeld = true;
        }
    }

    public override void OnRelease() {
        //If release on basket, trigger action with basket id
        if(Released != null)
            Released(this);
    }

    public override void Drag(Vector2 cursorPosition) {
        rb.MovePosition(cursorPosition);
        rb.velocity = Vector3.zero;
    }
}
