using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerSettingDial : LaundryButton
{
    Dryer dryer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dryer = GetComponentInParent<Dryer>();
        Rotate();
    }

    public override void Press() {
        base.Press();
        Rotate();
    }

    private void Rotate() {
        if (dryer.dryerSetting == DryerSetting.High) {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            
        }
        else if (dryer.dryerSetting == DryerSetting.Low) {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    public void ToggleDryerSetting() {
        dryer.ToggleDryerSetting();
        AudioManager.instance.PlaySound(Sounds.SettingButtonDryer);

    }
}
