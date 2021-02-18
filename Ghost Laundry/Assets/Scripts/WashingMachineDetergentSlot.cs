﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachineDetergentSlot : LaundryObject
{
    public Sprite openSprite;
    public Sprite closedSprite;
    public Sprite filledSprite;

    public LaundryButton startButton;

    private SpriteRenderer spriteRenderer;

    private WashingMachine washingMachine;
    private bool open;

    private void Start() {
        washingMachine = GetComponentInParent<WashingMachine>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnInteract() {
        if (washingMachine.state != WashingMachineState.Running) {
            if (!open) {
                Open();
            }else if(open && !washingMachine.Detergent) {
                Fill();
            }else if(open && washingMachine.Detergent) {
                Close();
            }
        }
    }

    private void Open() {
        if (washingMachine.Detergent)
            spriteRenderer.sprite = filledSprite;
        else
            spriteRenderer.sprite = openSprite;
        open = true;
        startButton.pressed = true;
    }

    private void Fill() {
        washingMachine.AddDetergent();
        spriteRenderer.sprite = filledSprite;
    }

    private void Close() {
        spriteRenderer.sprite = closedSprite;
        open = false;
        startButton.pressed = false;
    }
}