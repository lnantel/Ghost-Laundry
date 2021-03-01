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

    private GameObject moneyPopUpPrefab;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        CurrentAmount = -Rent;
        moneyPopUpPrefab = (GameObject)Resources.Load("MoneyPopUp");
    }

    private void OnEnable() {
        ShopInteractable.BoughtItem += OnItemBought;
        Customer.BagPickedUp += OnBagPickedUp;
    }

    private void OnDisable() {
        ShopInteractable.BoughtItem -= OnItemBought;
        Customer.BagPickedUp -= OnBagPickedUp;
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

        ModifyCurrentAmount(fee + tip);
    }

    private void OnItemBought(int price) {
        ModifyCurrentAmount(-price);
    }

    private void ModifyCurrentAmount(int amount) {
        CurrentAmount += amount;
    }

    private void OnGUI() {
        TXT_CurrentAmount.text = "$" + (CurrentAmount / 100.0f).ToString("N2");
    }
}
