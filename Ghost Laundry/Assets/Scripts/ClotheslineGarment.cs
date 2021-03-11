using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClotheslineGarment : MonoBehaviour
{
    public int index;

    public GameObject Top;
    public GameObject Pants;
    public GameObject Underwear;
    public GameObject Sock;
    public GameObject Dress;
    public GameObject Shirt;
    public GameObject Skirt;

    private Clothesline clothesline;
    private bool visible;

    private void Start() {
        clothesline = GetComponentInParent<Clothesline>();
    }

    private void OnGarmentHung(int i) {
        if(index == i) {
            visible = true;
            //Enable the right renderer, based on the garment's type
            //Give it a reference to the garment
        }
    }

    private void OnGarmentRemovedFromLine(int i) {
        if(index == i) {
            visible = false;
            //Disable renderers
            //Set garments to null
        }
    }
}
