using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryTaskController : MonoBehaviour
{
    public float CursorSensitivity;
    public float CursorSpeed;
    public Rigidbody2D cursor;
    private float snappiness = 50.0f;

    private bool interactInput;
    private bool interactInputHeld;
    private bool inspectInput;
    private float moveXInput;
    private float moveYInput;
    private bool backInput;

    private bool objectGrabbed;

    private IEnumerator DelayGrabCoroutine;
    private float grabDelay = 0.15f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Inputs
        interactInput = Input.GetButtonDown("TaskInteract");
        interactInputHeld = Input.GetButton("TaskInteract");
        inspectInput = Input.GetButtonDown("Inspect");
        backInput = Input.GetButtonDown("Back");

        moveXInput = Mathf.Lerp(moveXInput, Input.GetAxis("TaskHorizontal"), snappiness * Time.deltaTime);
        moveYInput = Mathf.Lerp(moveYInput, Input.GetAxis("TaskVertical"), snappiness * Time.deltaTime);
        Vector2 moveInput = new Vector2(moveXInput, moveYInput);

        //Cursor Movement
        cursor.velocity = moveInput * CursorSensitivity * CursorSpeed;

        //Interact vs Grab
        if (interactInput) {
            if (DelayGrabCoroutine != null) StopCoroutine(DelayGrabCoroutine);
            DelayGrabCoroutine = DelayGrab();
            StartCoroutine(DelayGrabCoroutine);
        }

        //Grab & Release
        if(objectGrabbed && !interactInputHeld) {
            Release();
        }
    }

    private IEnumerator DelayGrab() {
        float timer = 0.0f;
        while(timer < grabDelay) {
            yield return new WaitForSeconds(0.0f);
            timer += Time.deltaTime;
            if (!interactInputHeld) break;
        }
        if (interactInputHeld) Grab();
        else Interact();
        DelayGrabCoroutine = null;
    }

    private void Interact() {
        Debug.Log("Interact");
        //Detect an Interactable
        //Trigger its interaction action
    }

    private void Grab() {
        Debug.Log("Grab");
        //Detect a Grabbable
        //Grab it
    }

    private void Release() {
        Debug.Log("Release");
        //Release a grabbed object
    }
}
