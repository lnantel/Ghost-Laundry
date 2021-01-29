using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    private PlayerController player;
    private List<Interactable> interactablesInRange;

    private void Start() {
        player = GetComponentInParent<PlayerController>();
        interactablesInRange = new List<Interactable>();
    }

    public Interactable GetNearestInteractable() {
        Interactable nearest = null;
        foreach (Interactable interactable in interactablesInRange) {
            if (nearest == null || Vector2.Distance(interactable.transform.position, transform.position) < Vector2.Distance(nearest.transform.position, transform.position)) {
                nearest = interactable;
            }
        }
        return nearest;
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
