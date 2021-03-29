using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaundryButton : LaundryObject
{
    //Set listener functions in the Inspector
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonPressFailed;
    public UnityEvent OnButtonUnpressed;

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

    //Buttons is unpressed by default
    [HideInInspector]
    public bool pressed;

    private SpriteRenderer spriteRenderer;

    protected virtual void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract() {
        if (ToggleSwitch) {
            if (pressed) Unpress();
            else Press();
        }

        if (!pressed) {
            Press();
        }
        else {
            OnButtonPressFailed.Invoke();
        }
    }

    private void Update() {
        if(pressed && springsBack && springBackCoroutine == null) {
            springBackCoroutine = springBack();
            StartCoroutine(springBackCoroutine);
        }

        if (pressed && pressedSprite != null)
            spriteRenderer.sprite = pressedSprite;

        if (!pressed && unpressedSprite != null)
            spriteRenderer.sprite = unpressedSprite;
    }

    public virtual void Press() {
        if (!pressed) {
            OnButtonPressed.Invoke();
            pressed = true;
        }
    }

    public virtual void Unpress() {
        if (pressed) {
            pressed = false;
            OnButtonUnpressed.Invoke();
        }
    }

    private IEnumerator springBack() {
        yield return new WaitForLaundromatSeconds(springBackDelay);
        Unpress();
        springBackCoroutine = null;
    }

}
