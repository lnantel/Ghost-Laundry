using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public Canvas WorldSpaceCanvas;
    public int CurrentAmount;
    public int Rent;

    public int LaunderedGarmentFee;
    public int PerfectGarmentTip;
    public int RuinedGarmentPenalty;

    public TextMeshProUGUI TXT_CurrentAmount;
    public TextMeshProUGUI TXT_ChangeAmount;
    public Color PositiveAmountColor;
    public Color NegativeAmountColor;

    private GameObject moneyPopUpPrefab;
    private int displayedAmount;
    private int displayedChangeAmount;
    private bool updateDisplayedAmount;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        moneyPopUpPrefab = (GameObject)Resources.Load("MoneyPopUp");
        updateDisplayedAmount = true;
    }

    private void OnEnable() {
        ShopInteractable.BoughtItem += OnItemBought;
        HelmetInteractable.BoughtItem += OnItemBought;
        Customer.BagPickedUp += OnBagPickedUp;
        TimeManager.StartOfDay += OnStartOfDay;
    }

    private void OnDisable() {
        ShopInteractable.BoughtItem -= OnItemBought;
        HelmetInteractable.BoughtItem -= OnItemBought;
        Customer.BagPickedUp -= OnBagPickedUp;
        TimeManager.StartOfDay -= OnStartOfDay;
    }

    private void Update() {
        if (updateDisplayedAmount) {
            displayedAmount = (int)Mathf.MoveTowards(displayedAmount, CurrentAmount, 5 / Time.deltaTime);
            displayedChangeAmount = CurrentAmount - displayedAmount;
        }
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        int fee = 0;
        int tip = 0;

        fee += bag.launderedGarments * LaunderedGarmentFee;
        tip += bag.perfectGarments * PerfectGarmentTip;

        fee -= bag.ruinedGarments * RuinedGarmentPenalty;

        if (bag.ruinedGarments > 0) tip = 0;

        //Spawn a pop-up
        GameObject popUp = Instantiate(moneyPopUpPrefab, bag.transform.position + Vector3.up, bag.transform.rotation, WorldSpaceCanvas.transform);
        TextMeshProUGUI TXT_PopUp = popUp.GetComponentInChildren<TextMeshProUGUI>();

        if (tip > 0) {
            TXT_PopUp.text = "$" + (fee / 100.0f).ToString("N2") + " + " + (tip / 100.0f).ToString("N2");
            TXT_PopUp.color = PositiveAmountColor;
        }
        else if(fee >= 0) {
            TXT_PopUp.text = "$" + (fee / 100.0f).ToString("N2");
            TXT_PopUp.color = PositiveAmountColor;
        }
        else {
            TXT_PopUp.text = "-$" + Mathf.Abs((fee / 100.0f)).ToString("N2");
            TXT_PopUp.color = NegativeAmountColor;
        }

        AudioManager.instance.PlaySoundAtPosition(Sounds.MoneyGain, bag.transform.position);

        Customer.Pay(fee, tip, null);
        ModifyCurrentAmount(fee + tip);
    }

    private void OnItemBought(int price) {
        ModifyCurrentAmount(-price);
    }

    private IEnumerator displayCoroutine;

    public void ModifyCurrentAmount(int amount) {
        updateDisplayedAmount = false;
        displayedChangeAmount += amount;
        CurrentAmount += amount;
        if(displayCoroutine != null) {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = DelayDisplayedAmountUpdate();
        StartCoroutine(displayCoroutine);
    }

    private IEnumerator DelayDisplayedAmountUpdate() {
        yield return new WaitForLaundromatSeconds(1.0f);
        updateDisplayedAmount = true;
    }

    private void OnGUI() {
        if(displayedAmount >= 0) {
            TXT_CurrentAmount.text = "$" + (displayedAmount / 100.0f).ToString("N2");
            TXT_CurrentAmount.color = PositiveAmountColor;
        }
        else {
            TXT_CurrentAmount.text = "-$" + Mathf.Abs((displayedAmount / 100.0f)).ToString("N2");
            TXT_CurrentAmount.color = NegativeAmountColor;
        }

        if (displayedChangeAmount > 0) {
            TXT_ChangeAmount.text = "+$" + (displayedChangeAmount / 100.0f).ToString("N2");
            TXT_ChangeAmount.color = PositiveAmountColor;
        }
        else if(displayedChangeAmount < 0){
            TXT_ChangeAmount.text = "-$" + Mathf.Abs((displayedChangeAmount / 100.0f)).ToString("N2");
            TXT_ChangeAmount.color = NegativeAmountColor;
        }
        else {
            TXT_ChangeAmount.text = "";
        }
    }

    private void OnStartOfDay(int day) {
        if(day != 0)
            ModifyCurrentAmount(-Rent);
    }
}
