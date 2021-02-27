using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabric
{
    public string name;
    public Sprite pattern;

    public float ironingTime;

    public Fabric(FabricType type) {
        switch (type) {
            case FabricType.Cotton:
                name = "Cotton";
                ironingTime = 3.0f;
                pattern = null;
                break;
            case FabricType.Wool:
                name = "Wool";
                ironingTime = 3.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/test_pattern");
                break;
            case FabricType.Synthetic:
                name = "Synthetic";
                ironingTime = 3.0f;
                pattern = Resources.Load<Sprite>("Fabric Patterns/Synthetique_v2");
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
