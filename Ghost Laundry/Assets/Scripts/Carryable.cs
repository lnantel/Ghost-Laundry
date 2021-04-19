using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    public Material MAT_Outline;
    public SpriteRenderer spriteRenderer;
    protected Material MAT_Default;
    protected GameObject popUpPrefab;
    protected GameObject popUpInstance;
    protected bool popUpVisible;

    protected virtual void Start() {
        //Instantiate pop-up
        popUpPrefab = (GameObject)Resources.Load("CarryablePopUp");
        popUpInstance = Instantiate(popUpPrefab, transform.position + Vector3.up * 0.3f, transform.rotation, transform);
        popUpInstance.SetActive(false);
        MAT_Default = spriteRenderer.material;
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
        if(popUpInstance != null) {
            if (instanceID == gameObject.GetInstanceID() && !PlayerStateManager.instance.Carrying) {
                popUpInstance.SetActive(true);
                spriteRenderer.material.SetFloat("_OutlineThickness", 35.0f);
            }
            else {
                popUpInstance.SetActive(false);
                spriteRenderer.material.SetFloat("_OutlineThickness", 0.0f);
            }
        }
    }

    protected void HidePopUp() {
        if(popUpInstance != null) {
            popUpInstance.GetComponentInChildren<Animator>().SetTrigger("HidePopUp");
            spriteRenderer.material.SetFloat("_OutlineThickness", 0.0f);
        }
    }
}
