using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class EpilogueFlowchartController : MonoBehaviour
{
    public Canvas dialogCanvas;
    private Flowchart epilogue;

    private void Start() {
        epilogue = GetComponent<Flowchart>();
    }

    private void OnEnable() {
        EpilogueManager.StartEpilogue += OnEpilogueStart;
    }

    private void OnDisable() {
        EpilogueManager.StartEpilogue -= OnEpilogueStart;
    }

    private void OnEpilogueStart() {
        dialogCanvas.gameObject.SetActive(true);
        epilogue.ExecuteBlock("Start");
        GameManager.instance.OnDialogStart();
    }

    public void OnEpilogueEnd() {
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        dialogCanvas.gameObject.SetActive(false);
        GameManager.instance.OnEpilogueEnd();
    }
}
