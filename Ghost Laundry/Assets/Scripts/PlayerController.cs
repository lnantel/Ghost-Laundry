using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputManager input;

    Rigidbody2D rb;

    public float speed;
    public float rotationSpeed;
    public AnimationCurve accelerationCurve;
    public float accelerationTime;

    float accelerationFactor;
    Vector2 moveDir;


    void Start()
    {
        input = PlayerInputManager.instance;
        rb = GetComponent<Rigidbody2D>();
        moveDir = new Vector2(1.0f, 0.0f);
    }

    void Update()
    {
        Move();
    }

    private void Move() {

        if (input.move.magnitude != 0.0f) {
            //Increase acceleration factor
            accelerationFactor = Mathf.Min(accelerationFactor + Time.deltaTime / accelerationTime, input.move.magnitude);

            Debug.DrawRay(transform.position, input.move, Color.green, Time.deltaTime);
            Debug.DrawRay(transform.position, moveDir, Color.red, Time.deltaTime);
            Vector2 zero = Vector2.zero;
            moveDir = Vector2.SmoothDamp(moveDir, input.move.normalized, ref zero, 0.025f * accelerationCurve.Evaluate(accelerationFactor));
        }
        else {
            //Decrease acceleration factor
            accelerationFactor = Mathf.Max(accelerationFactor - Time.deltaTime / accelerationTime, 0.0f);
        }
        rb.velocity = moveDir * speed * accelerationCurve.Evaluate(accelerationFactor);
    }
}
