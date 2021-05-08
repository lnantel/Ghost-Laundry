using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CarriedBasketView : MonoBehaviour
{
    public Basket basket;

    public SpriteRenderer Background;
    public SpriteRenderer BasketSprite;

    public GameObject GarmentContainer;

    public RectTransform UIPosition;

    private bool visible;

    private void Start() {
        HideBasket();
    }

    private void Update() {
        //Position Update
        if(Camera.main != null) {
            transform.position = Camera.main.ScreenToWorldPoint(UIPosition.transform.position);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        }

        //Basket Update
        if (PlayerStateManager.instance != null && PlayerStateManager.instance.Carrying) {
            if (PlayerController.instance.carriedObject != null) {
                LaundromatBasket laundromatBasket = PlayerController.instance.carriedObject.GetComponent<LaundromatBasket>();
                if (laundromatBasket != null)
                    basket = laundromatBasket.basket;
            }
        }
        else
            basket = null;

        if(basket != null) {
            if (!visible) ShowBasket();
        }
        else {
            if(visible) HideBasket();
        }
    }

    private void ShowBasket() {
        visible = true;
        Background.enabled = true;
        BasketSprite.enabled = true;
        UpdateContents();
    }

    private void HideBasket() {
        visible = false;
        Background.enabled = false;
        BasketSprite.enabled = false;
        ClearContents();
    }

    private void ClearContents() {
        for (int i = GarmentContainer.transform.childCount - 1; i >= 0; i--) {
            LaundryGarment laundryGarment = GarmentContainer.transform.GetChild(i).GetComponent<LaundryGarment>();
            laundryGarment.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
            rb.simulated = true;
            laundryGarment.ReturnToPool();
        }
    }

    private void UpdateContents() {
        ClearContents();
        for(int i = 0; i < basket.contents.Count; i++) {
            LaundryGarment laundryGarment = basket.contents[i].CreateLaundryGarment(transform.position + 0.5f * basket.positions[i], transform.rotation, GarmentContainer.transform);
            laundryGarment.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
            Rigidbody2D rb = laundryGarment.GetComponent<Rigidbody2D>();
            rb.simulated = false;
        }
    }
}
