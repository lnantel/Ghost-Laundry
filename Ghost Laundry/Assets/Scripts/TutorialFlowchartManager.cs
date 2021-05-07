using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System;

public class TutorialFlowchartManager : MonoBehaviour
{
    public static TutorialFlowchartManager instance;

    public Flowchart introduction;
    public Flowchart[] transitions;
    public Flowchart[] detailedExplanations;
    public Canvas dialogCanvas;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartIntroduction() {
        introduction.gameObject.SetActive(true);
        dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    public void OnIntroductionEnd() {
        OnDialogEnd();
    }

    public void StartDialog(int step) {
        transitions[step].gameObject.SetActive(true);
        dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    public void StartDetailedExplanation(int step) {
        detailedExplanations[step].gameObject.SetActive(true);
        dialogCanvas.gameObject.SetActive(true);
        GameManager.instance.OnDialogStart();
    }

    public void OnDialogEnd() {
        introduction.gameObject.SetActive(false);
        for(int i = 0; i < transitions.Length; i++) {
            transitions[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < detailedExplanations.Length; i++) {
            detailedExplanations[i].gameObject.SetActive(false);
        }
        StartCoroutine(DisableDialogCanvas());
        GameManager.instance.OnDialogEnd(0);
    }

    private IEnumerator DisableDialogCanvas() {
        yield return null;
        dialogCanvas.gameObject.SetActive(false);
    }

    public void NextStep() {
        ToastManager.instance.SayLine("Move with WASD. Dash with SHIFT.", 2.0f, true);
        TutorialManager.instance.delayedNextStep = TutorialManager.instance.DelayedNextStep(10.0f);
        StartCoroutine(TutorialManager.instance.delayedNextStep);
    }

    public void StartStep0() {
        TutorialManager.instance.StartStep0();
    }

    public void StartStep1() {
        TutorialManager.instance.StartStep1();
    }

    public void StartStep2() {
        TutorialManager.instance.StartStep2();
    }

    public void StartStep3() {
        TutorialManager.instance.StartStep3();
    }

    public void StartStep4() {
        TutorialManager.instance.StartStep4();
    }

    public void StartStep5() {
        TutorialManager.instance.StartStep5();
    }

    public void StartFreePractice() {
        TutorialManager.instance.StartFreePractice();
    }
}
