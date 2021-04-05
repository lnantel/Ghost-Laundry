using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WashingMachineDoor : LaundryObject
{
    public static Action<Garment> GarmentGrabbed;

    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;

    private WashingMachine washingMachine;
    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
        WashingMachine.DoorCloses += CloseDoor;
        WashingMachine.DoorOpens += OpenDoor;
        
        
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
        WashingMachine.DoorCloses -= CloseDoor;
        WashingMachine.DoorOpens -= OpenDoor;

        
    }

    private void Start() {
        washingMachine = GetComponentInParent<WashingMachine>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnGrab() {
        if (washingMachine.state == WashingMachineState.DoorOpen) {
            Garment garment = washingMachine.RemoveTopGarment();
            if (garment != null && GarmentGrabbed != null)
                GarmentGrabbed(garment);
            else
                GrabEmpty();
        }
    }

    public override void OnInteract() {
        washingMachine.ToggleDoor();
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        //If released within open door bounds, add garment to machine
        if (washingMachine.state == WashingMachineState.DoorOpen && GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position)) {
            if (washingMachine.AddGarment(laundryGarment.garment)) {
                AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                Destroy(laundryGarment.gameObject);
            }
            else {
                Debug.Log("Washing machine is full");
            }
        }
    }

    private void GrabEmpty() {
        Debug.Log("Washing machine is empty");
    }

    private void OpenDoor() {
        spriteRenderer.sprite = openDoorSprite;
        AudioManager.instance.PlaySound(Sounds.OpenWMDoor);
    }

    private void CloseDoor() {
        spriteRenderer.sprite = closedDoorSprite;
        AudioManager.instance.PlaySound(Sounds.CloseWMDoor);
    }

    public void StartWashCycle() {
        washingMachine.StartWashCycle();
        AudioManager.instance.PlaySound(Sounds.StartButtonWM);
    }
}
