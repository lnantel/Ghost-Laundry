using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WashingMachine : WorkStation
{
    public Animator animator;

    public static Action DoorOpens;
    public static Action DoorCloses;
    public static Action WashCycleStarts;
    public static Action WashCycleDone;

    public float WashCycleTime;
    public int Capacity;

    [HideInInspector]
    public bool Detergent;

    private List<Garment> contents;

    [HideInInspector]
    public WashSetting washSetting;

    [HideInInspector]
    public WashingMachineState state;

    protected override void Start() {
        HasGravity = true;
        base.Start();

        contents = new List<Garment>();
        state = WashingMachineState.DoorClosed;
        washSetting = WashSetting.Hot;
    }

    private void Update() {
        animator.SetInteger("WashingMachineState", (int)state);
    }

    private float CurrentLoad() {
        float value = 0.0f;
        foreach (Garment garment in contents)
            value += garment.size;
        return value;
    }

    public bool AddGarment(Garment garment) {
        if (CurrentLoad() + garment.size <= Capacity) {
            contents.Add(garment);
            return true;
        }
        return false;
    }

    public Garment RemoveTopGarment() {
        if (contents.Count > 0) {
            Garment garment = contents[contents.Count - 1];
            contents.RemoveAt(contents.Count - 1);
            return garment;
        }
        return null;
    }

    public void ToggleWashSetting() {
        if(state != WashingMachineState.Running) {
            if (washSetting == WashSetting.Cold) washSetting = WashSetting.Hot;
            else if(washSetting == WashSetting.Hot) washSetting = WashSetting.Cold;
        }
    }

    public void ToggleDoor() {
        if (state == WashingMachineState.DoorClosed || state == WashingMachineState.Done) {
            state = WashingMachineState.DoorOpen;
            DoorOpens();
        }
        else if (state == WashingMachineState.DoorOpen) {
            state = WashingMachineState.DoorClosed;
            DoorCloses();
        }
    }

    public void StartWashCycle() {
        if (state == WashingMachineState.DoorClosed) {
            state = WashingMachineState.Running;
            StartCoroutine(WashCycle());
        }
    }

    private IEnumerator WashCycle() {
        bool containsColoredGarments = false;

        List<Garment> garmentsToBeAdded = new List<Garment>();
        foreach (Garment garment in contents) {
            //Unfold garments if they are folded
            if (garment is GarmentSock && garment.Folded) {
                GarmentSock other = ((GarmentSock)garment).SeparatePair();
                other.currentFoldingStep = 0;
                other.Dry = false;
                garmentsToBeAdded.Add(other);
            }
            garment.currentFoldingStep = 0;
            garment.Dry = false;
            if (garment.Colored()) containsColoredGarments = true;
        }

        //Put separated socks in machine
        foreach (Garment garment in garmentsToBeAdded)
            contents.Add(garment);

        yield return new WaitForLaundromatSeconds(WashCycleTime);

        foreach (Garment garment in contents) {
            if(!garment.Colored() && washSetting == WashSetting.Hot && containsColoredGarments) {
                garment.color = GarmentColor.Pink;
                garment.Ruined = true;
            }

            if ((garment.fabric.washingRestrictions == WashingRestrictions.ColdOnly && washSetting == WashSetting.Hot) ||
                (garment.fabric.washingRestrictions == WashingRestrictions.HotOnly && washSetting == WashSetting.Cold))
                garment.Ruined = true;

            if (Detergent) {
                garment.Clean = true;
                if (garment.fabric.washingRestrictions == WashingRestrictions.NoDetergent)
                    garment.Ruined = true;
            }
            else if(!Detergent){
                if (garment.fabric.washingRestrictions == WashingRestrictions.NoDetergent) {
                    garment.Clean = true;
                }
            }
        }
        Detergent = false;
        state = WashingMachineState.Done;
    }
}

public enum WashingMachineState {
    DoorClosed,
    DoorOpen,
    Running,
    Done
}

public enum WashSetting {
    Hot,
    Cold
}