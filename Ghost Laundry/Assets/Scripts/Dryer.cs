﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dryer : WorkStation
{
    public static Action DoorOpens;
    public static Action DoorCloses;
    public static Action DryerCycleStarts;
    public static Action DryerCycleDone;

    public float DryerCycleTime_High;
    public float DryerCycleTime_Low;
    public int Capacity;

    [HideInInspector]
    public bool lintTrapClean;

    [HideInInspector]
    public DryerSetting dryerSetting;

    [HideInInspector]
    public DryerState state;

    private List<Garment> contents;
    private Animator animator;

    protected override void Start() {
        HasGravity = true;
        base.Start();

        contents = new List<Garment>();
        animator = GetComponentInChildren<Animator>();
        state = DryerState.DoorClosed;
        dryerSetting = DryerSetting.High;
        lintTrapClean = true;
    }

    private void Update() {
        animator.SetInteger("DryerState", (int)state);
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

    public void ToggleDryerSetting() {
        if(state != DryerState.Running) {
            if (dryerSetting == DryerSetting.Low) dryerSetting = DryerSetting.High;
            else if (dryerSetting == DryerSetting.High) dryerSetting = DryerSetting.Low;
        }
    }

    public void ToggleDoor() {
        if (state == DryerState.DoorClosed || state == DryerState.Done) {
            state = DryerState.DoorOpen;
            DoorOpens();
        }
        else if (state == DryerState.DoorOpen) {
            state = DryerState.DoorClosed;
            DoorCloses();
        }
    }

    public void StartDryerCycle() {
        if (state == DryerState.DoorClosed) {
            state = DryerState.Running;
            StartCoroutine(DryerCycle());
        }
    }

    private IEnumerator DryerCycle() {
        float cycleTime = 0.0f;
        if (dryerSetting == DryerSetting.High) cycleTime = 10.0f;
        else if (dryerSetting == DryerSetting.Low) cycleTime = 20.0f;

        yield return new WaitForSeconds(cycleTime);

        foreach (Garment garment in contents) {
            garment.dry = lintTrapClean; //garments are dried if the lint trap is clean
        }

        if(contents.Count > 0)
            lintTrapClean = false;
        state = DryerState.Done;
    }

    public void CleanLintTrap() {
        if (!lintTrapClean) lintTrapClean = true;
        else
            Debug.Log("Lint trap already clean");
    }
}

public enum DryerState {
    DoorClosed,
    DoorOpen,
    Running,
    Done
}

public enum DryerSetting {
    High,
    Low
}