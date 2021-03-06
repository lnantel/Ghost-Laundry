﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    //Movement
    float xInput;
    float yInput;
    public Vector2 Move { private set; get; }

    //Dash
    private bool dash;

    //PickUp or Drop
    private bool pickUp;

    //Interact
    private bool interact;

    void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (TimeManager.instance.TimeIsPassing) {
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");

            dash = Input.GetButtonDown("Dash");

            pickUp = Input.GetButtonDown("PickUp");

            interact = Input.GetButtonDown("Interact");
        }
        else {
            xInput = 0;
            yInput = 0;
            dash = false;
            pickUp = false;
            interact = false;
        }
        Move = new Vector2(xInput, yInput);
        if (Move.magnitude >= 1.0f) Move = Move.normalized;
    }

    public bool GetDashInput() {
        bool value = dash;
        dash = false;
        return value;
    }

    public bool GetPickUpInput() {
        bool value = pickUp;
        pickUp = false;
        return value;
    }

    public bool GetInteractInput() {
        bool value = interact;
        interact = false;
        return value;
    }
}
