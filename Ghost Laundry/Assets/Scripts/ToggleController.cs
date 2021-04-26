using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    private Toggle toggle;

    private void Start() {
        toggle = GetComponent<Toggle>();
    }

    private void OnEnable() {
        if(toggle != null)
            toggle.interactable = true;
    }

    private void OnDisable() {
        if(toggle != null)
            toggle.interactable = true;
        StopAllCoroutines();
    }

    public void OnValueChanged() {
        if(gameObject.activeInHierarchy)
            StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown() {
        if(toggle != null) {
            toggle.interactable = false;
            yield return new WaitForSecondsRealtime(0.1f);
            toggle.interactable = true;
        }
    }
}
