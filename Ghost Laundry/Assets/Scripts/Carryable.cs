using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    protected GameObject popUpPrefab;
    protected GameObject popUpInstance;
    protected bool popUpVisible;

    protected virtual void Start() {
        //Instantiate pop-up
        popUpPrefab = (GameObject)Resources.Load("CarryablePopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position + Vector3.up * 0.3f, transform.rotation, transform);
        popUpInstance.SetActive(false);
    }

    protected virtual void OnEnable() {
        CarryableDetector.NearestCarryable += ShowPopUp;
        CarryableDetector.NoCarryablesInRange += HidePopUp;
    }

    protected virtual void OnDisable() {
        CarryableDetector.NearestCarryable -= ShowPopUp;
        CarryableDetector.NoCarryablesInRange -= HidePopUp;
    }

    protected void ShowPopUp(int instanceID) {
        if (instanceID == gameObject.GetInstanceID()) {
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
