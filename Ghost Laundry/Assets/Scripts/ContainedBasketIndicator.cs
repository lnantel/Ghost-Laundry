using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainedBasketIndicator : MonoBehaviour, ITrackable
{
    private WorkStation workStation;
    public int basketSlotIndex;

    public SpriteRenderer tagSpriteRenderer;
    public SpriteRenderer basketSprite;

    public Sprite[] tagSprites;

    void Start()
    {
        workStation = GetComponentInParent<WorkStation>();
        if (basketSlotIndex >= workStation.basketSlots.Length) gameObject.SetActive(false);
    }

    private void OnEnable() {
        WorkStation.BasketSlotsChanged += BasketUpdate;
        LaundryBasket.TagChanged += BasketUpdate;
    }

    private void OnDisable() {
        WorkStation.BasketSlotsChanged -= BasketUpdate;
        LaundryBasket.TagChanged -= BasketUpdate;
    }

    void BasketUpdate()
    {
        LaundryBasket laundryBasket = workStation.basketSlots[basketSlotIndex].laundryBasket;
        if(laundryBasket == null) {
            tagSpriteRenderer.enabled = false;
            basketSprite.enabled = false;
        }
        else {
            tagSpriteRenderer.enabled = true;
            basketSprite.enabled = true;
            if(laundryBasket.basket != null)
                tagSpriteRenderer.sprite = tagSprites[laundryBasket.basket.tag];
        }
    }

    public bool ContainsTrackedGarment() {
        if(workStation.basketSlots[basketSlotIndex].laundryBasket != null) {
            return workStation.basketSlots[basketSlotIndex].laundryBasket.ContainsTrackedGarment();
        }
        return false;
    }
}
