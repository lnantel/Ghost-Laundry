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

    public TextMeshProUGUI TXT_CurrentAmount;
    public Color PositiveAmountColor;
    public Color NegativeAmountColor;

    private GameObject moneyPopUpPrefab;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        moneyPopUpPrefab = (GameObject)Resources.Load("MoneyPopUp");
    }

    private void OnEnable() {
        ShopInteractable.BoughtItem += OnItemBought;
        Customer.BagPickedUp += OnBagPickedUp;
        TimeManager.StartOfDay += OnStartOfDay;
    }

    private void OnDisable() {
        ShopInteractable.BoughtItem -= OnItemBought;
        Customer.BagPickedUp -= OnBagPickedUp;
        TimeManager.StartOfDay -= OnStartOfDay;
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        int fee = 0;
        int tip = 0;

        fee += bag.launderedGarments * LaunderedGarmentFee;
        tip += bag.perfectGarments * PerfectGarmentTip;

        if(bag.ruinedGarments > 0) {
            fee = 0;
            tip = 0;
        }

        //Spawn a pop-up
        if (fee > 0) {
            //Money pop-up
            GameObject popUp = Instantiate(moneyPopUpPrefab, bag.transform.position + Vector3.up, bag.transform.rotation, WorldSpaceCanvas.transform);
            if (tip > 0)
                popUp.GetComponentInChildren<TextMeshProUGUI>().text = "$" + (fee / 100.0f).ToString("N2") + " + " + (tip / 100.0f).ToString("N2");
            else
                popUp.GetComponentInChildren<TextMeshProUGUI>().text = "$" + (fee / 100.0f).ToString("N2");
            AudioManager.instance.PlaySoundAtPosition(Sounds.MoneyGain, bag.transform.position);
        }
        else {
            //TODO: Dissatisfied customer pop-up?

        }
        Customer.Pay(fee, tip, null);
        ModifyCurrentAmount(fee + tip);
    }

    private void OnItemBought(int price) {
        ModifyCurrentAmount(-price);
    }

    private void ModifyCurrentAmount(int amount) {
        CurrentAmount += amount;
    }

    private void OnGUI() {
        if(CurrentAmount >= 0) {
            TXT_CurrentAmount.text = "$" + (CurrentAmount / 100.0f).ToString("N2");
            TXT_CurrentAmount.color = PositiveAmountColor;
        }
        else {
            TXT_CurrentAmount.text = "-$" + Mathf.Abs((CurrentAmount / 100.0f)).ToString("N2");
            TXT_CurrentAmount.color = NegativeAmountColor;
        }
    }

    private void OnStartOfDay(int day) {
        ModifyCurrentAmount(-Rent);
    }
}
