using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnimationEventHandler : MonoBehaviour
{
    public void DryerShake() {
        if(TimeManager.instance.TimeIsPassing)
            AudioManager.instance.PlaySound(SoundName.DryerShake);
    }

    public void WashingMachineShake() {
        if (TimeManager.instance.TimeIsPassing)
            AudioManager.instance.PlaySound(SoundName.WashingMachineShake);
    }
}
