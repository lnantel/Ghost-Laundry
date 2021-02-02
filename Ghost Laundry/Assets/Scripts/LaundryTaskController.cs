using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryTaskController : MonoBehaviour
{
    public float CursorSensitivity;
    public float CursorSpeed;
    public Transform cursor;
    private float snappiness = 50.0f;
    private Vector2 worldBottomLeft;
    private Vector2 worldTopRight;

    private bool interactInput;
    private bool interactInputHeld;
    private bool inspectInput;
    private float moveXInput;
    private float moveYInput;
    private bool backInput;

    private LaundryObject grabbedObject;

    private IEnumerator DelayGrabCoroutine;
    private float grabDelay = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        worldBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));
        worldTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, -Camera.main.transform.position.z));
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
        Vector2 displacement = moveInput * CursorSensitivity * CursorSpeed;

        //Cursor movement
        if(displacement.magnitude > 0.0006f) //accounts for floating point uncertainty; prevents drift
            cursor.position += new Vector3(displacement.x, displacement.y, 0.0f);

        //Cursor clamping
        float clampedX = Mathf.Clamp(cursor.position.x, worldBottomLeft.x, worldTopRight.x);
        float clampedY = Mathf.Clamp(cursor.position.y, worldBottomLeft.y, worldTopRight.y);
        cursor.position = new Vector2(clampedX, clampedY);

        //Interact vs Grab
        if (interactInput) {
            if (DelayGrabCoroutine != null) StopCoroutine(DelayGrabCoroutine);
            DelayGrabCoroutine = DelayGrab();
            StartCoroutine(DelayGrabCoroutine);
        }

        //Drag grabbed object
        if (grabbedObject != null)
            grabbedObject.Drag(cursor.position);

        //Grab & Release
        if(grabbedObject != null && !interactInputHeld) {
            Release();
        }
    }

    private IEnumerator DelayGrab() {
        //Detects the targeted LaundryObject
        LaundryObject target = null;
        int layerMask = LayerMask.GetMask("LaundryObject");
        Collider2D col = Physics2D.OverlapCircle(cursor.position, 0.1f, layerMask);
        if (col != null) {
            target = col.GetComponent<LaundryObject>();
        }
        if(target != null) {
            //If the cursor moves more than 0.1f units while interact is held, or if it is held for longer than grabDelay,
            //then the input is interpreted as a grab rather than an interact.
            //Otherwise, on release, the input is interpreted as an interact.
            float timer = 0.0f;
            Vector2 initialCursorPosition = cursor.position;
            float cursorDelta;
            while (timer < grabDelay) {
                yield return new WaitForSeconds(0.0f);
                timer += Time.deltaTime;
                cursorDelta = (initialCursorPosition - new Vector2(cursor.position.x, cursor.position.y)).magnitude;
                if (!interactInputHeld || cursorDelta > 0.1f) break;
            }
            if (interactInputHeld) Grab(target);
            else Interact(target);
        }
        DelayGrabCoroutine = null;
    }

    private void Interact(LaundryObject obj) {
        //Trigger its interaction action
        obj.OnInteract();
    }

    private void Grab(LaundryObject obj) {
        //Grab it
        grabbedObject = obj;
        obj.OnGrab();
    }

    private void Release() {
        grabbedObject.OnRelease();
        grabbedObject = null;
        //Release a grabbed object
    }
}
