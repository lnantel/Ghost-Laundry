using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : LaundryObject
{
    public LaundryIroningBoard laundryIroningBoard;

    public SpriteRenderer ironSpriteRenderer;
    public Sprite spriteIronOn;
    public Sprite spriteIronOff;

    private Rigidbody2D rb;
    private IronState state;
    private bool onIroningBoard;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Drag(Vector2 cursorPosition) {
        rb.MovePosition(cursorPosition);
        rb.velocity = Vector3.zero; //Stop gravity from accumulating while the object is grabbed
    }

    //TODO: Set up a LaundryButton on the Iron to turn it on/off
    public void TogglePower() {
        if (state == IronState.Off) {
            state = IronState.On;
            ironSpriteRenderer.sprite = spriteIronOn;
        }
        else {
            state = IronState.Off;
            ironSpriteRenderer.sprite = spriteIronOff;
        }
    }

    public void PlaceOnIroningBoard() {
        onIroningBoard = true;
        transform.position = new Vector3(transform.position.x, laundryIroningBoard.transform.position.y);
        rb.rotation = 90.0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void TakeOffIroningBoard() {
        onIroningBoard = false;
        rb.rotation = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate() {
        if(onIroningBoard && laundryIroningBoard.garmentOnBoard != null) {
            //Press garment
            if (laundryIroningBoard.Press(transform.position.x)) {
                state = IronState.Steaming;
            }
            else {
                state = IronState.Burning;
            }
        }
    }
}

public enum IronState {
    Off,
    On,
    Steaming,
    Burning
}
