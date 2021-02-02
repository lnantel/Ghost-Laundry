using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garment
{
    public Fabric fabric;
    public Color color;

    //States
    public bool clean;
    public bool dry;
    public bool pressed;
    public bool folded;
    public bool ruined;

    public int foldingSteps;
    public int currentFoldingStep;

    public Garment(Fabric fabric, Color color, bool clean, bool dry, bool pressed = false, bool folded = false, bool ruined = false) {
        this.fabric = fabric;
        this.color = color;
        this.clean = clean;
        this.dry = dry;
        this.pressed = pressed;
        this.folded = folded;
        this.ruined = ruined;

        foldingSteps = 3;
        currentFoldingStep = 0;
    }

    public void Fold() {
        currentFoldingStep = (currentFoldingStep + 1) % (foldingSteps + 1);
    }
}
