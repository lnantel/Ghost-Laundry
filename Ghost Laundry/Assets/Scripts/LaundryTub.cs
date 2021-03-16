using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaundryTub : MonoBehaviour
{
    public GameObject BubblesSprite;
    public float bubblesMinY;
    public float bubblesMaxY;

    private WashTub washTub;

    private void Start() {
        washTub = GetComponentInParent<WashTub>();
        if (washTub != null) UpdateAppearance();
    }

    private void OnEnable() {
        WashTub.SoapLevelChanged += UpdateAppearance;
    }

    private void OnDisable() {
        WashTub.SoapLevelChanged -= UpdateAppearance;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        LaundryGarment laundryGarment = collision.GetComponentInParent<LaundryGarment>();
        if(laundryGarment != null) {
            laundryGarment.garment.Dry = false;
            laundryGarment.UpdateAppearance();
        }
    }

    private void UpdateAppearance() {
        if (washTub.IsSoapy) {
            BubblesSprite.SetActive(true);
            BubblesSprite.transform.localPosition = new Vector3(BubblesSprite.transform.localPosition.x, bubblesMinY + (bubblesMaxY - bubblesMinY) * ((washTub.SoapLevel - 1.0f) / (washTub.MaxSoapLevel - 1.0f)), BubblesSprite.transform.localPosition.z);
        }
        else {
            BubblesSprite.SetActive(false);
        }
    }
}
