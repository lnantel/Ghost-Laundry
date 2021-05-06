using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Canvas skipCanvas;

    public TutorialArrow arrow;

    public Transform playerSpawnPoint;
    public Transform firstBasketSpawn;

    private GameObject laundromatBasketPrefab;
    private bool tutorialStarted;
    public int step;

    public List<List<Garment>> tutorialCustomers;

    public Transform pickUpCounter;

    //Workstations
    public TableWorkstation table;
    public WashingMachine washingMachine;
    public Dryer dryer;
    public IroningBoard ironingBoard;
    public Bagger bagger;
    [HideInInspector]
    public ShopInteractable shop;

    void Start()
    {
        step = -1;
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");
        tutorialCustomers = new List<List<Garment>>();
    }

    private void OnEnable() {
        PickUpCounter.BagReadyForPickUp += OnBagReadyForPickUp;
        ShopInteractable.BoughtItem += OnDetergentBought;
        WorkStation.LaundryGarmentReleased += OnLaundryGarmentReleased;
    }

    private void OnDisable() {
        PickUpCounter.BagReadyForPickUp -= OnBagReadyForPickUp;
        ShopInteractable.BoughtItem -= OnDetergentBought;
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
    }

    void Update()
    {
        
    }

    public void NextStep() {
        //Start the appropriate step transition
        step++;
        TutorialFlowchartManager.instance.StartDialog(step);
    }

    //Each step transition flowchart should end with a call to the appropriate "StartStepX" function

    //Step 0: Buy Detergent at the shop
    public void StartTutorial() {
        GameManager.instance.HideCursor();
        skipCanvas.gameObject.SetActive(false);
        StartCoroutine(StartTutorialCoroutine());
    }

    private IEnumerator StartTutorialCoroutine() {
        TutorialFlowchartManager.instance.StartIntroduction();

        if (GameManager.FadeIn != null) GameManager.FadeIn();

        DetergentManager.instance.CurrentAmount = 0;

        PlayerController.instance.transform.position = playerSpawnPoint.position;
        tutorialStarted = true;

        shop = FindObjectOfType<ShopInteractable>();
        LockAllWorkstations();

        //Start the first dialog
        TutorialFlowchartManager.instance.StartIntroduction();
        yield return null;
    }

    //Step 0: Buy Detergent
    public void StartStep0() {
        step = 0;
        shop.Unlock();
    }

    private void OnDetergentBought(int i) {
        if (step == 0) NextStep();
    }

    //Step 1: Washing Machine
    public void StartStep1() {
        step = 1;
        washingMachine.Unlock();
        SpawnFirstBasket();
    }

    //Step 2: Dryer
    public void StartStep2() {
        step = 2;
        dryer.Unlock();
    }

    //Step 3: Ironing Board
    public void StartStep3() {
        step = 3;
        ironingBoard.Unlock();
    }

    //Step 4: Folding
    public void StartStep4() {
        step = 4;
        table.Unlock();
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        switch (step) {
            case 1:
                if (laundryGarment.garment.Clean) NextStep();
                break;
            case 2:
                if (laundryGarment.garment.Dry) NextStep();
                break;
            case 3:
                if (laundryGarment.garment.Pressed) NextStep();
                break;
            case 4:
                if (laundryGarment.garment.Folded) NextStep();
                break;
            default:
                break;
        }
    }

    //Step 5: Bagger
    public void StartStep5() {
        step = 5;
        bagger.Unlock();
    }

    private void OnBagReadyForPickUp() {
        if (step == 5) NextStep();
    }

    //Step 6: Free practice
    public void StartFreePractice() {
        step = 6;
        UnlockAllWorkstations();
    }

    private void LockAllWorkstations() {
        table.Lock();
        washingMachine.Lock();
        dryer.Lock();
        ironingBoard.Lock();
        bagger.Lock();
        shop.Lock();
    }

    private void UnlockAllWorkstations() {
        table.Unlock();
        washingMachine.Unlock();
        dryer.Unlock();
        ironingBoard.Unlock();
        bagger.Unlock();
        shop.Unlock();
    }

    private void CreateTutorialCustomer(Basket basket) {
        List<Garment> garments = new List<Garment>();
        for (int i = 0; i < basket.contents.Count; i++) {
            basket.contents[i].customerID = tutorialCustomers.Count;
            garments.Add(basket.contents[i]);
        }
        tutorialCustomers.Add(garments);
    }

    private LaundromatBasket SpawnFirstBasket() {
        Basket basket = new Basket();
        Fabric cotton = new Fabric(FabricType.Cotton);

        basket.AddGarment(new GarmentPants(cotton, GarmentColor.Sky));
        basket.AddGarment(new GarmentUnderwear(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentShirt(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentTop(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentPants(cotton, GarmentColor.Salmon));

        CreateTutorialCustomer(basket);

        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
        return laundromatBasket;
    }

    private LaundromatBasket SpawnSecondBasket() {
        Basket basket = new Basket();
        Fabric cotton = new Fabric(FabricType.Cotton);

        basket.AddGarment(new GarmentPants(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentUnderwear(cotton, GarmentColor.Mint));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Golden));
        basket.AddGarment(new GarmentUnderwear(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Golden));
        basket.AddGarment(new GarmentShirt(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentTop(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentTop(cotton, GarmentColor.Salmon));

        CreateTutorialCustomer(basket);

        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
        return laundromatBasket;
    }

    private LaundromatBasket SpawnRandomBasket() {
        Basket basket = LaundryManager.GetRandomBasket();

        CreateTutorialCustomer(basket);

        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
        return laundromatBasket;
    }

    private void OnBagReadyForPickUp(LaundromatBag bag) {
        //if(step == 10 && tutorialSubStep == 1) {
        //    arrow.Deactivate();
        //    tutorialSubStep = 2;
        //    TutorialFlowchartManager.instance.StartDialog(step, tutorialSubStep);
        //}

        //Make baskets appear at the counter whenever a bag is produced during Step 10B
        //if (step == 10 && tutorialSubStep == 2) {
        //    SpawnRandomBasket();
        //}

        StartCoroutine(DestroyBagAfterDelay(bag));
    }

    private IEnumerator DestroyBagAfterDelay(LaundromatBag bag) {
        yield return new WaitForLaundromatSeconds(3.0f);
        Destroy(bag.gameObject);
    }

    private void Step10B() {
        //Free practice
        //Spawn the Boss
        //Talking to him ends the tutorial
        //tutorialBoss.gameObject.SetActive(true);
    }

    public void OnReady() {
        skipCanvas.sortingOrder = 99;
        MoneyManager.instance.CurrentAmount = 0;
        TimeManager.instance.EndDay();
    }

    public void BossDialog() {
        //Start the correct detailed explanation dialog based on which tutorial step has been reached
        if(step < 6)
            TutorialFlowchartManager.instance.StartDetailedExplanation(step);
        else
            OnReady();
    }
}
