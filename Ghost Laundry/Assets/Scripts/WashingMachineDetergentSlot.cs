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
    public bool open;

    private void Start() {
        washingMachine = GetComponentInParent<WashingMachine>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnInteract() {
        if (!washingMachine.DetergentSlotLocked && washingMachine.state != WashingMachineState.Running) {
            if (!open) {
                Open();
            }else if (open) {
                if (washingMachine.Detergent) Close();
                else {
                    if (DetergentManager.instance.UseDetergent()) Fill();
                    else Close();
                }
            }
        }
    }

    public override void OnRelease() {
        OnInteract();
    }

    public override InteractionType GetInteractionType() {
        if(washingMachine.state != WashingMachineState.Running) {
            if (!open)
                return InteractionType.Open;

            if (open && !washingMachine.Detergent && DetergentManager.instance.CurrentAmount > 0)
                return InteractionType.Detergent;

            return InteractionType.Close;
        }

        return InteractionType.None;
    }


    private void Open() {
        if (washingMachine.Detergent)
            spriteRenderer.sprite = filledSprite;
        else
            spriteRenderer.sprite = openSprite;
        open = true;
        startButton.locked = true;
        AudioManager.instance.PlaySound(SoundName.OpenDetergentWM);
        

    }

    private void Fill() {
        washingMachine.Detergent = true;
        spriteRenderer.sprite = filledSprite;
        AudioManager.instance.PlaySound(SoundName.PourDetergent);
    }

    private void Close() {
        spriteRenderer.sprite = closedSprite;
        open = false;
        startButton.locked = false;
        AudioManager.instance.PlaySound(SoundName.CloseDetergentWM);

    }
}
