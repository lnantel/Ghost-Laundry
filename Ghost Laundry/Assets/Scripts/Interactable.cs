using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool Locked { get;  protected set; }
    protected GameObject popUpPrefab;
    protected GameObject popUpInstance;
    protected Animator popUpAnimator;
    protected bool popUpVisible;

    protected virtual void Start() {
        //Instantiate pop-up
        popUpPrefab = (GameObject)Resources.Load("InteractablePopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position, transform.rotation, transform);
        popUpAnimator = popUpInstance.GetComponentInChildren<Animator>();
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

    public void OnInteract() {
        if (!Locked) {
            Interaction();
        }
        else {
            Debug.Log("Interactable locked");
        }
    }

    protected abstract void Interaction();

    public virtual void Lock() {
        HidePopUp();
        Locked = true;
    }

    public virtual void Unlock() {
        Locked = false;
    }

    protected void ShowPopUp(int instanceID) {
        if(!Locked && popUpInstance != null) {
            if (instanceID == GetInstanceID() && !Locked) {
                popUpInstance.SetActive(true);
            }
            else {
                //popUpInstance.SetActive(false);
                HidePopUp();
            }
        }
    }

    protected void HidePopUp() {
        if(popUpInstance != null && popUpAnimator != null) {
            popUpAnimator.SetTrigger("HidePopUp");
        }
    }
}
