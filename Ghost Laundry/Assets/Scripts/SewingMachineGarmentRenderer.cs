using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewingMachineGarmentRenderer : MonoBehaviour
{
    private SewingMachine sewingMachine;

    public ClotheslineGarmentRenderer Top;
    public ClotheslineGarmentRenderer Pants;
    public ClotheslineGarmentRenderer Underwear;
    public ClotheslineGarmentRenderer Sock;
    public ClotheslineGarmentRenderer Dress;
    public ClotheslineGarmentRenderer Shirt;
    public ClotheslineGarmentRenderer Skirt;

    public float MinY;
    public float MaxY;

    private void Start() {
        sewingMachine = GetComponentInParent<SewingMachine>();
    }

    private void OnEnable() {
        SewingMachine.GarmentUpdated += UpdateAppearance;
        SewingMachine.SewingProgressUpdated += OnSewingProgressUpdate;
    }

    private void OnDisable() {
        SewingMachine.GarmentUpdated -= UpdateAppearance;
        SewingMachine.SewingProgressUpdated -= OnSewingProgressUpdate;
    }

    private void OnSewingProgressUpdate() {
        transform.localPosition = new Vector3(transform.localPosition.x, MinY + (MaxY - MinY) * (Mathf.Floor(sewingMachine.progress * 10.0f) / 10.0f), transform.localPosition.z);
    }

    public void UpdateAppearance() {
        Garment garment = sewingMachine.garmentOnMachine;
        if(garment == null) {
            Top.garment = null;
            Top.gameObject.SetActive(false);
            Pants.garment = null;
            Pants.gameObject.SetActive(false);
            Underwear.garment = null;
            Underwear.gameObject.SetActive(false);
            Sock.garment = null;
            Sock.gameObject.SetActive(false);
            Dress.garment = null;
            Dress.gameObject.SetActive(false);
            Shirt.garment = null;
            Shirt.gameObject.SetActive(false);
            Skirt.garment = null;
            Skirt.gameObject.SetActive(false);
        }
        else if(garment is GarmentTop) {
            Top.gameObject.SetActive(true);
            Top.garment = garment;
            Top.UpdateAppearance();
        }
        else if (garment is GarmentPants) {
            Pants.gameObject.SetActive(true);
            Pants.garment = garment;
            Pants.UpdateAppearance();
        }
        else if (garment is GarmentUnderwear) {
            Underwear.gameObject.SetActive(true);
            Underwear.garment = garment;
            Underwear.UpdateAppearance();
        }
        else if (garment is GarmentSock) {
            Sock.gameObject.SetActive(true);
            Sock.garment = garment;
            Sock.UpdateAppearance();
        }
        else if (garment is GarmentDress) {
            Dress.gameObject.SetActive(true);
            Dress.garment = garment;
            Dress.UpdateAppearance();
        }
        else if (garment is GarmentShirt) {
            Shirt.gameObject.SetActive(true);
            Shirt.garment = garment;
            Shirt.UpdateAppearance();
        }
        else if (garment is GarmentSkirt) {
            Skirt.gameObject.SetActive(true);
            Skirt.garment = garment;
            Skirt.UpdateAppearance();
        }
    }
}
