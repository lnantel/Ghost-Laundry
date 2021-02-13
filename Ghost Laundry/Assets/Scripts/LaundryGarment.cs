using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryGarment : LaundryObject
{
    public static Action<LaundryGarment> Released;

    public Garment garment;

    private Rigidbody2D rb;

    private void Start() {
        if(garment == null) {
            garment = new Garment(new Fabric("Silk"), Color.white, false, true);
        }

        rb = GetComponent<Rigidbody2D>();
    }

    public LaundryGarment(Garment garment) {
        this.garment = garment;
    }

    public override void OnInteract() {
        garment.Fold();
        Debug.Log("Garment fold step: " + garment.currentFoldingStep);
    }

    public override void OnInspect() {
        Debug.Log("Displaying tag: ");
        Debug.Log("Fabric: " + garment.fabric.name);
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
