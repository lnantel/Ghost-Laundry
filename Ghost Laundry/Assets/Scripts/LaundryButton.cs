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
    //Does the button spring back when pressed, or does it stay pressed until something else happens?
    public bool springsBack;
    //How long does it take to spring back?
    public float springBackDelay = 0.15f;

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
        if (!pressed) {
            Press();
        }
        else {
            OnButtonPressFailed.Invoke();
        }
    }

    public virtual void Press() {
        OnButtonPressed.Invoke();
        pressed = true;
        if(pressedSprite != null)
            spriteRenderer.sprite = pressedSprite;
        if (springsBack) {
            StartCoroutine(springBack());
        }
    }

    public virtual void Unpress() {
        if (pressed) {
            pressed = false;
            OnButtonUnpressed.Invoke();
            if(unpressedSprite != null)
                spriteRenderer.sprite = unpressedSprite;
        }
    }

    private IEnumerator springBack() {
        yield return new WaitForSeconds(springBackDelay);
        Unpress();
    }

}
