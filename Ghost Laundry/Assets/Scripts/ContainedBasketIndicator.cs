using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainedBasketIndicator : Carryable, ITrackable
{
    private WorkStation workStation;
    public int basketSlotIndex;

    public SpriteRenderer tagSpriteRenderer;
    public SpriteRenderer basketSprite;

    public Sprite[] tagSprites;

    public Collider2D InputCollider;

    private GameObject laundromatBasketPrefab;

    private bool isBasketPile;

    protected override void Start()
    {
        base.Start();
        workStation = GetComponentInParent<WorkStation>();
        //isBasketPile = workStation is TableWorkstation;
        isBasketPile = true;
        if (basketSlotIndex >= workStation.basketSlots.Length) gameObject.SetActive(false);
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");
    }

    protected override void OnEnable() {
        base.OnEnable();
        WorkStation.BasketSlotsChanged += BasketUpdate;
        LaundryBasket.TagChanged += BasketUpdate;
    }

    protected override void OnDisable() {
        base.OnDisable();
        WorkStation.BasketSlotsChanged -= BasketUpdate;
        LaundryBasket.TagChanged -= BasketUpdate;
    }

    public override GameObject GetCarryableObject() {
        Basket basket = workStation.OutputBasket(basketSlotIndex);
        if(basket != null) {
            GameObject basketObject = Instantiate(laundromatBasketPrefab, transform.position, transform.rotation);
            basketObject.GetComponent<LaundromatBasket>().basket = basket;
            return basketObject;
        }
        return null;
    }

    void BasketUpdate()
    {
        LaundryBasket laundryBasket = workStation.basketSlots[basketSlotIndex].laundryBasket;
        if(laundryBasket == null) {
            tagSpriteRenderer.enabled = false;
            basketSprite.enabled = false;
            ShadowRenderer.enabled = false;
            if(InputCollider != null) InputCollider.enabled = true;
        }
        else {
            tagSpriteRenderer.enabled = true;
            basketSprite.enabled = true;
            ShadowRenderer.enabled = true;
            if (InputCollider != null) InputCollider.enabled = true;
            if (laundryBasket.basket != null)
                tagSpriteRenderer.sprite = tagSprites[laundryBasket.basket.tag];
        }
    }

    public bool ContainsTrackedGarment() {
        if(workStation.basketSlots[basketSlotIndex].laundryBasket != null) {
            return workStation.basketSlots[basketSlotIndex].laundryBasket.ContainsTrackedGarment();
        }
        return false;
    }

    public bool ReceiveBasket(Basket basket) {
        if (workStation.Locked || workStation.basketSlots[basketSlotIndex].Locked) return false;
        return workStation.InputBasket(basket, basketSlotIndex);
    }

    protected override void ShowPopUp(int instanceID) {
        bool locked = workStation.Locked || workStation.basketSlots[basketSlotIndex].Locked;
        if (!locked && !isBasketPile && basketSprite.enabled) {
            base.ShowPopUp(instanceID);
        }
        else if (isBasketPile && basketSprite.enabled) {
            if (popUpInstance != null && workStation.basketSlots[basketSlotIndex].laundryBasket != null) {
                if (!locked && instanceID == gameObject.GetInstanceID() && ((PlayerStateManager.instance.Carrying && workStation.basketSlots[basketSlotIndex].laundryBasket.basket.contents.Count == 0) || (!PlayerStateManager.instance.Carrying && workStation.basketSlots[basketSlotIndex].laundryBasket.basket.contents.Count > 0))) {
                    popUpInstance.SetActive(true);
                    OutlineRenderer.enabled = true;
                }
                else {
                    popUpInstance.SetActive(false);
                    OutlineRenderer.enabled = false;
                }
            }
        }
        else{
            if (popUpInstance != null) {
                if (!locked && instanceID == gameObject.GetInstanceID() && PlayerStateManager.instance.Carrying) {
                    popUpInstance.SetActive(true);
                    OutlineRenderer.enabled = true;
                }
                else {
                    popUpInstance.SetActive(false);
                    OutlineRenderer.enabled = false;
                }
            }
        }
    }
}
