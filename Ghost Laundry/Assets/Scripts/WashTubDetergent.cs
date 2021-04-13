using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashTubDetergent : LaundryObject
{
    private WashTub washTub;

    private void Start() {
        washTub = GetComponentInParent<WashTub>();
    }

    public override void OnInteract() {
        washTub.RefillSoap();
        AudioManager.instance.PlaySound(SoundName.PourSoap);

    }
}
