using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class OllieEventManager : MonoBehaviour
{
    public int SafetyPoints;

    public Flowchart SafetyEnding;
    public Flowchart DangerEnding;
    public Flowchart NeutralEnding;

    public Flowchart GetEnding() {
        if (SafetyPoints == 3) return SafetyEnding;
        if (SafetyPoints == -3) return DangerEnding;
        else return NeutralEnding;
    }
}
