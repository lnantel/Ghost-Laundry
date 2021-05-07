using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryIroningBoard : LaundryObject
{
    public static Action<Garment> GarmentGrabbed;

    public Collider2D boardTriggerCollider;

    public IroningBoardGarmentRenderer garmentRenderer;

    private float pressingProgress;

    public float burnTime;
    public float gracePeriod;
    private float graceTimer;
    private float burnTimer;

    private IEnumerator SoundCoroutine; 

    public float minIronSpeed;
    private float lastIronPos;

    private IroningBoard ironingBoard;

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += LaundryGarmentReleased;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= LaundryGarmentReleased;
    }

    private void Start() {
        ironingBoard = GetComponentInParent<IroningBoard>();
    }

    //If the garment is being pressed, returns true; if it is being burned, returns false
    public SteamState Press(float ironPosition) {
        SteamState steam = SteamState.Off;
        if (TimeManager.instance.timeScale == 0.0f) return SteamState.Off;

        //ironPosition has to be offset by an arbitrary value > 0.5 because the board overlaps with the 0 position on the X axis.
        //This ensures that we can distinguish between an uninitialized lastIronPos and a lastIronPos of value 0.
        ironPosition += 5.0f;

        if(lastIronPos == 0.0f) {
            lastIronPos = ironPosition;
            return SteamState.Off;
        }

        float ironSpeed = Mathf.Abs(ironPosition - lastIronPos) / Time.fixedDeltaTime;
        //Debug.Log("Current speed: " + ironSpeed + " Minimum: " + minIronSpeed);
        if(ironingBoard.garmentOnBoard.fabric.pressingRestrictions != PressingRestrictions.NoIroning && ironSpeed > minIronSpeed && pressingProgress < 1.0f && !ironingBoard.garmentOnBoard.Pressed && !ironingBoard.garmentOnBoard.Ruined && ironingBoard.garmentOnBoard.Clean && ironingBoard.garmentOnBoard.Dry) {
            graceTimer = 0;
            //Press
            steam = SteamState.Steam;
            pressingProgress += (ironSpeed * Time.fixedDeltaTime) / ironingBoard.garmentOnBoard.fabric.ironingDistance;
        }
        else if (graceTimer < gracePeriod && !(ironingBoard.garmentOnBoard.Burned || ironingBoard.garmentOnBoard.Melted)) {
            //Grace period
            steam = SteamState.Off;
            graceTimer += Time.fixedDeltaTime;
        }
        else if(!(ironingBoard.garmentOnBoard.Burned || ironingBoard.garmentOnBoard.Melted) && ironingBoard.garmentOnBoard.Dry) {
            //Burn
            steam = SteamState.Burn;
            burnTimer += Time.fixedDeltaTime;
        }

        //If the garment was steamed long enough, it becomes pressed
        if (pressingProgress >= 1.0f && !ironingBoard.garmentOnBoard.Pressed) {
            AudioManager.instance.PlaySound(SoundName.ShiningGarment);
            ironingBoard.garmentOnBoard.Pressed = true;
            garmentRenderer.UpdateAppearance();
        }

        //If the garment was burned too much, it becomes ruined
        if (burnTimer > burnTime && !(ironingBoard.garmentOnBoard.Burned || ironingBoard.garmentOnBoard.Melted)) {
            if (ironingBoard.garmentOnBoard.fabric.name.Equals("Synthetic"))
                ironingBoard.garmentOnBoard.Melted = true;
            else
                ironingBoard.garmentOnBoard.Burned = true;
            garmentRenderer.UpdateAppearance();
        }

        lastIronPos = ironPosition;

        return steam;
    }

    private void LaundryGarmentReleased(LaundryGarment laundryGarment) {
        if (ironingBoard.garmentOnBoard == null && boardTriggerCollider.bounds.Contains(laundryGarment.transform.position)) {
            PlaceGarmentOnBoard(laundryGarment);
        }
    }

    private void PlaceGarmentOnBoard(LaundryGarment laundryGarment) {
        //Unfold garment if folded
        //If pair of socks, separate them, then spawn the extra sock as a LaundryGarment
        if (laundryGarment.garment is GarmentSock && laundryGarment.garment.Folded) {
            GarmentSock sock = (GarmentSock)laundryGarment.garment;
            GarmentSock other = sock.SeparatePair();
            other.CreateLaundryGarment(laundryGarment.transform.position, laundryGarment.transform.rotation, laundryGarment.transform.parent);
        }
        laundryGarment.garment.currentFoldingStep = 0;

        ironingBoard.garmentOnBoard = laundryGarment.garment;
        AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
        laundryGarment.ReturnToPool();
        garmentRenderer.UpdateAppearance();
        pressingProgress = 0.0f;
        burnTimer = 0.0f;
        graceTimer = 0.0f;
    }

    private void RemoveGarmentFromBoard() {
        ironingBoard.garmentOnBoard = null;
        garmentRenderer.UpdateAppearance();
    }

    public override void OnGrab() {
        if (ironingBoard.garmentOnBoard != null) {
            if(GarmentGrabbed != null) GarmentGrabbed(ironingBoard.garmentOnBoard);
            RemoveGarmentFromBoard();
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Iron iron = collision.GetComponent<Iron>();
        if(iron != null && !iron.onIroningBoard) {
            iron.PlaceOnIroningBoard();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
            LaundryGarment laundryGarment = collision.GetComponentInParent<LaundryGarment>();
            if (laundryGarment != null && !laundryGarment.IsHeld && laundryGarment.GetComponent<Rigidbody2D>().gravityScale != 0.0f) {
                if (ironingBoard.garmentOnBoard == null) {
                    PlaceGarmentOnBoard(laundryGarment);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Iron iron = collision.GetComponent<Iron>();
        if (iron != null) {
            iron.TakeOffIroningBoard();
        }
    }
}
