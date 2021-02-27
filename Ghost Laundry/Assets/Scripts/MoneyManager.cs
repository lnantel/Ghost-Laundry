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

    public TextMeshProUGUI TXT_CurrentAmount;

    private GameObject moneyPopUpPrefab;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        CurrentAmount = 0;
        moneyPopUpPrefab = (GameObject)Resources.Load("MoneyPopUp");
    }

    private void OnEnable() {
        Customer.Pay += OnCustomerPay;
    }

    private void OnDisable() {
        Customer.Pay -= OnCustomerPay;
    }

    private void OnCustomerPay(int fee, int tip, Customer customer) {
        //Spawn a pop-up
        if(fee > 0) {
            //Money pop-up
            GameObject popUp = Instantiate(moneyPopUpPrefab, customer.transform.position + Vector3.up, customer.transform.rotation, WorldSpaceCanvas.transform);
            if (tip > 0)
                popUp.GetComponentInChildren<TextMeshProUGUI>().text = "$" + fee + " + " + tip;
            else
                popUp.GetComponentInChildren<TextMeshProUGUI>().text = "$" + fee;
            AudioManager.instance.PlaySoundAtPosition(Sounds.MoneyGain, customer.transform.position);
        }
        else {
            //Dissatisfied customer pop-up

        }

        ModifyCurrentAmount(fee + tip);
    }

    private void ModifyCurrentAmount(int amount) {
        CurrentAmount += amount;
    }

    private void OnGUI() {
        TXT_CurrentAmount.text = "$" + CurrentAmount;
    }
}
