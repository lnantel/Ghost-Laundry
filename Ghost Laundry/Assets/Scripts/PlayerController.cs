﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputManager input;
    PlayerStateManager state;

    Rigidbody2D rb;

    //Movement
    public float m_Speed;
    public float m_RotationDampeningFactor;
    public AnimationCurve m_AccelerationCurve;
    public float m_AccelerationTime;

    private float accelerationFactor;
    private Vector2 moveDir;
    private bool facingRight;

    //TODO: this will be handled in the animator or an animation script at a later point
    private SpriteRenderer sprite;

    //Dash
    public float m_DashDistance;
    public float m_DashDuration;
    public AnimationCurve m_DashCurve;
    public float m_DashCooldown;

    //Pick up / Drop
    private CarryableDetector carryableDetector;
    private GameObject carriedObject;
    public Transform carriedPos;

    void Start()
    {
        input = PlayerInputManager.instance;
        state = PlayerStateManager.instance;

        rb = GetComponent<Rigidbody2D>();
        carryableDetector = GetComponentInChildren<CarryableDetector>();

        moveDir = new Vector2(1.0f, 0.0f);

        //TODO: this will be handled in the animator or an animation script at a later point
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (state.CanMove())
            Move();

        //TODO: this will be handled in the animator or an animation script at a later point
        sprite.flipX = facingRight;

        if (input.GetDashInput() && state.CanDash())
            StartCoroutine(Dash());

        if (input.GetPickUpInput()) {
            if (state.Carrying)
                DropCarriedObject();
            else
                PickUp();
        }

        if (state.Carrying) {
            if (carriedPos.localPosition.x > 0.0f != facingRight) carriedPos.localPosition = new Vector3(-carriedPos.localPosition.x, carriedPos.localPosition.y, carriedPos.localPosition.z);
            carriedObject.transform.position = carriedPos.position;
        }
    }

    private void Move() {

        if (input.Move.magnitude != 0.0f) {
            //Increase acceleration factor
            accelerationFactor = Mathf.Min(accelerationFactor + Time.deltaTime / m_AccelerationTime, input.Move.magnitude);

            Debug.DrawRay(transform.position, input.Move, Color.green, Time.deltaTime);
            Debug.DrawRay(transform.position, moveDir, Color.red, Time.deltaTime);
            Vector2 zero = Vector2.zero;
            moveDir = Vector2.SmoothDamp(moveDir, input.Move.normalized, ref zero, m_RotationDampeningFactor * m_AccelerationCurve.Evaluate(accelerationFactor));
        }
        else {
            //Decrease acceleration factor
            accelerationFactor = Mathf.Max(accelerationFactor - Time.deltaTime / m_AccelerationTime, 0.0f);
        }

        //Facing
        if (moveDir.x > 0.01f) facingRight = true;
        else if (moveDir.x < -0.01f) facingRight = false;

        rb.velocity = moveDir * m_Speed * m_AccelerationCurve.Evaluate(accelerationFactor);
    }

    private IEnumerator Dash() {
        state.StartDash();
        if(state.Carrying)
            DropCarriedObject();

        Vector2 dashStartPos = rb.position;
        Vector2 dashEndPos = rb.position + moveDir.normalized * m_DashDistance;
        Vector2 startVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        float timer = 0.0f;
        while(timer < m_DashDuration) {
            rb.position = Vector2.Lerp(dashStartPos, dashEndPos, m_DashCurve.Evaluate(timer / m_DashDuration));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.0f);
        }

        state.EndDash();
        StartCoroutine(state.DashCooldown(m_DashCooldown));

        rb.position = dashEndPos;
        rb.velocity = startVelocity;
    }

    private void PickUp() {
        GameObject targetedCarryable = carryableDetector.GetNearestCarryable();
        if (targetedCarryable != null) {
            state.StartCarry();
            carriedObject = targetedCarryable;
            carriedObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void DropCarriedObject() {
        state.EndCarry();
        carriedObject.GetComponent<Collider2D>().enabled = true;
        carriedObject = null;
    }
}
