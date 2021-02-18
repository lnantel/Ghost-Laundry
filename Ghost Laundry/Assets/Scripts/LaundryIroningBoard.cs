using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryIroningBoard : LaundryObject
{
    public static Action<Garment> GarmentGrabbed;

    public Collider2D boardTriggerCollider;
    public SpriteRenderer garmentSpriteRenderer;

    public Garment garmentOnBoard;
    private float pressingProgress;

    public float burnTime;
    public float gracePeriod;
    private float graceTimer;
    private float burnTimer;

    public float minIronSpeed;
    private float lastIronPos;

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += LaundryGarmentReleased;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= LaundryGarmentReleased;
    }

    //If the garment is being pressed, returns true; if it is being burned, returns false
    public SteamState Press(float ironPosition) {
        SteamState steam = SteamState.Off;

        float ironSpeed = Mathf.Abs((ironPosition - lastIronPos) / Time.fixedDeltaTime);
        Debug.Log("Iron speed: " + ironSpeed);

        if(ironSpeed > minIronSpeed && pressingProgress < 1.0f && !garmentOnBoard.pressed && !garmentOnBoard.ruined) {
            graceTimer = 0;
            //Press
            steam = SteamState.Steam;
            pressingProgress += Time.fixedDeltaTime / garmentOnBoard.fabric.ironingTime;
            Debug.Log("Ironing progress: " + pressingProgress);
        }
        else if (graceTimer < gracePeriod) {
            //Grace period
            steam = SteamState.Off;
            graceTimer += Time.fixedDeltaTime;
            Debug.Log("Grace");
        }
        else {
            //Burn
            steam = SteamState.Burn;
            burnTimer += Time.fixedDeltaTime;
            Debug.Log("Burning!");
        }

        //If the garment was steamed long enough, it becomes pressed
        if (pressingProgress >= 1.0f && !garmentOnBoard.pressed) {
            garmentOnBoard.pressed = true;
            Debug.Log("Ironing done!");
        }

        //If the garment was burned too much, it becomes ruined
        if (burnTimer > burnTime && !garmentOnBoard.ruined) {
            garmentOnBoard.ruined = true;
            Debug.Log("Garment ruined!");
        }

        lastIronPos = ironPosition;
        return steam;
    }

    private void LaundryGarmentReleased(LaundryGarment laundryGarment) {
        if (garmentOnBoard == null && boardTriggerCollider.bounds.Contains(laundryGarment.transform.position)) {
            garmentOnBoard = laundryGarment.garment;
            Destroy(laundryGarment.gameObject);
            garmentSpriteRenderer.enabled = true;
            pressingProgress = 0.0f;
        }
    }

    public override void OnGrab() {
        if (garmentOnBoard != null) {
            if(GarmentGrabbed != null) GarmentGrabbed(garmentOnBoard);
            garmentOnBoard = null;
            garmentSpriteRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Iron iron = collision.GetComponent<Iron>();
        if(iron != null) {
            iron.PlaceOnIroningBoard();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Iron iron = collision.GetComponent<Iron>();
        if (iron != null) {
            iron.TakeOffIroningBoard();
        }
    }
}
