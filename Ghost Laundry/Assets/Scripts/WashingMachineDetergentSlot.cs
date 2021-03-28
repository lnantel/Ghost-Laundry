using System.Collections;
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

    private void Open() {
        if (washingMachine.Detergent)
            spriteRenderer.sprite = filledSprite;
        else
            spriteRenderer.sprite = openSprite;
        open = true;
        startButton.pressed = true;
        AudioManager.instance.PlaySound(Sounds.OpenDetergentWM);
        

    }

    private void Fill() {
        washingMachine.Detergent = true;
        spriteRenderer.sprite = filledSprite;
        AudioManager.instance.PlaySound(Sounds.PourDetergent);
    }

    private void Close() {
        spriteRenderer.sprite = closedSprite;
        open = false;
        startButton.pressed = false;
        AudioManager.instance.PlaySound(Sounds.CloseDetergentWM);

    }
}
