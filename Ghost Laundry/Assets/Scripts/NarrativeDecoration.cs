using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeDecoration : MonoBehaviour
{
    public GameObject OllieSkull;
    public GameObject OllieSkateboard;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialize()); 
    }

    private IEnumerator Initialize() {
        yield return null;
        UpdateDecorations();
    }

    private void UpdateDecorations() {
        if (EventManager.instance != null) {
            OllieSkull.SetActive(EventManager.instance.OlliesSkull());
            OllieSkateboard.SetActive(EventManager.instance.OlliesSkateboard());
        }
    }

    private void OnEnable() {
        GameManager.HideDialog += UpdateDecorations;
    }

    private void OnDisable() {
        GameManager.HideDialog -= UpdateDecorations;
    }
}
