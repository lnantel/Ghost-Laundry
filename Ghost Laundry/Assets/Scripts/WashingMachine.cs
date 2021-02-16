using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WashingMachine : MonoBehaviour
{
    public static Action DoorOpens;
    public static Action DoorCloses;
    public static Action WashCycleStarts;
    public static Action WashCycleDone;

    public float WashCycleTime;
    public int Capacity;

    private List<Garment> contents;

    [HideInInspector]
    public WashSetting washSetting;

    [HideInInspector]
    public WashingMachineState state;

    private void Start() {
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
        if(CurrentLoad() + garment.size <= Capacity) {
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
        if(state == WashingMachineState.DoorClosed || state == WashingMachineState.Done) {
            state = WashingMachineState.DoorOpen;
            DoorOpens();
        }
        else if(state == WashingMachineState.DoorOpen) {
            state = WashingMachineState.DoorClosed;
            DoorCloses();
        }
    }

    public void StartWashCycle() {
        if(state == WashingMachineState.DoorClosed) {
            state = WashingMachineState.Running;
            StartCoroutine(WashCycle());
        }
    }

    private IEnumerator WashCycle() {
        foreach (Garment garment in contents) {
            garment.dry = false;
        }

        yield return new WaitForSeconds(WashCycleTime);

        foreach (Garment garment in contents) {
            //TODO: Washing logic
            garment.clean = true;
        }
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