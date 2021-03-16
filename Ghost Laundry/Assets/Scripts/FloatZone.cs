using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatZone : MonoBehaviour
{
    public float FloatSpeed;

    private Collider2D col;

    private void Start() {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        LaundryObject laundryObject = collision.GetComponentInParent<LaundryObject>();
        if(laundryObject != null) {
            if(LaundryTaskController.instance.grabbedObject == null || LaundryTaskController.instance.grabbedObject.GetInstanceID() != laundryObject.GetInstanceID()) {
                if (col.bounds.Contains(laundryObject.transform.position)) {
                    collision.attachedRigidbody.gravityScale = -FloatSpeed;
                }
                else {
                    collision.attachedRigidbody.gravityScale = 0.0f;
                }
            }
        }
    }
}
