using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryTable : MonoBehaviour
{
    private TableWorkstation table;

    private IEnumerator soundCoroutine;

    private void Start() {
        table = GetComponentInParent<TableWorkstation>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
            LaundryGarment laundryGarment = collision.gameObject.GetComponentInParent<LaundryGarment>();
            if(!laundryGarment.IsHeld) {
                Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0.0f;

                if (laundryGarment.OnFoldingSurface == table.FoldingLocked) {
                    laundryGarment.OnFoldingSurface = !table.FoldingLocked;
                    if(soundCoroutine == null) {
                        soundCoroutine = DropSound(laundryGarment.garment.fabric.dropSound);
                        StartCoroutine(soundCoroutine);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
            LaundryGarment laundryGarment = collision.gameObject.GetComponentInParent<LaundryGarment>();
            if (laundryGarment.IsHeld) {
                laundryGarment.OnFoldingSurface = false;
            }
            else {
                Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
                //Does the laundryGarment's new collider touch the trigger?
                if (laundryGarment.colliders[laundryGarment.garment.currentFoldingStep].bounds.Intersects(GetComponent<Collider2D>().bounds)) {
                    rb.gravityScale = 0.0f;
                }
                else {
                    rb.gravityScale = 1.0f;
                }
            }
        }
    }

    private IEnumerator DropSound(Sounds sound) {
        AudioManager.instance.PlaySound(sound);
        yield return null;
        soundCoroutine = null;
    }
}
