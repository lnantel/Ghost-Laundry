using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClotheslinePeg : LaundryObject
{
    public static Action<Garment> GrabGarment;

    public int index;

    private Clothesline clothesline;
    private Collider2D col;

    private void Start() {
        clothesline = GetComponentInParent<Clothesline>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable() {
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
    }

    private void OnDisable() {
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        if (col.bounds.Contains(laundryGarment.transform.position)) {
            if (clothesline.HangGarment(index, laundryGarment.garment)) {
                AudioManager.instance.PlaySound(laundryGarment.garment.fabric.dropSound);
                laundryGarment.ReturnToPool();
            }
        }
    }

    public override void OnGrab() {
        if (clothesline.hungGarments[index] != null) {
            if (GrabGarment != null) GrabGarment(clothesline.hungGarments[index]);
            clothesline.RemoveGarmentFromLine(index);
        }
    }
}
