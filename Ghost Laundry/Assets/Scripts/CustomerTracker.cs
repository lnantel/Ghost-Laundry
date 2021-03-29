using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CustomerTracker : MonoBehaviour
{
    public static Action<Customer> CustomerTracked;

    public CustomerUI[] CustomerUIs;

    public Customer TrackedCustomer;

    public TextMeshProUGUI TXT_PageTracker;

    public LaundryButton NextPageButton;
    public LaundryButton PreviousPageButton;

    private Bagger bagger;

    private int currentPage;
    private int numberOfPages;

    private void Start() {
        bagger = GetComponentInParent<Bagger>();
    }

    private void OnEnable() {
        CustomerManager.CustomerLeft += OnCustomerLeft;
    }

    private void OnDisable() {
        CustomerManager.CustomerLeft -= OnCustomerLeft;
    }

    private void OnCustomerLeft(Customer customer) {
        if (TrackedCustomer.Equals(customer)) {
            TrackedCustomer = null;
            if (CustomerTracked != null) CustomerTracked(null);
        }
    }

    private void Update() {
        UpdateTrackerGUI();
    }

    private void UpdateTrackerGUI() {
        numberOfPages = Mathf.CeilToInt((float) CustomerManager.instance.customersInLaundromat.Count / CustomerUIs.Length);
        currentPage = Mathf.Clamp(currentPage, 1, numberOfPages + 1);

        for (int i = 0; i < CustomerUIs.Length; i++) {
            if ((currentPage - 1) * CustomerUIs.Length + i < CustomerManager.instance.customersInLaundromat.Count)
                CustomerUIs[i].targetCustomer = CustomerManager.instance.customersInLaundromat[(currentPage - 1) * CustomerUIs.Length + i];
            else
                CustomerUIs[i].targetCustomer = null;
        }

        TXT_PageTracker.text = currentPage + " / " + numberOfPages;

        if(currentPage < numberOfPages) {
            NextPageButton.springsBack = true;
        }
        else {
            NextPageButton.pressed = true;
            NextPageButton.springsBack = false;
        }

        if(currentPage > 1) {
            PreviousPageButton.springsBack = true;
        }
        else {
            PreviousPageButton.pressed = true;
            PreviousPageButton.springsBack = false;
        }
    }

    public void NextPage() {
        currentPage = Mathf.Clamp(currentPage + 1, 1, numberOfPages);
    }

    public void PreviousPage() {
        currentPage = Mathf.Clamp(currentPage - 1, 1, numberOfPages);
    }

    public void TrackCustomer(int uiIndex) {
        Customer target = CustomerUIs[uiIndex].targetCustomer;
        if (target != null && (TrackedCustomer == null || !TrackedCustomer.Equals(target))) {
            TrackedCustomer = target;
            if (CustomerTracked != null) CustomerTracked(target);
        }
        else if(target != null && TrackedCustomer != null && TrackedCustomer.Equals(target)) {
            TrackedCustomer = null;
            if (CustomerTracked != null) CustomerTracked(null);
        }
    }
}
