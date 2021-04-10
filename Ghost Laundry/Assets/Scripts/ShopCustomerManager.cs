using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCustomerManager : MonoBehaviour
{
    //Odds of spawning a customer at a given time throughout the day
    public AnimationCurve Occupancy;

    public float MinVisitTime;
    public float MaxVisitTime;

    private void OnEnable() {
        TimeManager.TimeOfDay += OnTimeOfDay;
    }

    private void OnDisable() {
        TimeManager.TimeOfDay -= OnTimeOfDay;
    }

    private void OnTimeOfDay(int[] time) {
        float ratio = ((time[0] - 12.0f) + time[1] / 60.0f) / 12.0f;
        float odds = Occupancy.Evaluate(ratio);
        float roll = UnityEngine.Random.value;
        if (roll < odds) SpawnShopCustomer();
    }

    private void SpawnShopCustomer() {
        //Get a random inactive customer
        //Spawn them for a random time
        List<ShopCustomer> shopCustomers = new List<ShopCustomer>();
        for(int i = 0; i < transform.childCount; i++) {
            if (!transform.GetChild(i).gameObject.activeSelf) shopCustomers.Add(transform.GetChild(i).GetComponent<ShopCustomer>());
        }
        ShopCustomer shopCustomer = shopCustomers[UnityEngine.Random.Range(0, shopCustomers.Count)];
        shopCustomer.gameObject.SetActive(true);
        shopCustomer.lifetime = UnityEngine.Random.Range(MinVisitTime, MaxVisitTime);
    }
}
