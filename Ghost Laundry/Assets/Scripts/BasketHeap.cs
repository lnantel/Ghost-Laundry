using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketHeap : MonoBehaviour
{
    public float minY;
    public float maxY;

    private Basket basket;

    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        WorkStation.BasketSlotsChanged += BasketUpdate;
        BasketUpdate();
    }

    private void OnDisable() {
        WorkStation.BasketSlotsChanged -= BasketUpdate;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Find a basket to track its capacity
        LaundromatBasket laundromatBasket = GetComponentInParent<LaundromatBasket>();
        if(laundromatBasket != null) {
            basket = laundromatBasket.basket;
        }

        LaundryBasket laundryBasket = GetComponentInParent<LaundryBasket>();
        if(laundryBasket != null){
            basket = laundryBasket.basket;
        }
    }

    private void BasketUpdate() {
        ContainedBasketIndicator basketIndicator = GetComponentInParent<ContainedBasketIndicator>();
        if (basketIndicator != null) {
            WorkStation workstation = basketIndicator.GetComponentInParent<WorkStation>();
            LaundryBasket laundryB = workstation.basketSlots[basketIndicator.basketSlotIndex].laundryBasket;
            if (laundryB != null) {
                basket = laundryB.basket;
            }
            else if (laundryB == null)
                basket = null;
        }

        LaundryBasket laundryBasket = GetComponentInParent<LaundryBasket>();
        if (laundryBasket != null) {
            basket = laundryBasket.basket;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (basket != null && basket.currentLoad > 0 && basket.capacity > 1) {
            spriteRenderer.enabled = true;
            transform.localPosition = new Vector3(transform.localPosition.x, minY + (maxY - minY) * ((float)(basket.currentLoad - 1) / (basket.capacity - 1)), transform.localPosition.z);
        }
        else {
            spriteRenderer.enabled = false;
        }
    }
}
