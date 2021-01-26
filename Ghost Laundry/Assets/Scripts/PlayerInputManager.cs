using System.Collections;
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

    void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        Move = new Vector2(xInput, yInput);
        if (Move.magnitude >= 1.0f) Move = Move.normalized;

        if(!dash) dash = Input.GetButton("Dash");
    }

    public bool GetDashInput() {
        bool value = dash;
        dash = false;
        return value;
    }
}
