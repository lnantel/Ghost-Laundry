using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarryableDetector : MonoBehaviour
{
    public static Action<int> NearestCarryable;
    public static Action NoCarryablesInRange;

    public float OffsetX;

    private List<GameObject> carryablesInRange;

    private int lastCount;

    private void Start() {
        carryablesInRange = new List<GameObject>();
    }

    public GameObject GetNearestCarryable() {
        if (carryablesInRange.Count > 0)
            return carryablesInRange[0];
        else return null;
    }

    public ContainedBasketIndicator GetNearestBasketIndicator() {
        for (int i = 0; i < carryablesInRange.Count; i++) {
            if(carryablesInRange[i] != null) {
                ContainedBasketIndicator indicator = carryablesInRange[i].GetComponent<ContainedBasketIndicator>();
                if (indicator != null) return indicator;
            }
        }
        return null;
    }

    private void Update() {
        float facingOffset = PlayerController.instance.facingRight ? OffsetX : -OffsetX;
        transform.localPosition = new Vector3(facingOffset, transform.localPosition.y, transform.localPosition.z);

        if (carryablesInRange.Count > 0) {
            carryablesInRange.Sort((x, y) => Vector2.Distance(x.transform.position, transform.position).CompareTo(Vector2.Distance(y.transform.position, transform.position)));
            if (NearestCarryable != null) NearestCarryable(carryablesInRange[0].GetInstanceID());
        }
        else if (lastCount > 0) {
            if (NoCarryablesInRange != null) NoCarryablesInRange();
        }
        lastCount = carryablesInRange.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Carryable")) {
            carryablesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Carryable")) {
            carryablesInRange.Remove(collision.gameObject);
        }
    }
}
