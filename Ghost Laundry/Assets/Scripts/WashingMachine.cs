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

    public bool SettingsLocked;
    public bool DoorLocked;
    public bool StartButtonLocked;
    public bool DetergentSlotLocked;

    private List<Garment> contents;

    private bool autoCompleteFlag;    

    [HideInInspector]
    public WashSetting washSetting;

    [HideInInspector]
    public WashingMachineState state;

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("WashingMachineArea");
        HasGravity = true;
        base.Start();

        contents = new List<Garment>();
        state = WashingMachineState.DoorClosed;
        washSetting = WashSetting.Hot;
    }

    private IEnumerator WMCoroutine;
    private IEnumerator WMDoneCoroutine;
    private void Update() {
        animator.SetInteger("WashingMachineState", (int)state);

       //if(state == WashingMachineState.Running && WMCoroutine == null){           
       // WMCoroutine = WashingMachineRunningCoroutineSound();
       // StartCoroutine(WMCoroutine);   
       //}

       if(state == WashingMachineState.Done && WMDoneCoroutine == null){           
        WMDoneCoroutine = WashingMachineDoneCoroutineSound();
        StartCoroutine(WMDoneCoroutine);   
       }        
    }

     //IEnumerator WashingMachineRunningCoroutineSound(){
     //    AudioManager.instance.PlaySound(SoundName.RunningWM);
     //    yield return new WaitForLaundromatSeconds(1); 
     //    WMCoroutine = null;
     //   }

    IEnumerator WashingMachineDoneCoroutineSound(){
         AudioManager.instance.PlaySound(SoundName.EndWMBeep);;
         yield return new WaitForLaundromatSeconds(20); 
         WMDoneCoroutine = null;
        }

    public void SetAutoCompleteFlag() {
        autoCompleteFlag = true;
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

    public void ToggleWashSetting() {
        if(state != WashingMachineState.Running && !SettingsLocked) {
            if (washSetting == WashSetting.Cold) washSetting = WashSetting.Hot;
            else if(washSetting == WashSetting.Hot) washSetting = WashSetting.Cold;
        }
    }

    public void ToggleDoor() {
        if (!DoorLocked && (state == WashingMachineState.DoorClosed || state == WashingMachineState.Done)) {
            state = WashingMachineState.DoorOpen;
            DoorOpens();
        }
        else if (!DoorLocked && (state == WashingMachineState.DoorOpen)) {
            state = WashingMachineState.DoorClosed;
            DoorCloses();
        }
    }

    public void StartWashCycle() {
        if (!StartButtonLocked && state == WashingMachineState.DoorClosed) {
            state = WashingMachineState.Running;
            StartCoroutine(WashCycle());
        }
    }

    private IEnumerator WashCycle() {
        bool containsColoredGarments = false;

        AudioManager.instance.PlaySoundLoop(SoundName.RunningWM, WashCycleTime);

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

        if (autoCompleteFlag) {
            yield return new WaitForLaundromatSeconds(2.0f);
            autoCompleteFlag = false;
        }
        else {
            yield return new WaitForLaundromatSeconds(WashCycleTime);
        }

        foreach (Garment garment in contents) {
            if(!garment.Colored() && washSetting == WashSetting.Hot && containsColoredGarments) {
                garment.color = GarmentColor.Pink;
                garment.Dyed = true;
            }

            if (garment.fabric.washingRestrictions == WashingRestrictions.HandWashOnly)
                garment.Torn = true;

            if (garment.fabric.washingRestrictions == WashingRestrictions.ColdOnly && washSetting == WashSetting.Hot)
                garment.Melted = true;

            if(garment.fabric.washingRestrictions == WashingRestrictions.HotOnly && washSetting == WashSetting.Cold) {
                garment.Shrunk = true;
            }

            if (Detergent) {
                garment.Clean = true;
                if (garment.fabric.washingRestrictions == WashingRestrictions.NoDetergent)
                    garment.Dyed = true;
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

    protected override List<Garment> GetCustomContainerGarments() {
        return contents;
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