﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerLintTrap : LaundryObject
{
    public Sprite openCleanSprite;
    public Sprite openDirtySprite;
    public Sprite closedSprite;

    public LaundryButton startButton;

    public SpriteRenderer lintTrapSpriteRenderer;

    private Dryer dryer;
    private bool open;

    private void Start() {
        dryer = GetComponentInParent<Dryer>();
    }

    public override void OnInteract() {
        if (dryer.state != DryerState.Running) {
            if (!open) {
                Open();
            }
            else if (open && !dryer.lintTrapClean) {
                Clean();
            }
            else if (open && dryer.lintTrapClean) {
                Close();
            }
        }
    }

    private void Open() {
        if (dryer.lintTrapClean) {
            lintTrapSpriteRenderer.sprite = openCleanSprite;
        }
        else {
            lintTrapSpriteRenderer.sprite = openDirtySprite;
        }
        open = true;
        startButton.pressed = true;
    }

    private void Clean() {
        dryer.CleanLintTrap();
    }

    private void Close() {
        lintTrapSpriteRenderer.sprite = closedSprite;
        open = false;
        startButton.pressed = false;
    }
}
