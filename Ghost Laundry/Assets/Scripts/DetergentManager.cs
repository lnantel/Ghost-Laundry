using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DetergentManager : MonoBehaviour
{
    public static DetergentManager instance;

    public int MaxAmount;

    [HideInInspector]
    public int CurrentAmount;

    public TextMeshProUGUI TXT_Detergent;

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
        TXT_Detergent.text = "Detergent: " + CurrentAmount + "/" + MaxAmount;
    }
}
