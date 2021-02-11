using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundromatBasket : MonoBehaviour
{
    public Basket basket;
    public Sprite[] tags;
    public SpriteRenderer tagSprite;

    private void Start() {
        if (basket == null) basket = new Basket();
        tagSprite.sprite = tags[basket.tag];
    }
}
