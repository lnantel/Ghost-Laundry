using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool locked;
    protected GameObject popUpPrefab;
    protected GameObject popUpInstance;
    protected bool popUpVisible;

    protected virtual void Start() {
        //Instantiate pop-up
        popUpPrefab = (GameObject)Resources.Load("InteractablePopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position + Vector3.up, transform.rotation, transform);
        popUpInstance.SetActive(false);
    }

    protected virtual void OnEnable() {
        InteractableDetector.NearestInteractable += ShowPopUp;
        InteractableDetector.NoInteractablesInRange += HidePopUp;
    }

    protected virtual void OnDisable() {
        InteractableDetector.NearestInteractable -= ShowPopUp;
        InteractableDetector.NoInteractablesInRange -= HidePopUp;
    }

    public virtual void Interact() {
        if (!locked) {
            Debug.Log("Interaction");
        }
    }

    protected void Lock() {
        locked = true;
    }

    protected void Unlock() {
        locked = false;
    }

    protected void ShowPopUp(int instanceID) {
        if (instanceID == GetInstanceID() && !locked) {
            popUpInstance.SetActive(true);
        }
        else {
            popUpInstance.SetActive(false);
        }
    }

    protected void HidePopUp() {
        popUpInstance.SetActive(false);
    }
}
