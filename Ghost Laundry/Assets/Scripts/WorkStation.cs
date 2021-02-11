using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkStation : Interactable
{
    public GameObject laundryTaskArea;

    public List<LaundryBasket> containedBaskets;

    public int basketCapacity = 1;

    private GameObject laundryBasketPrefab;

    private void Start() {
        containedBaskets = new List<LaundryBasket>();
        laundryBasketPrefab = (GameObject)Resources.Load("LaundryBasket");
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
        TaskView.instance.PopUp(transform.position);
        LaundryTaskController.exitedTask += OnTaskExit;
    }

    protected void OnTaskExit() {
        LaundryTaskController.exitedTask -= OnTaskExit;
        TaskView.instance.Minimize(transform.position);
        laundryTaskArea.SetActive(false);
        PlayerController.instance.enabled = true;
    }

    public Basket OutputBasket() {
        //if the workstation contains at least one basket
        if (containedBaskets.Count > 0) {
            Basket basket = containedBaskets[0].basket;

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
            //instantiate it in the LaundryArea
            GameObject basketObject = Instantiate(laundryBasketPrefab, laundryTaskArea.transform.position, laundryTaskArea.transform.rotation, laundryTaskArea.transform);
            LaundryBasket laundryBasket = basketObject.GetComponent<LaundryBasket>();

            //replace default Basket component with the input basket
            laundryBasket.basket = basket;

            //add it to the list
            containedBaskets.Add(laundryBasket);

            //Basket successfully added
            return true;
        }
        return false;
    }
}
