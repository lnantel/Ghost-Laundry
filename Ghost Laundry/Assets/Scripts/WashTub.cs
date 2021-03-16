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

    //Plunge makes a given garment wet.
    public void Plunge(Garment garment) {
        garment.Dry = false;
    }
}
