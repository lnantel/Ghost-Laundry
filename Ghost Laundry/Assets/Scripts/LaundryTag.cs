using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LaundryTag : MonoBehaviour
{
    public bool visible;

    public TextMeshProUGUI TXT_Fabric;
    public TextMeshProUGUI TXT_Owner;

    public Image WashIcon;
    public Image DryIcon;
    public Image PressIcon;

    public Sprite[] WashSprites;
    public Sprite[] DrySprites;
    public Sprite[] PressSprites;

    public Garment garment;
    private Canvas canvas;

    private void Start() {
        //garment = GetComponentInParent<LaundryGarment>().garment;
        canvas = GetComponent<Canvas>();
        Hide();
        //Initialize();
    }

    //private void Initialize() {

    //}

    private void UpdateAppearance() {
        garment = GetComponentInParent<LaundryGarment>().garment;
        if (garment != null) {
            TXT_Fabric.text = "100% " + garment.fabric.name;
            TXT_Owner.text = "Owner: #" + garment.customerID.ToString("D3");
            WashIcon.sprite = WashSprites[(int)garment.fabric.washingRestrictions];
            DryIcon.sprite = DrySprites[(int)garment.fabric.dryingRestrictions];
            PressIcon.sprite = PressSprites[(int)garment.fabric.pressingRestrictions];
        }
    }

    public void Show() {
        if (!visible) {
            UpdateAppearance();
            visible = true;
            canvas.enabled = true;
        }
    }

    public void Hide() {
        if (visible) {
            visible = false;
            canvas.enabled = false;
        }
    }
}
