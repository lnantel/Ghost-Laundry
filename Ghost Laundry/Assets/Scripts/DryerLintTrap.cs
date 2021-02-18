using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerLintTrap : LaundryObject
{
    public Sprite openSprite;
    public Sprite closedSprite;

    public LaundryButton startButton;

    public SpriteRenderer lintTrapSpriteRenderer;
    public SpriteRenderer lintSpriteRenderer;

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
        if (!dryer.lintTrapClean) {
            lintSpriteRenderer.enabled = true;
        }
        lintTrapSpriteRenderer.sprite = openSprite;
        open = true;
        startButton.pressed = true;
    }

    private void Clean() {
        dryer.CleanLintTrap();
        lintSpriteRenderer.enabled = false;
    }

    private void Close() {
        lintTrapSpriteRenderer.sprite = closedSprite;
        open = false;
        startButton.pressed = false;
        lintSpriteRenderer.enabled = false;
    }
}
