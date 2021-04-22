using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    public SpriteRenderer OutlineRenderer;
    public SpriteRenderer ShadowRenderer;
    protected GameObject popUpPrefab;
    protected GameObject popUpInstance;
    protected bool popUpVisible;

    protected virtual void Start() {
        //Instantiate pop-up
        popUpPrefab = (GameObject)Resources.Load("CarryablePopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position + Vector3.up * 0.3f, transform.rotation, transform);
        popUpInstance.SetActive(false);
        OutlineRenderer.enabled = false;
        ShadowRenderer.enabled = true;
    }

    protected virtual void OnEnable() {
        CarryableDetector.NearestCarryable += ShowPopUp;
        CarryableDetector.NoCarryablesInRange += HidePopUp;
    }

    protected virtual void OnDisable() {
        CarryableDetector.NearestCarryable -= ShowPopUp;
        CarryableDetector.NoCarryablesInRange -= HidePopUp;
    }

    protected virtual void ShowPopUp(int instanceID) {
        if(popUpInstance != null) {
            if (instanceID == gameObject.GetInstanceID() && !PlayerStateManager.instance.Carrying) {
                popUpInstance.SetActive(true);
                OutlineRenderer.enabled = true;
            }
            else {
                popUpInstance.SetActive(false);
                OutlineRenderer.enabled = false;
            }
        }
    }

    protected virtual void HidePopUp() {
        if(popUpInstance != null) {
            popUpInstance.GetComponentInChildren<Animator>().SetTrigger("HidePopUp");
            OutlineRenderer.enabled = false;
        }
    }

    public virtual GameObject GetCarryableObject() {
        return gameObject;
    }
}
