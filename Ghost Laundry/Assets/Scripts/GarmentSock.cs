using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentSock : Garment {
    public GarmentSock pairedSock;

    public GarmentSock(Fabric fabric, Color color, float cleanliness = 0.0f, float humidity = 0.0f, bool pressed = false, bool folded = false, bool shrunk = false, bool burned = false, bool dyed = false, bool torn = false, bool melted = false) : base(fabric, color, cleanliness, humidity, pressed, folded, shrunk, burned, dyed, torn, melted) {
        size = 1;
        foldingSteps = 1;
        clotheslinePegs = 1;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentSock");
    }

    public GarmentSock(GarmentSock other) : base(other){
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

    //Custom accessors to manage paired socks
    protected override bool GetClean() {
        if (pairedSock == null)
            return base.GetClean();
        else
            return base.GetClean() && pairedSock.GetClean();
    }

    protected override void SetClean(bool value) {
        if (pairedSock == null)
            base.SetClean(value);
        else {
            base.SetClean(value);
            pairedSock.SetClean(value);
        }
    }

    protected override float GetCleanliness() {
        if (pairedSock != null) return Mathf.Max(base.GetCleanliness(), pairedSock.GetCleanliness());
        return base.GetCleanliness();
    }

    protected override void SetCleanliness(float value) {
        base.SetCleanliness(value);
        if (pairedSock != null) pairedSock.SetCleanliness(value);
    }

    protected override bool GetPressed() {
        if (pairedSock == null)
            return base.GetPressed();
        else
            return base.GetPressed() && pairedSock.GetPressed();
    }

    protected override void SetPressed(bool value) {
        if (pairedSock == null)
            base.SetPressed(value);
        else {
            base.SetPressed(value);
            pairedSock.SetPressed(value);
        }
    }

    protected override bool GetDry() {
        if (pairedSock == null)
            return base.GetDry();
        else
            return base.GetDry() && pairedSock.GetDry();
    }

    protected override void SetDry(bool value) {
        if (pairedSock == null)
            base.SetDry(value);
        else {
            base.SetDry(value);
            pairedSock.SetDry(value);
        }
    }

    protected override bool GetRuined() {
        if (pairedSock == null)
            return base.GetRuined();
        else
            return base.GetRuined() || pairedSock.GetRuined();
    }

    protected override bool GetShrunk() {
        if (pairedSock == null)
            return base.GetShrunk();
        else
            return base.GetShrunk() || pairedSock.GetShrunk();
    }

    protected override void SetShrunk(bool value) {
        base.SetShrunk(value);
        if (pairedSock != null) pairedSock.SetShrunk(value);
    }

    protected override bool GetBurned() {
        if (pairedSock == null)
            return base.GetBurned();
        else
            return base.GetBurned() || pairedSock.GetBurned();
    }

    protected override void SetBurned(bool value) {
        base.SetBurned(value);
        if (pairedSock != null) pairedSock.SetBurned(value);
    }

    protected override bool GetMelted() {
        if (pairedSock == null)
            return base.GetMelted();
        else
            return base.GetMelted() || pairedSock.GetMelted();
    }

    protected override void SetMelted(bool value) {
        base.SetMelted(value);
        if (pairedSock != null) pairedSock.SetMelted(value);
    }

    protected override bool GetDyed() {
        if (pairedSock == null)
            return base.GetDyed();
        else
            return base.GetDyed() || pairedSock.GetDyed();
    }

    protected override void SetDyed(bool value) {
        base.SetDyed(value);
        if (pairedSock != null) pairedSock.SetDyed(value);
    }

    protected override bool GetTorn() {
        if (pairedSock == null)
            return base.GetTorn();
        else
            return base.GetTorn() || pairedSock.GetTorn();
    }

    protected override void SetTorn(bool value) {
        base.SetTorn(value);
        if (pairedSock != null) pairedSock.SetTorn(value);
    }

    protected override float GetHumidity() {
        if (pairedSock == null) return base.GetHumidity();
        return Mathf.Max(base.GetHumidity(), pairedSock.GetHumidity());
    }

    protected override void SetHumidity(float value) {
        base.SetHumidity(value);
        if (pairedSock != null) pairedSock.SetHumidity(value);
    }

    public void PairUp(GarmentSock other) {
        pairedSock = other;
        AudioManager.instance.PlaySound(Sounds.Fold1 + currentFoldingStep);
        currentFoldingStep = 1;
        other.currentFoldingStep = 1;
    }

    public GarmentSock SeparatePair() {
        GarmentSock sock = pairedSock;
        pairedSock = null;
        AudioManager.instance.PlaySound(Sounds.Fold1 + currentFoldingStep);
        currentFoldingStep = 0;
        sock.currentFoldingStep = 0;
        return sock;
    }
}
