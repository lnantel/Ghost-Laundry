using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SewingMachine : WorkStation
{
    public static Action GarmentUpdated;
    public static Action SewingProgressUpdated;

    public float progress;

    public Garment garmentOnMachine;

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("SewingMachineArea");
        base.Start();
    }

    public void ResetProgress() {
        progress = 0.0f;
        if (SewingProgressUpdated != null) SewingProgressUpdated();
    }

    //Sew is called by SewingMachinePedal every frame it is held down
    //Returns true if the machine sews successfully (progress is made)
    public bool Sew() {
        if(garmentOnMachine != null && progress < 1.0f) {
            progress += TimeManager.instance.deltaTime;
            if (SewingProgressUpdated != null) SewingProgressUpdated();
            if (progress >= 1.0f) {
                garmentOnMachine.Torn = false;
                if (GarmentUpdated != null) GarmentUpdated();
            }
            return true;
        }
        return false;
    }

    //Called when a garment is placed on the machine.
    public bool PlaceGarment(Garment garment) {
        if(garmentOnMachine == null) {
            garmentOnMachine = garment;
            ResetProgress();
            if (GarmentUpdated != null) GarmentUpdated();
            return true;
        }
        return false;
    }

    //Called when a garment is removed from the machine
    public Garment RemoveGarmentFromMachine() {
        if(garmentOnMachine != null) {
            Garment garment = garmentOnMachine;
            garmentOnMachine = null;
            ResetProgress();
            if (GarmentUpdated != null) GarmentUpdated();
            return garment;
        }
        return null;
    }
}
