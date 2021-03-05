using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabric
{
    public string name;
    public Sprite pattern;
    public Sounds grabSound;
    public Sounds dropSound;
    public WashingRestrictions washingRestrictions;
    public DryingRestrictions dryingRestrictions;
    public PressingRestrictions pressingRestrictions;

    public float ironingTime;

    public Fabric(FabricType type) {
        switch (type) {
            case FabricType.Cotton:
                name = "Cotton";
                ironingTime = 3.0f;
                pattern = null;
                grabSound = Sounds.CottonGrab;
                dropSound = Sounds.CottonDrop;
                washingRestrictions = WashingRestrictions.None;
                dryingRestrictions = DryingRestrictions.None;
                pressingRestrictions = PressingRestrictions.None;
                break;
            case FabricType.Wool:
                name = "Wool";
                ironingTime = 3.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Laine_v1");
                grabSound = Sounds.WoolGrab;
                dropSound = Sounds.WoolDrop;
                washingRestrictions = WashingRestrictions.HotOnly;
                dryingRestrictions = DryingRestrictions.LowOnly;
                pressingRestrictions = PressingRestrictions.None;
                break;
            case FabricType.Synthetic:
                name = "Synthetic";
                ironingTime = 3.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Synthetique_v2");
                grabSound = Sounds.SyntheticGrab;
                dropSound = Sounds.SyntheticDrop;
                washingRestrictions = WashingRestrictions.ColdOnly;
                dryingRestrictions = DryingRestrictions.None;
                pressingRestrictions = PressingRestrictions.NoIroning;
                break;
            case FabricType.Bone:
                name = "Bone";
                ironingTime = 10.0f;
                pattern = null;
                grabSound = Sounds.BoneGrab;
                dropSound = Sounds.BoneDrop;
                washingRestrictions = WashingRestrictions.NoDetergent;
                dryingRestrictions = DryingRestrictions.None;
                pressingRestrictions = PressingRestrictions.NoIroning;
                break;
        }
    }

    public bool Equals(Fabric other) {
        return name.Equals(other.name);
    }
}

public enum FabricType {
    Cotton,
    Wool,
    Synthetic,
    Bone
}

public enum WashingRestrictions {
    None,
    ColdOnly,
    HotOnly,
    NoDetergent
}

public enum DryingRestrictions {
    None,
    LowOnly,
    HighOnly
}

public enum PressingRestrictions {
    None,
    NoIroning
}
