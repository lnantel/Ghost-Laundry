using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garment
{
    public Fabric fabric;
    public Color color;
    public int size;

    [HideInInspector]
    public int customerID;

    //States
    protected bool clean;
    protected bool dry;
    protected bool pressed;
    protected bool folded;
    protected bool shrunk;
    protected bool burned;
    protected bool melted;
    protected bool dyed;
    protected bool torn;

    public int foldingSteps;
    [HideInInspector]
    public int currentFoldingStep;

    protected GameObject laundryGarmentPrefab;

    public bool Clean { get => GetClean(); set => SetClean(value); }
    public bool Dry { get => GetDry(); set => SetDry(value); }
    public bool Pressed { get => GetPressed(); set => SetPressed(value); }
    public bool Folded { get => GetFolded(); }
    public bool Ruined { get => GetRuined(); }
    public bool Shrunk { get => GetShrunk(); set => SetShrunk(value); }
    public bool Burned { get => GetBurned(); set => SetBurned(value); }
    public bool Melted { get => GetMelted(); set => SetMelted(value); }
    public bool Dyed { get => GetDyed(); set => SetDyed(value); }
    public bool Torn { get => GetTorn(); set => SetTorn(value); }

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

    public Garment(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) {
        this.fabric = fabric;
        this.color = color;
        this.clean = clean;
        this.dry = dry;
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
        foldingSteps = 3;
        size = 1;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarment");
    }

    public Garment(Garment other) {
        this.fabric = other.fabric;
        this.color = other.color;
        this.clean = other.clean;
        this.dry = other.dry;
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
        AudioManager.instance.PlaySound(Sounds.Fold1 + currentFoldingStep);
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
        GameObject obj = GameObject.Instantiate(laundryGarmentPrefab, position, rotation, parent);
        LaundryGarment laundryGarment = obj.GetComponent<LaundryGarment>();
        laundryGarment.garment = this;
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
        int rand = Random.Range(0, 8);
        switch (rand) {
            case 0: return White;
            case 1: return Red;
            case 2: return Pink;
            case 3: return Sky;
            case 4: return Salmon;
            case 5: return Teal;
            case 6: return Golden;
            case 7: return Mint;
            default: return White;
        }
    }
}
