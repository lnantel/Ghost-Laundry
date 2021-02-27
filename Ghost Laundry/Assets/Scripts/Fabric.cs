using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabric
{
    public string name;
    public Sprite pattern;
    public Sounds grabSound;
    public Sounds dropSound;

    public float ironingTime;

    public Fabric(FabricType type) {
        switch (type) {
            case FabricType.Cotton:
                name = "Cotton";
                ironingTime = 3.0f;
                pattern = null;
                grabSound = Sounds.CottonGrab;
                dropSound = Sounds.CottonDrop;
                break;
            case FabricType.Wool:
                name = "Wool";
                ironingTime = 3.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/test_pattern");
                grabSound = Sounds.WoolGrab;
                dropSound = Sounds.WoolDrop;
                break;
            case FabricType.Synthetic:
                name = "Synthetic";
                ironingTime = 3.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Synthetique_v2");
                grabSound = Sounds.SyntheticGrab;
                dropSound = Sounds.SyntheticDrop;
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
    Synthetic
}
