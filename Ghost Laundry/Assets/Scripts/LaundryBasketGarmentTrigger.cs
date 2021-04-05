using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundryBasketGarmentTrigger : MonoBehaviour
{
    private LaundryBasket laundryBasket;
    public Animator animator;

    private void Start() {
        laundryBasket = GetComponentInParent<LaundryBasket>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!laundryBasket.basketView.activeSelf) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("LaundryGarment")) {
                LaundryGarment laundryGarment = collision.GetComponentInParent<LaundryGarment>();
                if (laundryGarment != null && !laundryGarment.IsHeld && laundryGarment.GetComponent<Rigidbody2D>().gravityScale != 0.0f) {
                    if (laundryBasket.basket.AddGarment(laundryGarment.garment)) {
                        AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                        Destroy(laundryGarment.gameObject);
                        animator.SetTrigger("BasketInput");
                    }
                    else {
                        animator.SetTrigger("BasketFull");
                    }
                }
            }
        }
    }
}
