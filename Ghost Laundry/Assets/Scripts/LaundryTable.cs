using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryTable : MonoBehaviour
{
    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        //If released within table bounds, turn off gravity and set velocity to 0
        if (GetComponent<Collider2D>().bounds.Contains(laundryGarment.transform.position)) {
            Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //LaundryGarment obj = collision.GetComponent<LaundryGarment>();
        if(collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
            LaundryGarment laundryGarment = collision.gameObject.GetComponentInParent<LaundryGarment>();
            laundryGarment.OnFoldingSurface = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
            LaundryGarment laundryGarment = collision.gameObject.GetComponentInParent<LaundryGarment>();
            laundryGarment.OnFoldingSurface = false;
        }
    }
}
