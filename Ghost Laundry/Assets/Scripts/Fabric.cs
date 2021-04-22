using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabric
{
    public string name;
    public Sprite pattern;
    public SoundName grabSound;
    public SoundName dropSound;
    public WashingRestrictions washingRestrictions;
    public DryingRestrictions dryingRestrictions;
    public PressingRestrictions pressingRestrictions;

    public float ironingDistance;

    public Fabric(FabricType type) {
        switch (type) {
            case FabricType.Cotton:
                name = "Cotton";
                ironingDistance = 15.0f;
                pattern = null;
                grabSound = SoundName.CottonGrab;
                dropSound = SoundName.CottonDrop;
                washingRestrictions = WashingRestrictions.None;
                dryingRestrictions = DryingRestrictions.None;
                pressingRestrictions = PressingRestrictions.None;
                break;
            case FabricType.Wool:
                name = "Wool";
                ironingDistance = 15.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Laine_v1");
                grabSound = SoundName.WoolGrab;
                dropSound = SoundName.WoolDrop;
                washingRestrictions = WashingRestrictions.HotOnly;
                dryingRestrictions = DryingRestrictions.LowOnly;
                pressingRestrictions = PressingRestrictions.None;
                break;
            case FabricType.Synthetic:
                name = "Synthetic";
                ironingDistance = 15.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Synthetique_v2");
                grabSound = SoundName.SyntheticGrab;
                dropSound = SoundName.SyntheticDrop;
                washingRestrictions = WashingRestrictions.ColdOnly;
                dryingRestrictions = DryingRestrictions.None;
                pressingRestrictions = PressingRestrictions.NoIroning;
                break;
            case FabricType.Bone:
                name = "Bone";
                ironingDistance = 15.0f;
                pattern = null;
                grabSound = SoundName.BoneGrab;
                dropSound = SoundName.BoneDrop;
                washingRestrictions = WashingRestrictions.NoDetergent;
                dryingRestrictions = DryingRestrictions.None;
                pressingRestrictions = PressingRestrictions.NoIroning;
                break;
            case FabricType.Linen:
                name = "Linen";
                ironingDistance = 15.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Linen_material"); ;
                grabSound = SoundName.LinenGrab;
                dropSound = SoundName.LinenDrop;
                washingRestrictions = WashingRestrictions.None;
                dryingRestrictions = DryingRestrictions.HangDryOnly;
                pressingRestrictions = PressingRestrictions.None;
                break;
            case FabricType.Silk:
                name = "Silk";
                ironingDistance = 15.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Silk_material"); ;
                grabSound = SoundName.SilkGrab;
                dropSound = SoundName.SilkDrop;
                washingRestrictions = WashingRestrictions.HandWashOnly;
                dryingRestrictions = DryingRestrictions.HangDryOnly;
                pressingRestrictions = PressingRestrictions.NoIroning;
                break;
            case FabricType.Denim:
                name = "Denim";
                ironingDistance = 15.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Denim_material"); ;
                grabSound = SoundName.DenimGrab;
                dropSound = SoundName.DenimDrop;
                washingRestrictions = WashingRestrictions.None;
                dryingRestrictions = DryingRestrictions.LowOnly;
                pressingRestrictions = PressingRestrictions.None;
                break;
        }
    }

    public bool Equals(Fabric other) {
        return name.Equals(other.name);
    }

    public static Fabric GetRandomFabric() {
        int i = Random.Range(0, 6);
        return new Fabric((FabricType)i);
    }
}

[System.Serializable]
public enum FabricType {
    Cotton,
    Wool,
    Synthetic,
    Linen,
    Silk,
    Denim,
    Bone
}

public enum WashingRestrictions {
    None,
    ColdOnly,
    HotOnly,
    HandWashOnly,
    NoDetergent
}

public enum DryingRestrictions {
    None,
    LowOnly,
    HighOnly,
    HangDryOnly
}

public enum PressingRestrictions {
    None,
    NoIroning
}
