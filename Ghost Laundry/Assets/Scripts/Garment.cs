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
    protected bool clean;
    protected bool dry;
    protected bool pressed;
    protected bool folded;
    protected bool ruined;

    public int foldingSteps;
    public int currentFoldingStep;

    protected GameObject laundryGarmentPrefab;

    public bool Clean { get => GetClean(); set => SetClean(value); }
    public bool Dry { get => GetDry(); set => SetDry(value); }
    public bool Pressed { get => GetPressed(); set => SetPressed(value); }
    public bool Folded { get => GetFolded(); set => SetFolded(value); }
    public bool Ruined { get => GetRuined(); set => SetRuined(value); }

    //Accessors
    protected virtual bool GetClean() {
        return clean;
    }

    protected virtual void SetClean(bool value) {
        clean = value;
    }

    protected virtual bool GetDry() {
        return dry;
    }

    protected virtual void SetDry(bool value) {
        dry = value;
    }

    protected virtual bool GetPressed() {
        return pressed;
    }

    protected virtual void SetPressed(bool value) {
        pressed = value;
    }

    protected virtual bool GetFolded() {
        return folded;
    }

    protected virtual void SetFolded(bool value) {
        folded = value;
    }

    protected virtual bool GetRuined() {
        return ruined;
    }

    protected virtual void SetRuined(bool value) {
        ruined = value;
    }

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
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarment");
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
        int type = Random.Range(0, 4);
        switch (type) {
            case 0:
                return new GarmentTop(randomFabric, randomColor);
            case 1:
                return new GarmentPants(randomFabric, randomColor);
            case 2:
                return new GarmentUnderwear(randomFabric, randomColor);
            case 3:
                return new GarmentSock(randomFabric, randomColor);
            default:
                return new Garment(randomFabric, randomColor);
        }
    }

    public virtual LaundryGarment CreateLaundryGarment(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject obj = GameObject.Instantiate(laundryGarmentPrefab, position, rotation, parent);
        LaundryGarment laundryGarment = obj.GetComponent<LaundryGarment>();
        laundryGarment.garment = this;
        return laundryGarment;
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
