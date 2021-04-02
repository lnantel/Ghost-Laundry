using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpOverlay : MonoBehaviour
{
    public GameObject overlay;
    public bool visible;

    private void Start() {
        overlay.SetActive(visible);
    }

    public void ToggleOverlay() {
        visible = !visible;
        overlay.SetActive(visible);
    }
}
