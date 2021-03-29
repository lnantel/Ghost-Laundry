using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    public Customer targetCustomer;

    public Canvas CustomerCanvas;
    public LaundryButton FindButton;

    public TextMeshProUGUI TXT_CustomerID;
    public TextMeshProUGUI TXT_ProgressBar;

    public Image FilledProgressBar;
    public Image Portrait;

    public int TotalGarments { get => GetTotalGarments(); }
    public int GarmentsInBagger { get => GetGarmentsInBagger(); }

    private Bagger bagger;
    private CustomerTracker tracker;

    private void Start() {
        bagger = GetComponentInParent<Bagger>();
        tracker = GetComponentInParent<CustomerTracker>();
    }

    private int GetTotalGarments() {
        return targetCustomer.garments.Count;
    }

    private int GetGarmentsInBagger() {
        int count = 0;
        for(int i = 0; i < targetCustomer.garments.Count; i++) {
            if (bagger.contents.Contains(targetCustomer.garments[i]))
                count++;
        }
        return count;
    }

    private void Update() {
        UpdateUI();
    }

    private void UpdateUI() {
        if(targetCustomer != null) {
            if(!CustomerCanvas.gameObject.activeSelf) CustomerCanvas.gameObject.SetActive(true);
            if (!FindButton.gameObject.activeSelf) FindButton.gameObject.SetActive(true);

            //Customer information
            if(targetCustomer is RecurringCustomer recCustomer) {
                TXT_CustomerID.text = "Customer ID: " + recCustomer.customerName;
                Portrait.sprite = EventManager.instance.customerPortraits[recCustomer.EventTreeIndex];
            }
            else {
                TXT_CustomerID.text = "Customer ID: #" + targetCustomer.ticketNumber.ToString("D3");
                Portrait.sprite = targetCustomer.portraits[targetCustomer.silhouetteIndex];
            }

            //Customer state information
            switch (targetCustomer.state) {
                case CustomerState.Arriving:
                    TXT_ProgressBar.text = "Arriving...";
                    FilledProgressBar.fillAmount = 0.0f;
                    break;
                case CustomerState.Queueing:
                    TXT_ProgressBar.text = "Queueing...";
                    FilledProgressBar.fillAmount = 0.0f;
                    break;
                case CustomerState.WaitingForService:
                    TXT_ProgressBar.text = "Waiting for service...";
                    FilledProgressBar.fillAmount = 0.0f;
                    break;
                case CustomerState.WaitingForClothes:
                    TXT_ProgressBar.text = GarmentsInBagger + " / " + TotalGarments;
                    FilledProgressBar.fillAmount = (float) GarmentsInBagger / TotalGarments;
                    break;
                case CustomerState.PickingUpBag:
                    TXT_ProgressBar.text = "Picking up bag...";
                    FilledProgressBar.fillAmount = 1.0f;
                    break;
                case CustomerState.Leaving:
                    FilledProgressBar.fillAmount = 1.0f;
                    TXT_ProgressBar.text = "Leaving...";
                    break;
                case CustomerState.Ragequitting:
                    FilledProgressBar.fillAmount = 0.0f;
                    TXT_ProgressBar.text = "Leaving...";
                    break;
                case CustomerState.HasLeft:
                    TXT_ProgressBar.text = "Has left.";
                    break;
            }

            //Find Button status
            if (targetCustomer.state == CustomerState.WaitingForClothes) {
                //Enable button
                FindButton.springsBack = true;
                if (tracker.TrackedCustomer != null && tracker.TrackedCustomer.Equals(targetCustomer))
                    FindButton.pressed = true;
                else
                    FindButton.pressed = false;
            }
            else {
                //Disable button
                FindButton.springsBack = false;
                FindButton.pressed = true;
            }
        }
        else {
            if (CustomerCanvas.gameObject.activeSelf) CustomerCanvas.gameObject.SetActive(false);
            if (FindButton.gameObject.activeSelf) FindButton.gameObject.SetActive(false);
        }
    }
}
