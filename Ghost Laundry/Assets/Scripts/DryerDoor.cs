using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DryerDoor : LaundryObject
{
    public static Action<Garment> GarmentGrabbed;

    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;

    public BoxCollider2D openDoorCollider;
    public BoxCollider2D closedDoorCollider;

    private Dryer dryer;
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
        Dryer.DoorCloses += CloseDoor;
        Dryer.DoorOpens += OpenDoor;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
        Dryer.DoorCloses -= CloseDoor;
        Dryer.DoorOpens -= OpenDoor;
    }

    private void Start() {
        dryer = GetComponentInParent<Dryer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public override void OnGrab() {
        if (dryer.state == DryerState.DoorOpen && closedDoorCollider.bounds.Contains(LaundryTaskController.instance.cursor.position)) {
            Garment garment = dryer.RemoveTopGarment();
            if (garment != null && GarmentGrabbed != null) {
                GarmentGrabbed(garment);
                animator.SetTrigger("BasketOutput");
            }
            else
                GrabEmpty();
        }
    }

    public override void OnInteract() {
        dryer.ToggleDoor();
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        //If released within open door bounds, add garment to machine
        if (dryer.state == DryerState.DoorOpen && GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position)) {
            if (dryer.AddGarment(laundryGarment.garment)) {
                AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                laundryGarment.ReturnToPool();
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
        openDoorCollider.enabled = true;
        closedDoorCollider.enabled = true;
        AudioManager.instance.PlaySound(SoundName.OpenDryerDoor);
    }

    private void CloseDoor() {
        spriteRenderer.sprite = closedDoorSprite;
        openDoorCollider.enabled = false;
        closedDoorCollider.enabled = true;
        AudioManager.instance.PlaySound(SoundName.CloseDryerDoor);

    }

    public void StartDryerCycle() {
        dryer.StartDryerCycle();
        AudioManager.instance.PlaySound(SoundName.StartButtonDryer);

    }

    public void ToggleSetting() {
        dryer.ToggleDryerSetting();
    }
}
