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
        if (interactablesInRange.Count > 0) {
            for(int i = 0; i < interactablesInRange.Count; i++) {
                if (!interactablesInRange[i].Locked) return interactablesInRange[i];
            }
        }
        return null;
    }

    private void Update() {
        if(interactablesInRange.Count > 0) {
            interactablesInRange.Sort((x, y) => Vector2.Distance(x.transform.position, transform.position).CompareTo(Vector2.Distance(y.transform.position, transform.position)));
            for(int i = 0; i < interactablesInRange.Count; i++) {
                if (!interactablesInRange[i].Locked) {
                    if (NearestInteractable != null) NearestInteractable(interactablesInRange[i].GetInstanceID());
                    break;
                }
            }
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
