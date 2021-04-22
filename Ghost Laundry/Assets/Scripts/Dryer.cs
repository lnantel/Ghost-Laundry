using System.Collections;
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

    private bool autoCompleteFlag;

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("DryerArea");

        HasGravity = true;
        base.Start();

        contents = new List<Garment>();
        animator = GetComponentInChildren<Animator>();
        state = DryerState.DoorClosed;
        dryerSetting = DryerSetting.Low;
        lintTrapClean = true;
    }

    private IEnumerator DryerRunningCoroutine;
    private IEnumerator DryerDoneCoroutine;
    private void Update() {
        animator.SetInteger("DryerState", (int)state);

       if(state == DryerState.Done && DryerDoneCoroutine == null){           
        DryerDoneCoroutine = DryerDoneCoroutineSound();
        StartCoroutine(DryerDoneCoroutine);   
       }
    
    }

    public void SetAutoCompleteFlag() {
        autoCompleteFlag = true;
    }

    IEnumerator DryerDoneCoroutineSound(){
         AudioManager.instance.PlaySound(SoundName.EndDryerBeep);
         yield return new WaitForLaundromatSeconds(20); 
         DryerDoneCoroutine = null;
    }

    public float CurrentLoad() {
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
            if (dryerSetting == DryerSetting.Low) {
                dryerSetting = DryerSetting.High;
                animator.SetFloat("Shake", 1.0f);
            }
            else if (dryerSetting == DryerSetting.High) {
                dryerSetting = DryerSetting.Low;
                animator.SetFloat("Shake", 0.0f);
            }
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

        AudioManager.instance.PlaySoundLoop(SoundName.RunningDryer, cycleTime);

        List<Garment> garmentsToBeAdded = new List<Garment>();
        foreach (Garment garment in contents) {
            //Unfold garments if they are folded
            if (garment is GarmentSock && garment.Folded) {
                GarmentSock other = ((GarmentSock)garment).SeparatePair();
                other.currentFoldingStep = 0;
                garmentsToBeAdded.Add(other);
            }
            garment.currentFoldingStep = 0;
        }

        //Put separated socks in machine
        foreach (Garment garment in garmentsToBeAdded)
            contents.Add(garment);

        if (autoCompleteFlag) {
            yield return new WaitForLaundromatSeconds(2.0f);
            autoCompleteFlag = false;
        }
        else {
            yield return new WaitForLaundromatSeconds(cycleTime);
        }

        foreach (Garment garment in contents) {
            if ((garment.fabric.dryingRestrictions == DryingRestrictions.LowOnly && dryerSetting == DryerSetting.High) ||
                (garment.fabric.dryingRestrictions == DryingRestrictions.HighOnly && dryerSetting == DryerSetting.Low) ||
                garment.fabric.dryingRestrictions == DryingRestrictions.HangDryOnly)
                garment.Torn = true;

            if (lintTrapClean) {
                garment.Dry = true;
            }
        }

        if(contents.Count > 0)
            lintTrapClean = false;
        state = DryerState.Done;
    }

    public void CleanLintTrap() {
        if (!lintTrapClean) lintTrapClean = true;
    }

    protected override List<Garment> GetCustomContainerGarments() {
        return contents;
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