﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarmentSock : Garment {
    private GarmentSock pairedSock;

    public GarmentSock(Fabric fabric, Color color, bool clean = false, bool dry = true, bool pressed = false, bool folded = false, bool ruined = false) : base(fabric, color, clean, dry, pressed, folded, ruined) {
        size = 1;
        foldingSteps = 1;
        laundryGarmentPrefab = (GameObject)Resources.Load("LaundryGarmentSock");
    }

    public GarmentSock(GarmentSock other) : base(other){
        this.fabric = other.fabric;
        this.color = other.color;
        this.clean = other.clean;
        this.dry = other.dry;
        this.pressed = other.pressed;
        this.folded = other.folded;
        this.ruined = other.ruined;
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

    protected override void SetRuined(bool value) {
        if (pairedSock == null)
            base.SetRuined(value);
        else {
            base.SetRuined(value);
            pairedSock.SetRuined(value);
        }
    }

    public void PairUp(GarmentSock other) {
        pairedSock = other;
        AudioManager.instance.PlaySound(Sounds.Fold1 + currentFoldingStep);
        currentFoldingStep = 1;
    }

    public GarmentSock SeparatePair() {
        GarmentSock sock = pairedSock;
        pairedSock = null;
        AudioManager.instance.PlaySound(Sounds.Fold1 + currentFoldingStep);
        currentFoldingStep = 0;
        return sock;
    }
}
