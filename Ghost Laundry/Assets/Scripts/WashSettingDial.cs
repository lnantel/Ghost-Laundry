using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashSettingDial : LaundryButton
{
    WashingMachine washingMachine;

    protected override void Start() {
        base.Start();
        washingMachine = GetComponentInParent<WashingMachine>();
        Rotate();
    }

    public override void Press() {
        base.Press();
        Rotate();
    }

    private void Rotate() {
        if (washingMachine.washSetting == WashSetting.Cold) {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

        }
        else if (washingMachine.washSetting == WashSetting.Hot) {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    public void ToggleWashSetting() {
        washingMachine.ToggleWashSetting();
        AudioManager.instance.PlaySound(Sounds.SettingButtonWM);

    }
}
