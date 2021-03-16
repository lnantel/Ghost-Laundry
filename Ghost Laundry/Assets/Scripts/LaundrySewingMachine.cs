using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundrySewingMachine : LaundryObject
{
    public static Action<Garment> GarmentGrabbed;

    private SewingMachine sewingMachine;
    private Collider2D col;

    private void Start() {
        sewingMachine = GetComponentInParent<SewingMachine>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        //Place garment on machine
        if (col.bounds.Contains(laundryGarment.transform.position)) {
            if (sewingMachine.PlaceGarment(laundryGarment.garment)) {
                AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                Destroy(laundryGarment.gameObject);
            }
        }
    }

    public override void OnGrab() {
        //Grab garment from machine
        Garment garment = sewingMachine.RemoveGarmentFromMachine();
        if (garment != null)
            GarmentGrabbed(garment);
    }
}
