﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUpCounter : MonoBehaviour
{
    public static Action<LaundromatBag> BagReadyForPickUp;

    private void OnTriggerStay2D(Collider2D collision) {
        LaundromatBag bag = collision.GetComponent<LaundromatBag>();
        if (bag != null && !bag.ReadyForPickUp) {
            collision.enabled = false;
            bag.ReadyForPickUp = true;
            BagReadyForPickUp(bag);
        }
    }
}