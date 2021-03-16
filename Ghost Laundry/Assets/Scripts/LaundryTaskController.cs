using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LaundryTaskController : MonoBehaviour
{
    public static LaundryTaskController instance;

    public static Action exitedTask;

    public float CursorSpeed;
    public Transform cursor;
    public SpriteRenderer cursorSpriteRenderer;
    public Sprite defaultCursorSprite;
    public Sprite targetCursorSprite;
    public Camera LaundryCamera;
    public GameObject LaundryGarmentPrefab;

    [HideInInspector]
    public WorkStation activeWorkStation;

    private float snappiness = 50.0f;
    private Vector2 worldBottomLeft;
    private Vector2 worldTopRight;

    private bool interactInput;
    private bool interactInputHeld;
    private bool inspectInputDown;
    private bool inspectInputHeld;
    private float moveXInput;
    private float moveYInput;
    private bool backInput;

    private LaundryObject target;
    public LaundryObject grabbedObject;
    public float grabDelay;

    private IEnumerator DelayGrabCoroutine;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else {
            instance = this;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        StartCoroutine(Initialize());
        LaundryBasket.TakeOutGarment += GrabGarmentFromContainer;
        WashingMachineDoor.GarmentGrabbed += GrabGarmentFromContainer;
        DryerDoor.GarmentGrabbed += GrabGarmentFromContainer;
        LaundryIroningBoard.GarmentGrabbed += GrabGarmentFromContainer;
        ClotheslinePeg.GrabGarment += GrabGarmentFromContainer;
        LaundrySewingMachine.GarmentGrabbed += GrabGarmentFromContainer;
    }

    private void OnDisable() {
        LaundryBasket.TakeOutGarment -= GrabGarmentFromContainer;
        WashingMachineDoor.GarmentGrabbed -= GrabGarmentFromContainer;
        DryerDoor.GarmentGrabbed -= GrabGarmentFromContainer;
        LaundryIroningBoard.GarmentGrabbed -= GrabGarmentFromContainer;
        ClotheslinePeg.GrabGarment -= GrabGarmentFromContainer;
        LaundrySewingMachine.GarmentGrabbed -= GrabGarmentFromContainer;
    }

    private IEnumerator Initialize() {
        yield return new WaitForEndOfFrame();
        worldBottomLeft = LaundryCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, -LaundryCamera.transform.position.z));
        worldTopRight = LaundryCamera.ScreenToWorldPoint(new Vector3(LaundryCamera.pixelWidth, LaundryCamera.pixelHeight, -LaundryCamera.transform.position.z));
        cursor.position = LaundryCamera.ScreenToWorldPoint(new Vector3(LaundryCamera.pixelWidth / 2.0f, LaundryCamera.pixelHeight / 2.0f, -LaundryCamera.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.instance.timeScale != 0 && Time.timeScale != 0) {
            //Inputs
            interactInput = Input.GetButtonDown("TaskInteract");
            interactInputHeld = Input.GetButton("TaskInteract");
            inspectInputDown = Input.GetButtonDown("Inspect");
            inspectInputHeld = Input.GetButton("Inspect");
            backInput = Input.GetButtonDown("Back");

            moveXInput = Mathf.Lerp(moveXInput, Input.GetAxis("TaskHorizontal"), snappiness * Time.deltaTime);
            moveYInput = Mathf.Lerp(moveYInput, Input.GetAxis("TaskVertical"), snappiness * Time.deltaTime);
            Vector2 moveInput = new Vector2(moveXInput, moveYInput);
            Vector2 displacement = moveInput * SettingsManager.instance.MouseSensitivity * CursorSpeed;

            //Cursor movement
            if (displacement.magnitude > 0.0006f) //accounts for floating point uncertainty; prevents drift
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

            //Inspect
            if (inspectInputDown) {
                Inspect();
            }

            //Drag grabbed object
            if (grabbedObject != null)
                grabbedObject.Drag(cursor.position);

            //Grab & Release
            if (grabbedObject != null && !interactInputHeld) {
                Release();
            }

            //Back out
            if (backInput) {
                BackOut();
            }

            //Hover
            target = GetTarget();
            if (target != null) {
                target.OnHover(cursor.position);
                if(interactInputHeld || inspectInputHeld)
                    cursorSpriteRenderer.sprite = defaultCursorSprite;
                else
                    cursorSpriteRenderer.sprite = targetCursorSprite;
            }
            else {
                cursorSpriteRenderer.sprite = defaultCursorSprite;
            }
        }
    }

    private LaundryObject GetTarget() {
        //Attempts to find a LaundryObject or Basket under the cursor
        //Priority is given to LaundryObjects
        int layerMask = LayerMask.GetMask("LaundryGarment");
        Collider2D col = Physics2D.OverlapCircle(cursor.position, 0.1f, layerMask);
        if (col != null) {
            return col.GetComponentInParent<LaundryObject>();
        }
        else {
            layerMask = LayerMask.GetMask("LaundryObject");
            col = Physics2D.OverlapCircle(cursor.position, 0.1f, layerMask);
            if (col != null) {
                return col.GetComponentInParent<LaundryObject>();
            }
            else {
                layerMask = LayerMask.GetMask("Basket");
                col = Physics2D.OverlapCircle(cursor.position, 0.1f, layerMask);
                if (col != null) {
                    return col.GetComponentInParent<LaundryObject>();
                }
            }
        }
        return null;
    }

    private IEnumerator DelayGrab() {
        if(target != null) {
            //If the cursor moves more than 0.1f units while interact is held, or if it is held for longer than grabDelay,
            //then the input is interpreted as a grab rather than an interact.
            //Otherwise, on release, the input is interpreted as an interact.
            float timer = 0.0f;
            Vector2 initialCursorPosition = cursor.position;
            Vector2 initialTargetPosition = target.transform.position;
            float cursorDelta;
            while (true) {
                yield return new WaitForLaundromatSeconds(0.0f);
                timer += TimeManager.instance.deltaTime;
                cursorDelta = (initialCursorPosition - new Vector2(cursor.position.x, cursor.position.y)).magnitude;
                if(target != null) cursorDelta += (initialTargetPosition - new Vector2(target.transform.position.x, target.transform.position.y)).magnitude;
                if (!interactInputHeld || cursorDelta > 0.1f || timer > grabDelay) break;
            }
            if (interactInputHeld) Grab();
            else Interact();
        }
        DelayGrabCoroutine = null;
    }

    private void Inspect() {
        if (target != null) target.OnInspect();
    }

    private void Interact() {
        if(target != null)
            target.OnInteract();
    }

    private void Grab() {
        if(target != null) {
            grabbedObject = target;
            target.OnGrab();
        }
    }

    private void Release() {
        grabbedObject.OnRelease();
        grabbedObject = null;
    }

    private void BackOut() {
        if (grabbedObject != null) Release();
        if (exitedTask != null)
            exitedTask();
        activeWorkStation = null;
        gameObject.SetActive(false);
    }

    private void GrabGarmentFromContainer(Garment garment) {
        LaundryGarment laundryGarment = garment.CreateLaundryGarment(cursor.transform.position, cursor.transform.rotation, activeWorkStation.laundryTaskArea.transform);
        target = laundryGarment;
        Grab();
    }
}
