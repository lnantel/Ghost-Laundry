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
    }

    private void OnDisable() {
        GameManager.FadeIn -= FadeIn;
        GameManager.FadeOut -= FadeOut;
    }

    private void FadeIn() {
        animator.SetTrigger("FadeIn");
    }

    private void FadeOut() {
        animator.SetTrigger("FadeOut");
    }
}
