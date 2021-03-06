﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garment
{
    public Fabric fabric;
    public Color color;
    public int size;
    public int clotheslinePegs;

    [HideInInspector]
    public int customerID;

    public GarmentType type;

    //States
    protected float cleanliness;
    protected bool pressed;
    protected bool folded;
    protected bool shrunk;
    protected bool burned;
    protected bool melted;
    protected bool dyed;
    protected bool torn;
    protected float humidity;

    public int foldingSteps;
    [HideInInspector]
    public int currentFoldingStep;

    public GameObject laundryGarmentPrefab;

    public bool Clean { get => GetClean(); set => SetClean(value); }
    public float Cleanliness { get => GetCleanliness(); set => SetCleanliness(value); }
    public bool Dry { get => GetDry(); set => SetDry(value); }
    public bool Pressed { get => GetPressed(); set => SetPressed(value); }
    public bool Folded { get => GetFolded(); }
    public bool Ruined { get => GetRuined(); }
    public bool Shrunk { get => GetShrunk(); set => SetShrunk(value); }
    public bool Burned { get => GetBurned(); set => SetBurned(value); }
    public bool Melted { get => GetMelted(); set => SetMelted(value); }
    public bool Dyed { get => GetDyed(); set => SetDyed(value); }
    public bool Torn { get => GetTorn(); set => SetTorn(value); }
    public float Humidity { get => GetHumidity(); set => SetHumidity(value); }

    //Accessors
    protected virtual bool GetClean() {
        return cleanliness >= 0.99f;
    }

    protected virtual void SetClean(bool value) {
        if (value)
            cleanliness = 1.0f;
        else
            cleanliness = 0.0f;
    }

    protected virtual float GetCleanliness() {
        return cleanliness;
        
    }

    protected virtual void SetCleanliness(float value) {
        cleanliness = Mathf.Clamp01(value);
    }

    protected virtual bool GetDry() {
        return humidity < 0.01f;
    }

    protected virtual void SetDry(bool value) {
        if (value)
            humidity = 0.0f;
        else
            humidity = 1.0f;
    }

    protected virtual bool GetPressed() {
        if (fabric.name.Equals("Silk")) return Clean && Dry && !Ruined;
        return pressed;
    }

    protected virtual void SetPressed(bool value) {
        pressed = value;
    }

    protected virtual bool GetFolded() {
        return currentFoldingStep == foldingSteps;
    }

    protected virtual bool GetRuined() {
        return (shrunk || burned || melted || dyed || torn);
    }

    protected virtual bool GetShrunk() {
        return shrunk;
    }

    protected virtual void SetShrunk(bool value) {
        shrunk = value;
    }

    protected virtual bool GetBurned() {
        return burned;
    }

    protected virtual void SetBurned(bool value) {
        burned = value;
    }

    protected virtual bool GetMelted() {
        return melted;
    }

    protected virtual void SetMelted(bool value) {
        melted = value;
    }

    protected virtual bool GetDyed() {
        return dyed;
    }

    protected virtual void SetDyed(bool value) {
        dyed = value;
    }

    protected virtual bool GetTorn() {
        return torn;
    }

    protected virtual void SetTorn(bool value) {
        torn = value;
    }

    protected virtual float GetHumidity() {
        return humidity;
    }

    protected virtual void SetHumidity(float value) {
        humidity = Mathf.Clamp01(value);
    }

    public Garment(Fabric fabric, Color color, float cleanliness = 0.0f, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) {
        this.fabric = fabric;
        this.color = color;
        this.cleanliness = cleanliness;
        this.humidity = humidity;
        this.pressed = pressed;
        this.folded = folded;
        this.shrunk = shrunk;
        this.dyed = dyed;
        this.melted = melted;
        this.torn = torn;
        this.burned = burned;

        customerID = 0;
        currentFoldingStep = 0;

        //Overridden by garment category
        type = GarmentType.Dress;
        foldingSteps = 3;
        size = 1;
        clotheslinePegs = 1;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarment");
    }

    public Garment(Garment other) {
        this.fabric = other.fabric;
        this.color = other.color;
        this.cleanliness = other.cleanliness;
        this.humidity = other.humidity;
        this.pressed = other.pressed;
        this.folded = other.folded;
        this.shrunk = other.shrunk;
        this.dyed = other.dyed;
        this.melted = other.melted;
        this.torn = other.torn;
        this.burned = other.burned;
        this.customerID = other.customerID;
        this.currentFoldingStep = other.currentFoldingStep;
        this.size = other.size;
        this.foldingSteps = other.foldingSteps;
        this.laundryGarmentPrefab = other.laundryGarmentPrefab;
    }

    public virtual void Fold() {
        AudioManager.instance.PlaySound(SoundName.Fold1 + currentFoldingStep);
        currentFoldingStep = (currentFoldingStep + 1) % (foldingSteps + 1);
    }

    public bool Colored() {
        return !(color == GarmentColor.White);
    }

    public static Garment GetRandomGarment() {
        Fabric randomFabric = Fabric.GetRandomFabric();
        Color randomColor = GarmentColor.RandomColor();
        int type = Random.Range(0, 7);
        switch (type) {
            case 0:
                return new GarmentTop(randomFabric, randomColor);
            case 1:
                return new GarmentPants(randomFabric, randomColor);
            case 2:
                return new GarmentUnderwear(randomFabric, randomColor);
            case 3:
                return new GarmentSock(randomFabric, randomColor);
            case 4:
                return new GarmentSkirt(randomFabric, randomColor);
            case 5:
                return new GarmentDress(randomFabric, randomColor);
            case 6:
                return new GarmentShirt(randomFabric, randomColor);
            default:
                return new Garment(randomFabric, randomColor);
        }
    }

    public virtual LaundryGarment CreateLaundryGarment(Vector3 position, Quaternion rotation, Transform parent) {
        //GameObject obj = GameObject.Instantiate(laundryGarmentPrefab, position, rotation, parent);
        //LaundryGarment laundryGarment = obj.GetComponent<LaundryGarment>();
        //laundryGarment.garment = this;
        LaundryGarment laundryGarment = LaundryGarmentPoolManager.instance.SpawnLaundryGarment(position, parent, this);
        return laundryGarment;
    }
}

public static class GarmentColor {
    public static Color White = Color.white;
    public static Color Red = new Color(240f / 256f, 69f / 256f, 69f / 256f);
    public static Color Pink = new Color(240f / 256f, 128f / 256f, 215f / 256f);
    public static Color Sky = new Color(91f / 256f, 144f / 256f, 240f / 256f);
    public static Color Salmon = new Color(240f / 256f, 123f / 256f, 103f / 256f);
    public static Color Teal = new Color(80f / 256f, 222f / 256f, 240f / 256f);
    public static Color Golden = new Color(240f / 256f, 181f / 256f, 55f / 256f);
    public static Color Mint = new Color(67f / 256f, 240f / 256f, 164f / 256f);

    public static Color RandomColor() {
        int rand = Random.Range(0, 13);
        switch (rand) {
            case 0: return Red;
            case 1: return Pink;
            case 2: return Sky;
            case 3: return Salmon;
            case 4: return Teal;
            case 5: return Golden;
            case 6: return Mint;
            default: return White;
        }
    }
}

[System.Serializable]
public enum GarmentType {
    Top,
    Pants,
    Sock,
    Underwear,
    Dress,
    Skirt,
    Shirt,
    Femur
}