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

    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    public override void OnGrab() {
        if (washingMachine.state == WashingMachineState.DoorOpen) {
            Garment garment = washingMachine.RemoveTopGarment();
            if (garment != null && GarmentGrabbed != null) {
                GarmentGrabbed(garment);
                animator.SetTrigger("BasketOutput");
            }
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
                animator.SetTrigger("BasketInput");
            }
            else {
                animator.SetTrigger("BasketFull");
            }
        }
    }

    private void GrabEmpty() {
        animator.SetTrigger("BasketEmpty");
    }

    private void OpenDoor() {
        spriteRenderer.sprite = openDoorSprite;
        AudioManager.instance.PlaySound(SoundName.OpenWMDoor);
    }

    private void CloseDoor() {
        spriteRenderer.sprite = closedDoorSprite;
        AudioManager.instance.PlaySound(SoundName.CloseWMDoor);
    }

    public void StartWashCycle() {
        washingMachine.StartWashCycle();
        AudioManager.instance.PlaySound(SoundName.StartButtonWM);
    }
}
