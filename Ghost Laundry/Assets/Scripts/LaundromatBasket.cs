using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundromatBasket : MonoBehaviour, ITrackable
{
    public Basket basket;
    public Sprite[] tags;
    public SpriteRenderer tagSprite;

    public bool ContainsTrackedGarment() {
        return ((ITrackable)basket).ContainsTrackedGarment();
    }

    private void Start() {
        if (basket == null) basket = new Basket();
        tagSprite.sprite = tags[basket.tag];
    }
}
