using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DetergentManager : MonoBehaviour
{
    public static DetergentManager instance;

    public Animator animator;

    public int MaxAmount;

    [HideInInspector]
    public int CurrentAmount;

    public Image fillImage;

    private float displayedAmount;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        displayedAmount = 0.0f;
        Refill();
    }

    public bool UseDetergent() {
        if (CurrentAmount > 0) {
            CurrentAmount--;
            animator.SetTrigger("DetergentLoss");
            return true;
        }
        else {
            animator.SetTrigger("DetergentEmpty");
            return false;
        }
    }

    public void Refill() {
        CurrentAmount = MaxAmount;
        animator.SetTrigger("DetergentGain");
    }

    private void OnGUI() {
        //fillImage.fillAmount = (float)CurrentAmount / MaxAmount;
        displayedAmount = Mathf.MoveTowards(displayedAmount, (float)CurrentAmount / MaxAmount, 0.001f);
        fillImage.fillAmount = displayedAmount;
    }
}
