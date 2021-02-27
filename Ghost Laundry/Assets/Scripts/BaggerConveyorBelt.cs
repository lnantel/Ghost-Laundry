using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaggerConveyorBelt : MonoBehaviour
{
    public float speed;

    private void OnTriggerStay2D(Collider2D collision) {
        LaundromatBag bag = collision.GetComponent<LaundromatBag>();
        if(bag != null) {
            bag.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, -speed, 0.0f);
        }
    }
}
