using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garment
{
    public Fabric fabric;
    public Color color;
    public int size;
    public string clientName; //TODO: Client class

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

        clientName = "John Johnson";
        foldingSteps = 3;
        currentFoldingStep = 0;
        size = 1; //TODO: Add to constructor
    }

    public void Fold() {
        currentFoldingStep = (currentFoldingStep + 1) % (foldingSteps + 1);
    }

    public bool Colored() {
        return !(color == GarmentColor.White);
    }
}

public static class GarmentColor {
    public static Color White = Color.white;
    public static Color Red = Color.red;
    public static Color Pink = new Color(1.0f, 0.0f, 1.0f);
}
