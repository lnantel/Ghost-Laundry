using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DetergentManager : MonoBehaviour
{
    public static DetergentManager instance;

    public int MaxAmount;

    [HideInInspector]
    public int CurrentAmount;

    public Image fillImage;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Refill();
    }

    public bool UseDetergent() {
        if (CurrentAmount > 0) {
            CurrentAmount--;
            return true;
        }
        else
            return false;
    }

    public void Refill() {
        CurrentAmount = MaxAmount;
    }

    private void OnGUI() {
        fillImage.fillAmount = (float)CurrentAmount / MaxAmount;
    }
}
