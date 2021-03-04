using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager instance;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public int CurrentAmount;
    public int MaxAmount;

    public int PerfectGarmentRep;
    public int RagequitRep;
    public int NotDoneRep;
    public int RuinedGarmentRep;

    public Image fillImage;

    private void OnEnable() {
        Customer.Ragequit += OnRagequit;
        Customer.BagPickedUp += OnBagPickedUp;
    }

    private void OnDisable() {
        Customer.Ragequit -= OnRagequit;
        Customer.BagPickedUp -= OnBagPickedUp;
    }

    private void Start() {
        CurrentAmount = 0;
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        int value = 0;
        if(bag.ruinedGarments == 0) value += bag.perfectGarments * PerfectGarmentRep;
        else value += bag.ruinedGarments * RuinedGarmentRep;
        value += (bag.totalGarments - bag.launderedGarments - bag.ruinedGarments) * NotDoneRep;
        ModifyCurrentAmount(value);
    }

    private void OnRagequit() {
        ModifyCurrentAmount(RagequitRep);
    }

    public void ModifyCurrentAmount(int amount) {
        CurrentAmount = Mathf.Clamp(CurrentAmount + amount, 0, MaxAmount);
    }

    private void OnGUI() {
        fillImage.fillAmount = (float) CurrentAmount / MaxAmount;
    }
}
