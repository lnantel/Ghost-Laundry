using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloRenderer : MonoBehaviour
{
    public GameObject CW_Light;
    public GameObject CCW_Light;

    public bool Visible;

    public void ShowHalo() {
        CW_Light.SetActive(true);
        CCW_Light.SetActive(true);
        Visible = true;
    }

    public void HideHalo() {
        CW_Light.SetActive(false);
        CCW_Light.SetActive(false);
        Visible = false;
    }
}
