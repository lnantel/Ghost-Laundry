using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractableDetector : MonoBehaviour
{
    public static Action<int> NearestInteractable;
    public static Action NoInteractablesInRange;

    private PlayerController player;
    private List<Interactable> interactablesInRange;

    private int lastCount;

    private void Start() {
        player = GetComponentInParent<PlayerController>();
        interactablesInRange = new List<Interactable>();
    }

    public Interactable GetNearestInteractable() {
        if (interactablesInRange.Count > 0)
            return interactablesInRange[0];
        else return null;
    }

    private void Update() {
        if(interactablesInRange.Count > 0) {
            interactablesInRange.Sort((x, y) => Vector2.Distance(x.transform.position, transform.position).CompareTo(Vector2.Distance(y.transform.position, transform.position)));
            if(NearestInteractable != null) NearestInteractable(interactablesInRange[0].GetInstanceID());
        }
        else if(lastCount > 0) {
            if(NoInteractablesInRange != null) NoInteractablesInRange();
        }
        lastCount = interactablesInRange.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            interactablesInRange.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Interactable interactable = collision.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            interactablesInRange.Remove(interactable);
        }
    }
}
