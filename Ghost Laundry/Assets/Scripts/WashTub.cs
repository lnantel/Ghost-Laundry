using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashTub : WorkStation
{
    public int SoapLevel;
    public bool IsSoapy { get => SoapLevel > 0; }

    public void AddSoap() {
        SoapLevel = 10;
    }

    public void CleanGarment(Garment garment) {
        if (IsSoapy) {
            garment.Clean = true;
            SoapLevel--;
        }
    }
}
