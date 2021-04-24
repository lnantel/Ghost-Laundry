using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnimationEventHandler : MonoBehaviour
{
    public void DryerShake() {
        AudioManager.instance.PlaySound(SoundName.DryerShake);
    }

    public void WashingMachineShake() {
        AudioManager.instance.PlaySound(SoundName.WashingMachineShake);
    }
}
