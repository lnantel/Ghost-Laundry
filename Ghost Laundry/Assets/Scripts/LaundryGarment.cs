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
    private bool hovering;
    private bool inspected;
    private Vector2 lastPosition;
    
    private void Start() {
        if(garment == null) {
            garment = new Garment(new Fabric("Silk"), Color.white, false, true);
        }

        rb = GetComponent<Rigidbody2D>();
        laundryTag = GetComponentInChildren<LaundryTag>();
        lastPosition = transform.position;
    }

    private void LateUpdate() {
        if (!hovering) {
            laundryTag.Hide();
            inspected = false;
        }
        else hovering = false;
    }

    private void FixedUpdate() {
        lastPosition = transform.position;
    }

    public LaundryGarment(Garment garment) {
        this.garment = garment;
    }

    public override void OnInteract() {
        inspected = false;
        garment.Fold();
        Debug.Log("Garment fold step: " + garment.currentFoldingStep);
    }

    public override void OnInspect() {
        inspected = !inspected;
    }

    public override void OnHover(Vector2 position) {
        if(laundryTag != null && inspected) {
            laundryTag.Show();
            laundryTag.transform.position = position;
            hovering = true;
        }
    }

    public override void OnRelease() {
        rb.velocity = new Vector2(transform.position.x, transform.position.y) - lastPosition;

        if(Released != null)
            Released(this);
    }

    public override void Drag(Vector2 cursorPosition) {
        inspected = false;
        rb.MovePosition(cursorPosition);
        rb.velocity = Vector3.zero; //Stop gravity from accumulating while the object is grabbed
    }
}
