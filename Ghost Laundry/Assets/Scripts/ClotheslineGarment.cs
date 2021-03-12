using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClotheslineGarment : MonoBehaviour
{
    public int index;

    public ClotheslineGarmentRenderer Top;
    public ClotheslineGarmentRenderer Pants;
    public ClotheslineGarmentRenderer Underwear;
    public ClotheslineGarmentRenderer Sock;
    public ClotheslineGarmentRenderer Dress;
    public ClotheslineGarmentRenderer Shirt;
    public ClotheslineGarmentRenderer Skirt;

    private Clothesline clothesline;

    private void Start() {
        clothesline = GetComponentInParent<Clothesline>();
    }

    private void OnEnable() {
        Clothesline.GarmentHung += OnGarmentHung;
        Clothesline.GarmentRemoved += OnGarmentRemovedFromLine;
    }

    private void OnDisable() {
        Clothesline.GarmentHung -= OnGarmentHung;
        Clothesline.GarmentRemoved -= OnGarmentRemovedFromLine;
    }

    private void OnGarmentHung(int i) {
        if(index == i) {
            //Enable the right renderer, based on the garment's type
            //Give it a reference to the garment
            Garment garment = clothesline.hungGarments[i];
            if (garment is GarmentTop) {
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

    private void OnGarmentRemovedFromLine(int i) {
        if(index == i) {
            //Disable renderers
            //Set garments to null
            Top.gameObject.SetActive(false);
            Pants.gameObject.SetActive(false);
            Underwear.gameObject.SetActive(false);
            Sock.gameObject.SetActive(false);
            Dress.gameObject.SetActive(false);
            Shirt.gameObject.SetActive(false);
            Skirt.gameObject.SetActive(false);

            Top.garment = null;
            Pants.garment = null;
            Underwear.garment = null;
            Sock.garment = null;
            Dress.garment = null;
            Shirt.garment = null;
            Skirt.garment = null;
        }
    }
}
