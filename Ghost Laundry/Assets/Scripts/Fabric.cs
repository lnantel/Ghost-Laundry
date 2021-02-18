using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabric
{
    public string name;

    public float ironingTime;

    public Fabric(FabricType type) {
        switch (type) {
            case FabricType.Cotton:
                name = "Cotton";
                ironingTime = 3.0f;
                break;
            case FabricType.Wool:
                name = "Wool";
                ironingTime = 3.0f;
                break;
            case FabricType.Synthetic:
                name = "Synthetic";
                ironingTime = 3.0f;
                break;
        }
    }
}

public enum FabricType {
    Cotton,
    Wool,
    Synthetic
}
