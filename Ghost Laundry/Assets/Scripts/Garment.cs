﻿using System.Collections;
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

    public Garment(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool ruined = false) {
        this.fabric = fabric;
        this.color = color;
        this.clean = clean;
        this.dry = dry;
        this.pressed = pressed;
        this.folded = folded;
        this.ruined = ruined;

        clientName = "John Johnson";
        currentFoldingStep = 0;

        //Overridden by garment category
        foldingSteps = 3;
        size = 1;
    }

    public virtual void Fold() {
        currentFoldingStep = (currentFoldingStep + 1) % (foldingSteps + 1);
    }

    public bool Colored() {
        return !(color == GarmentColor.White);
    }

    public static Garment GetRandomGarment() {
        Fabric randomFabric = new Fabric((FabricType)Random.Range(0, 3));
        Color randomColor = GarmentColor.RandomColor();
        return new Garment(randomFabric, randomColor);
    }
}

public static class GarmentColor {
    public static Color White = Color.white;
    public static Color Red = Color.red;
    public static Color Pink = new Color(1.0f, 0.0f, 1.0f);

    public static Color RandomColor() {
        int rand = Random.Range(0, 3);
        switch (rand) {
            case 0: return White;
            case 1: return Red;
            case 2: return Pink;
            default: return White;
        }
    }
}
