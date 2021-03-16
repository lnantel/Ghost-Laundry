using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewingMachinePedal : LaundryObject
{
    private SewingMachine sewingMachine;
    private SpriteRenderer spriteRenderer;

    public Sprite unpressedSprite;
    public Sprite pressedSprite;

    private void Start() {
        sewingMachine = GetComponentInParent<SewingMachine>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = unpressedSprite;
    }

    public override void OnGrab() {
        spriteRenderer.sprite = pressedSprite;
    }

    public override void Drag(Vector2 cursorPosition) {
        sewingMachine.Sew();
    }

    public override void OnRelease() {
        spriteRenderer.sprite = unpressedSprite;
    }
}
