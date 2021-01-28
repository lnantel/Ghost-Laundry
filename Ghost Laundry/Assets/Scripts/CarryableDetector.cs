using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryableDetector : MonoBehaviour
{
    private PlayerController player;
    private List<GameObject> carryablesInRange;

    private void Start() {
        player = GetComponentInParent<PlayerController>();
        carryablesInRange = new List<GameObject>();
    }

    public GameObject GetNearestCarryable() {
        GameObject nearest = null;
        foreach(GameObject carryable in carryablesInRange) {
            if(nearest == null || Vector2.Distance(carryable.transform.position, transform.position) < Vector2.Distance(nearest.transform.position, transform.position)) {
                nearest = carryable;
            }
        }
        return nearest;
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
