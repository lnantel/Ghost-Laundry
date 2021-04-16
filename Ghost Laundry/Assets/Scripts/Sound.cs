using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Sound {
    public SoundName Name;
    public AudioClip Clip;
    [Range(0.0f, 10.0f)]
    public float Volume;
    [Range(-10.0f, 10.0f)]
    public float Pitch;

    public Sound(SoundName name, AudioClip clip, float volume = 1.0f, float pitch = 1.0f) {
        Name = name;
        Clip = clip;
        Volume = volume;
        Pitch = pitch;
    }
}

public enum SoundName
{
    Dash,
    Collision,
    MoneyGain,
    Fold1,
    Fold2,
    Fold3,
    Fold4,
    SyntheticGrab,
    SyntheticDrop,
    CottonGrab,
    CottonDrop,
    WoolGrab,
    WoolDrop,
    BoneGrab,
    BoneDrop,
    Tick,
    Tock,
    LaundromatOpening,
    OllieArrives,
    OllieWhistle,
    LaundromatClosing,
    CustomerArrives,
    ShirtButton1,
    ShirtButton2,
    ShirtButton3,
    ShirtButton4,
    ShirtButton5,
    ShirtButton6,
    LinenGrab,
    LinenDrop,
    SilkGrab,
    SilkDrop,
    DenimGrab,
    DenimDrop,

    //WM, 9 sons

    OpenWMDoor,
    CloseWMDoor,
    RunningWM,
    OpenDetergentWM,
    CloseDetergentWM,
    PourDetergent,
    StartButtonWM,
    SettingButtonWM,
    EndWMBeep,

    //Dryer 8 sons

    OpenDryerDoor,
    CloseDryerDoor,
    RunningDryer,
    OpenLintTrap,
    CloseLintTrap,
    CleanLintTrap,
    StartButtonDryer,
    SettingButtonDryer,
    EndDryerBeep,

    //IronBoard 4 sons

    IronIsOn,
    IronIsWorking,
    IronisBurning,
    ShiningGarment,

    //Bassine 4 sons
    ScratchGarment, 
    BubbleFoam,
    PourSoap,
    SplashGarment,


    //Emballeur 4 sons
    DropGarmentEmb, 
    OpenEmbDoor,
    CloseEmbDoor,
    ProcessingEmb,

    DropBasket,
    PickUpBasket,

    MoneyTallyLoss,
    MoneyTallyGain,

    SewingMachineUp,
    SewingMachineDown
}