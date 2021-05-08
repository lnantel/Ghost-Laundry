using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingStar : MonoBehaviour
{
    public static Action<bool> ReachedDestination;

    public float initialSpeed;
    public float force;
    public RectTransform reputationBar;
    public AnimationCurve offsetCurve;
    public float minDeviance;
    public float maxDeviance;

    public TrailRenderer PositiveTrail;
    public TrailRenderer NegativeTrail;

    public Color PositiveColor;
    public Color NegativeColor;

    private Vector2 maxOffset;
    private Transform target;
    private Vector3 initialPos { get => GetInitialPos(); }
    private Vector3 startingPos;
    private float totalDistance;
    private Vector3 destination { get => GetDestination(); }

    private Vector2 velocity;
    private Vector2 dir;
    private Vector2 offset;
    private float currentDistance;
    private Vector2 currentPos;

    private bool sign;

    private void OnEnable() {
        //if(Camera.main != null)
        //    SetStartingValues();
    }

    private Vector3 GetInitialPos() {
        if (sign) return target.position;
        else return Camera.main.ScreenToWorldPoint(reputationBar.transform.position);
    }

    private Vector3 GetDestination() {
        if (sign) return Camera.main.ScreenToWorldPoint(reputationBar.transform.position); 
        else return target.position;
    }

    public void SetSignAndTarget(bool value, Transform target) {
        sign = value;

        this.target = target;
        if (!sign) {
            if (ReachedDestination != null) ReachedDestination(sign);
        }
        transform.position = initialPos;
        GetComponent<SpriteRenderer>().color = value ? PositiveColor : NegativeColor;
        PositiveTrail.emitting = value;
        NegativeTrail.emitting = !value;
        SetStartingValues();
    }

    private void SetStartingValues() {
        startingPos = initialPos;
        currentPos = startingPos;
        velocity = Vector2.zero;
        totalDistance = Vector2.Distance(startingPos, destination);

        dir = (destination - transform.position).normalized;
        float randomSign = UnityEngine.Random.Range(0, 2);
        randomSign = randomSign == 0 ? -1.0f : 1.0f;
        maxOffset = Vector2.Perpendicular(dir) * UnityEngine.Random.Range(minDeviance, maxDeviance) * randomSign;
    }

    void FixedUpdate() {
        //Vector3 relativeDestination = Camera.main.ScreenToWorldPoint(reputationBar.transform.position);

        Vector2 acceleration = dir * force * Time.fixedDeltaTime;
        velocity += acceleration;
        currentPos = Vector2.MoveTowards(currentPos, destination, velocity.magnitude);
        currentDistance = Vector2.Distance(currentPos, destination);
        float ratio = (totalDistance - currentDistance) / totalDistance;
        offset = maxOffset * offsetCurve.Evaluate(ratio);

        transform.position = new Vector3(startingPos.x, startingPos.y, 0.0f) + (destination - new Vector3(startingPos.x, startingPos.y, 0.0f)) * ratio + new Vector3(offset.x, offset.y, 0.0f);

        if (currentDistance < 0.2f) {
            if (sign && ReachedDestination != null) ReachedDestination(sign);
            PositiveTrail.Clear();
            NegativeTrail.Clear();
            gameObject.SetActive(false);
        }
    }

    private Vector2 Rotate(Vector2 vector, float angle) {
        float x = Mathf.Cos(vector.x * angle) - Mathf.Sin(vector.y * angle);
        float y = Mathf.Sin(vector.x * angle) + Mathf.Cos(vector.y * angle);
        return new Vector2(x, y);
    }
}
