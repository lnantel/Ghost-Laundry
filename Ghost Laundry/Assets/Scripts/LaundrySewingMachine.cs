using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundrySewingMachine : LaundryObject
{
    public static Action<Garment> GarmentGrabbed;

    public SpriteRenderer tableRenderer;
    public Sprite tableOn;
    public Sprite tableOff;

    public SpriteRenderer machineRenderer;
    public Sprite machineDown;
    public Sprite machineUp;

    private SewingMachine sewingMachine;
    private Collider2D col;

    private void Start() {
        sewingMachine = GetComponentInParent<SewingMachine>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
        SewingMachine.SewingProgressUpdated += OnProgress;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
        SewingMachine.SewingProgressUpdated -= OnProgress;
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

    private void OnProgress() {
        if(sewingMachine.progress % 0.1f < 0.05f) {
            if(machineRenderer.sprite == machineDown){
                tableRenderer.sprite = tableOff;
                machineRenderer.sprite = machineUp;
                AudioManager.instance.PlaySound(Sounds.SewingMachine1,0.5f);
            }
            
        }
        else {
            if(machineRenderer.sprite == machineUp){
            tableRenderer.sprite = tableOn;
            machineRenderer.sprite = machineDown;
            AudioManager.instance.PlaySound(Sounds.SewingMachine2,0.5f);
        }
    }
}
}
