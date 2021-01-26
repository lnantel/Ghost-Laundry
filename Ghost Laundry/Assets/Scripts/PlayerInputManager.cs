using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    float xInput;
    float yInput;
    float accelerationFactor;
    public Vector2 Move { private set; get; }

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
    }
}
