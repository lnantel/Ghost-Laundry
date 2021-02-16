using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WashingMachine : WorkStation
{
    public static Action DoorOpens;
    public static Action DoorCloses;
    public static Action WashCycleStarts;
    public static Action WashCycleDone;

    public float WashCycleTime;
    public int Capacity;

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
        if (washSetting == WashSetting.Cold) washSetting = WashSetting.Hot;
        else washSetting = WashSetting.Cold;
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

    public bool AddDetergent() {
        if (!Detergent) {
            Detergent = true;
            return true;
        }
        else {
            Debug.Log("Detergent overflow");
            return false;
        }
    }

    private IEnumerator WashCycle() {
        foreach (Garment garment in contents) {
            garment.dry = false;
        }

        yield return new WaitForSeconds(WashCycleTime);

        foreach (Garment garment in contents) {
            //TODO: Garment-dependant washing logic
            if (Detergent) {
                garment.clean = true;
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