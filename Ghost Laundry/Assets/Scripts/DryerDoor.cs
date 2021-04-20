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
    }

    public override void OnGrab() {
        if (dryer.state == DryerState.DoorOpen) {
            Garment garment = dryer.RemoveTopGarment();
            if (garment != null && GarmentGrabbed != null)
                GarmentGrabbed(garment);
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
                Destroy(laundryGarment.gameObject);
            }
            else {
                Debug.Log("Dryer is full");
            }
        }
    }

    private void GrabEmpty() {
        Debug.Log("Dryer is empty");
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
