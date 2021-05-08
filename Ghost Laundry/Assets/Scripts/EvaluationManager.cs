using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvaluationManager : MonoBehaviour
{
    public GameObject success;
    public GameObject failure;

    public TextMeshProUGUI TXT_S_MoneyCollected;
    public TextMeshProUGUI TXT_F_MoneyCollected;
    private int moneyCollected;

    public TextMeshProUGUI TXT_S_Rent;
    public TextMeshProUGUI TXT_F_Rent;
    private int rent;

    public TextMeshProUGUI TXT_S_SpentOnDetergent;
    public TextMeshProUGUI TXT_F_SpentOnDetergent;
    private int moneySpentOnDetergent;

    public TextMeshProUGUI TXT_S_CustomersServed;
    public TextMeshProUGUI TXT_F_CustomersServed;
    private int customersServed;

    public TextMeshProUGUI TXT_S_GarmentsLaundered;
    public TextMeshProUGUI TXT_F_GarmentsLaundered;
    private int garmentsLaundered;

    public ShowStarRating StarRating;

    private void OnEnable() {
        GameManager.ShowEvaluation += OnShowEvaluation;
        GameManager.HideEvaluation += OnHideEvaluation;

        Customer.Pay += OnCustomerPaid;
        ShopInteractable.BoughtItem += OnBoughtItem;
        Customer.BagPickedUp += OnBagPickedUp;
    }

    private void OnDisable() {
        GameManager.ShowEvaluation -= OnShowEvaluation;
        GameManager.HideEvaluation -= OnHideEvaluation;

        Customer.Pay -= OnCustomerPaid;
        ShopInteractable.BoughtItem -= OnBoughtItem;
        Customer.BagPickedUp -= OnBagPickedUp;
    }

    private void Start() {
        moneyCollected = 0;
        rent = MoneyManager.instance.Rent;
        moneySpentOnDetergent = 0;
        customersServed = 0;
        garmentsLaundered = 0;
    }

    private void OnShowEvaluation() {
        if(MoneyManager.instance.CurrentAmount >= 0 || SettingsManager.instance.NoFailMode) {
            UpdateSuccessText();
            success.SetActive(true);
            SaveManager.Save();
        }
        else {
            UpdateFailureText();
            failure.SetActive(true);
        }
    }

    private void OnHideEvaluation() {
        success.SetActive(false);
        failure.SetActive(false);
    }

    private void OnCustomerPaid(int fee, int tip, Customer customer) {
        customersServed++;
        moneyCollected += fee + tip;
    }

    private void OnBoughtItem(int price) {
        moneySpentOnDetergent += price;
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        garmentsLaundered += bag.launderedGarments;
    }

    private void UpdateSuccessText() {
        TXT_S_MoneyCollected.text = "Money collected : $" + (moneyCollected / 100.0f).ToString("N2");
        TXT_S_Rent.text = "Rent : -$" + (rent / 100.0f).ToString("N2");
        TXT_S_SpentOnDetergent.text = "Detergent : -$" + (moneySpentOnDetergent / 100.0f).ToString("N2");
        TXT_S_CustomersServed.text = "Customers served : " + customersServed;
        TXT_S_GarmentsLaundered.text = "Garments laundered : " + garmentsLaundered;
        StarRating.StarRating = (int)(ReputationManager.instance.CurrentAmount / ReputationManager.instance.AmountPerStar);
    }

    private void UpdateFailureText() {
        TXT_F_MoneyCollected.text = "Money collected : $" + (moneyCollected / 100.0f).ToString("N2");
        TXT_F_Rent.text = "Rent : -$" + (rent / 100.0f).ToString("N2");
        TXT_F_SpentOnDetergent.text = "Detergent : -$" + (moneySpentOnDetergent / 100.0f).ToString("N2");
        TXT_F_CustomersServed.text = "Customers served : " + customersServed;
        TXT_F_GarmentsLaundered.text = "Garments laundered : " + garmentsLaundered;
    }
}
