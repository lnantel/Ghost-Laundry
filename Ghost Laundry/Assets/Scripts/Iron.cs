using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : LaundryObject
{
    public float boardHeight;

    public LaundryIroningBoard laundryIroningBoard;

    public SpriteRenderer ironSpriteRenderer;
    public Sprite spriteIronStandingOn;
    public Sprite spriteIronStandingOff;
    public Sprite spriteIronFlatOn;
    public Sprite spriteIronFlatOff;

    public ParticleSystem steam;
    public ParticleSystem smoke;

    private Rigidbody2D rb;
    private IronState state;
    private SteamState steamState;
    private bool onIroningBoard;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        steam.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public override void Drag(Vector2 cursorPosition) {
        if (onIroningBoard) {
            if (Mathf.Abs(cursorPosition.y - rb.position.y) > 2.0f)
                TakeOffIroningBoard();
        }
        rb.MovePosition(cursorPosition);
        rb.velocity = Vector3.zero; //Stop gravity from accumulating while the object is grabbed
    }

    public void TogglePower() {
        if (state == IronState.Off) {
            state = IronState.On;
            ironSpriteRenderer.sprite = spriteIronStandingOn;
            AudioManager.instance.PlaySound(Sounds.IronIsOn);

        }
        else if(state == IronState.On) {
            state = IronState.Off;
            ironSpriteRenderer.sprite = spriteIronStandingOff;

        }
    }

    public void PlaceOnIroningBoard() {
        onIroningBoard = true;
        transform.position = new Vector3(transform.position.x, boardHeight);
        //rb.rotation = 90.0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        if (state == IronState.On)
            ironSpriteRenderer.sprite = spriteIronFlatOn;
        else
            ironSpriteRenderer.sprite = spriteIronFlatOff;
    }

    public void TakeOffIroningBoard() {
        onIroningBoard = false;
        //rb.rotation = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (state == IronState.On)
            ironSpriteRenderer.sprite = spriteIronStandingOn;
        else
            ironSpriteRenderer.sprite = spriteIronStandingOff;
    }

    private void FixedUpdate() {
        if(state == IronState.On && onIroningBoard && laundryIroningBoard.garmentOnBoard != null) {
            steamState = laundryIroningBoard.Press(transform.position.x);
        }
        else {
            steamState = SteamState.Off;
        }

        if (steamState == SteamState.Steam && !steam.isEmitting) {
            steam.Play(true);
            smoke.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (steamState == SteamState.Burn && !smoke.isEmitting) {
            smoke.Play(true);
            steam.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (steamState == SteamState.Off && (steam.isEmitting || smoke.isEmitting)) {
            steam.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            smoke.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}

public enum IronState {
    Off,
    On
}

public enum SteamState {
    Off,
    Steam,
    Burn
}