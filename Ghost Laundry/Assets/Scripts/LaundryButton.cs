﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaundryButton : LaundryObject
{
    //Set listener functions in the Inspector
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonPressFailed;
    public UnityEvent OnButtonUnpressed;

    public bool locked;

    //Behaviour
    //Is this button a toggleable switch?
    public bool ToggleSwitch;
    //Does the button spring back when pressed, or does it stay pressed until something else happens?
    public bool springsBack;
    //How long does it take to spring back?
    public float springBackDelay = 0.15f;

    private IEnumerator springBackCoroutine;

    //Sprites
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public Sprite lockedSprite;

    //Buttons is unpressed by default
    [HideInInspector]
    public bool pressed;

    private SpriteRenderer spriteRenderer;

    private Collider2D col;

    protected virtual void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnDisable() {
        if(springBackCoroutine != null) {
            StopCoroutine(springBackCoroutine);
            springBackCoroutine = null;
        }
    }

    public override void OnInteract() {
        if (ToggleSwitch) {
            if (pressed) Unpress();
            else Press();
        }
        else {
            if (!pressed) {
                Press();
            }
            else {
                OnButtonPressFailed.Invoke();
            }
        }
    }

    public override InteractionType GetInteractionType() {
        if(ToggleSwitch || !pressed)
            return InteractionType.Button;

        return InteractionType.None;
    }

    private void Update() {
        if(!ToggleSwitch && pressed && springsBack && springBackCoroutine == null) {
            springBackCoroutine = springBack();
            StartCoroutine(springBackCoroutine);
        }

        if (pressed && pressedSprite != null)
            spriteRenderer.sprite = pressedSprite;

        if (!pressed && unpressedSprite != null)
            spriteRenderer.sprite = unpressedSprite;

        if (locked && lockedSprite != null)
            spriteRenderer.sprite = lockedSprite;
    }

    public virtual void Press() {
        if (!pressed && !locked) {
            OnButtonPressed.Invoke();
            pressed = true;
        }
    }

    public virtual void Unpress() {
        if (pressed && !locked) {
            pressed = false;
            OnButtonUnpressed.Invoke();
        }
    }

    private IEnumerator springBack() {
        yield return new WaitForLaundromatSeconds(springBackDelay);
        Unpress();
        springBackCoroutine = null;
    }

    public override void OnRelease() {
        if (HoveredOver) OnInteract();
    }
}
