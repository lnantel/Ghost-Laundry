using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WashTub : WorkStation
{
    public static Action SoapLevelChanged;

    public int SoapLevel;
    public int MaxSoapLevel;
    public bool IsSoapy { get => SoapLevel > 0; }
    public float ScrubbingStrength;

    private IEnumerator SoundCoroutine; 

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("WashTubArea");
        base.Start();
    }

    //Replenishes the tub's soap level
    public void RefillSoap() {
        if(SoapLevel <= MaxSoapLevel) {
            if (DetergentManager.instance.UseDetergent()) {
                SoapLevel = MaxSoapLevel;
                if (SoapLevelChanged != null) SoapLevelChanged();
            }
        }
    }

    //Scrub increases a given Garment's cleanliness based on the 'distance' argument, if the garment is wet and the tub is soapy. 
    //Returns true if the garment was successfully scrubbed.
    public bool Scrub(Garment garment, float distance) {
        Debug.Log(distance * ScrubbingStrength);
        if(!garment.Dry && distance * ScrubbingStrength > 0.001f){
            if(SoundCoroutine == null){
                SoundCoroutine = ScrubbingSounds();
                StartCoroutine(SoundCoroutine); 
            }
        }
        if(!garment.Dry && IsSoapy) {  
            bool garmentWasDirty = !garment.Clean;
            garment.Cleanliness = garment.Cleanliness + distance * ScrubbingStrength;
            //If the garment was successfully cleaned, decrease the soap level
            if (garment.Clean && garmentWasDirty) {
                SoapLevel--;
                if (SoapLevelChanged != null) SoapLevelChanged();
            }
            return true;
        }
        return false;
    }

    IEnumerator ScrubbingSounds(){

        AudioManager.instance.PlaySound(Sounds.ScratchGarment);
        if(IsSoapy){
            AudioManager.instance.PlaySound(Sounds.BubbleFoam);
        }
        yield return new WaitForLaundromatSeconds(0.5f);
        SoundCoroutine = null; 
    }


    //Plunge makes a given garment wet.
    public void Plunge(Garment garment) {
        garment.Dry = false;
    }

}
