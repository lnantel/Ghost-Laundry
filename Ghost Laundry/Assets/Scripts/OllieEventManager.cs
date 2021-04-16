using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class OllieEventManager : MonoBehaviour
{
    public int SafetyPoints { get => m_SafetyPoints; set => SetSafetyPoints(value); }
    private int m_SafetyPoints;

    public Flowchart SafetyEnding;
    public Flowchart DangerEnding;
    public Flowchart NeutralEnding;

    public Flowchart Epilogue;

    public Flowchart GetEnding() {
        if (SafetyPoints == 3) return SafetyEnding;
        if (SafetyPoints == -3) return DangerEnding;
        else return NeutralEnding;
    }

    private void SetSafetyPoints(int value) {
        m_SafetyPoints = value;
        Epilogue.SetIntegerVariable("SafetyPoints", m_SafetyPoints);
    }
}
