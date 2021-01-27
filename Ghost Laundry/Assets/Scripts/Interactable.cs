using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool locked;

    // Start is called before the first frame update
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
}
