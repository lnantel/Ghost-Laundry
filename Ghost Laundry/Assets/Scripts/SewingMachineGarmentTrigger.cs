using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewingMachineGarmentTrigger : MonoBehaviour
{
    private SewingMachine sewingMachine;

    private void Start() {
        sewingMachine = GetComponentInParent<SewingMachine>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
            LaundryGarment laundryGarment = collision.GetComponentInParent<LaundryGarment>();
            if (laundryGarment != null && !laundryGarment.IsHeld && laundryGarment.GetComponent<Rigidbody2D>().gravityScale != 0.0f) {
                if (sewingMachine.PlaceGarment(laundryGarment.garment)) {
                    AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                    laundryGarment.ReturnToPool();
                }
            }
        }
    }
}
