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
    public Image IMG_WashTemp;
    public Image IMG_DrySetting;

    private Garment garment;
    private Canvas canvas;

    private void Start() {
        garment = GetComponentInParent<LaundryGarment>().garment;
        canvas = GetComponent<Canvas>();
        Hide();
        Initialize();
    }

    private void Initialize() {
        TXT_Fabric.text = "100% " + garment.fabric.name;
        TXT_Owner.text = "Customer number: #" + garment.customerID.ToString("D3");
        //TODO: Change IMG_WashTemp and IMG_DrySetting according to fabric
    }

    public void Show() {
        if (!visible) {
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
