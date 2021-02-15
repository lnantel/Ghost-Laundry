using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WorkStation : Interactable
{
    public static Action<LaundryGarment> LaundryGarmentReleased;

    public bool HasGravity;
    public Vector3[] basketSlots;

    [HideInInspector]
    public GameObject laundryTaskArea;

    [HideInInspector]
    public List<LaundryBasket> containedBaskets;

    protected int basketCapacity;
    protected bool[] basketSlotOccupied;

    protected GameObject laundryBasketPrefab;

    private void Start() {
        containedBaskets = new List<LaundryBasket>();
        laundryBasketPrefab = (GameObject)Resources.Load("LaundryBasket");
        basketCapacity = basketSlots.Length;
        basketSlotOccupied = new bool[basketCapacity];
    }

    public void Initialize() {
        LaundryBasket[] laundryBaskets = laundryTaskArea.GetComponentsInChildren<LaundryBasket>();
        foreach (LaundryBasket basket in laundryBaskets)
            containedBaskets.Add(basket);
    }

    public override void Interact() {
        //TODO:
        //if player is carrying a basket, attempt to input it to the workstation
        //if input fails, cancel interaction

        PlayerController.instance.enabled = false;
        laundryTaskArea.SetActive(true);
        LaundryTaskController.instance.gameObject.SetActive(true);
        LaundryTaskController.instance.activeWorkStation = this;
        TaskView.instance.PopUp(transform.position);
        LaundryTaskController.exitedTask += OnTaskExit;
        LaundryGarment.Released += OnLaundryGarmentReleased;
    }

    protected void OnTaskExit() {
        LaundryTaskController.exitedTask -= OnTaskExit;
        LaundryGarment.Released -= OnLaundryGarmentReleased;
        TaskView.instance.Minimize(transform.position);
        laundryTaskArea.SetActive(false);
        PlayerController.instance.enabled = true;
    }

    public Basket OutputBasket() {
        //if the workstation contains at least one basket
        if (containedBaskets.Count > 0) {
            Basket basket = containedBaskets[0].basket;

            //free up corresponding basket slot
            for(int i = 0; i < basketCapacity; i++) {
                if (basketSlotOccupied[i]) {
                    if (basketSlots[i] == containedBaskets[0].transform.localPosition) {
                        basketSlotOccupied[i] = false;
                        break;
                    }
                }
            }

            //destroy the LaundryBasket object in the LaundryArea
            Destroy(containedBaskets[0].gameObject);

            //remove it from the list
            containedBaskets.RemoveAt(0);

            //return the basket
            return basket;
        }
        return null;
    }

    public bool InputBasket(Basket basket) {
        //if the workstation has space for a basket
        if (containedBaskets.Count < basketCapacity) {
            //find a free spot
            Vector3 position = Vector3.zero;
            for(int i = 0; i < basketCapacity; i++) {
                if (!basketSlotOccupied[i]) {
                    position = basketSlots[i];
                    basketSlotOccupied[i] = true;
                    break;
                }
            }

            //instantiate it in the LaundryArea
            GameObject basketObject = Instantiate(laundryBasketPrefab, laundryTaskArea.transform.position + position, laundryTaskArea.transform.rotation, laundryTaskArea.transform);
            LaundryBasket laundryBasket = basketObject.GetComponent<LaundryBasket>();
            laundryBasket.basket = basket;

            //add it to the list
            containedBaskets.Add(laundryBasket);

            //Basket successfully added
            return true;
        }
        return false;
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        if(HasGravity)
            laundryGarment.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        else
            laundryGarment.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

        if(LaundryGarmentReleased != null)
            LaundryGarmentReleased(laundryGarment);
    }
}
