using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        GameManager.FadeIn += FadeIn;
        GameManager.FadeOut += FadeOut;
        GameManager.WhiteIn += WhiteIn;
        GameManager.WhiteOut += WhiteOut;
    }

    private void OnDisable() {
        GameManager.FadeIn -= FadeIn;
        GameManager.FadeOut -= FadeOut;
        GameManager.WhiteIn -= WhiteIn;
        GameManager.WhiteOut -= WhiteOut;
    }

    private void FadeIn() {
        animator.SetTrigger("FadeIn");
    }

    private void FadeOut() {
        animator.SetTrigger("FadeOut");
    }

    private void WhiteIn() {
        animator.SetTrigger("WhiteIn");
    }

    private void WhiteOut() {
        animator.SetTrigger("WhiteOut");
    }
}
