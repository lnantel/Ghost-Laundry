using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    float xInput;
    float yInput;
    float accelerationFactor;
    public Vector2 move;

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
        move = new Vector2(xInput, yInput);
        if (move.magnitude >= 1.0f) move = move.normalized; 
    }
}
