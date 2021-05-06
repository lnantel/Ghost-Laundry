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
    public int step;

    public List<List<Garment>> tutorialCustomers;

    public Transform pickUpCounter;

    //Arrow locations
    public Transform CrackedWallLocation;
    public Transform ShopCounterLocation;

    //Boss & Spawn locations
    public TutorialBoss boss;
    public Transform bossShopLocation;
    public Transform bossWMLocation;
    public Transform bossDryerLocation;
    public Transform bossIronLocation;
    public Transform bossTableLocation;
    public Transform bossBaggerLocation;
    public Transform bossFreePracticeLocation;

    //Workstations
    public TableWorkstation table;
    public WashingMachine washingMachine;
    public Dryer dryer;
    public IroningBoard ironingBoard;
    public Bagger bagger;
    [HideInInspector]
    public ShopInteractable shop;

    private IEnumerator delayedNextStep;

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
        Bagger.BagOutput += OnBagOutput;
        BaggerAnimator.PlayerNearby += OnPlayerNearBagger;
    }

    private void OnDisable() {
        PickUpCounter.BagReadyForPickUp -= OnBagReadyForPickUp;
        ShopInteractable.BoughtItem -= OnDetergentBought;
        WorkStation.LaundryGarmentReleased -= OnLaundryGarmentReleased;
        Bagger.BagOutput -= OnBagOutput;
        BaggerAnimator.PlayerNearby -= OnPlayerNearBagger;
    }

    private void Update() {
        if (TimeManager.instance.TimeIsPassing && delayedNextStep == null) {
            switch (step) {
                case 0:
                    Step0();
                    break;
                case 1:
                    Step1();
                    break;
                case 2:
                    Step2();
                    break;
                case 3:
                    Step3();
                    break;
                case 4:
                    Step4();
                    break;
                case 5:
                    Step5();
                    break;
                default:
                    break;
            }
        }
    }

    private void SpawnBoss(Transform location) {
        boss.gameObject.SetActive(true);
        boss.transform.position = location.position;
    }

    public void NextStep() {
        //Start the appropriate step transition
        step++;
        TutorialFlowchartManager.instance.StartDialog(step);
    }

    private IEnumerator DelayedNextStep(float delay) {
        if(arrow != null) arrow.Deactivate();
        yield return new WaitForLaundromatSeconds(delay);
        NextStep();
        delayedNextStep = null;
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
        SpawnBoss(bossShopLocation);
    }

    //Step 0 Update
    private void Step0() {
        //If player is in Laundromat, show arrow over cracked wall
        if (PlayerStateManager.instance.CurrentRoomIndex == 0) {
            arrow.SetTarget(CrackedWallLocation);
        }
        //otherwise, show arrow over shop counter
        else {
            arrow.SetTarget(ShopCounterLocation);
        }
    }

    private void OnDetergentBought(int i) {
        if (step == 0 && delayedNextStep == null) {
            delayedNextStep = DelayedNextStep(3.0f);
            StartCoroutine(delayedNextStep);
        }
    }

    //Step 1: Washing Machine
    LaundromatBasket firstBasket;
    bool WMDetergentTooltip = false;
    bool WMDoorTooltip = false;
    bool WMinit = false;
    WashingMachineDetergentSlot detergentSlot;
    WashingMachineDoor WMDoor;

    public void StartStep1() {
        step = 1;
        washingMachine.Unlock();
        firstBasket = SpawnFirstBasket();
        SpawnBoss(bossWMLocation);
    }

    //Step 1 Update
    private void Step1() {
        //If basket is not carried or in washing machine, show arrow over basket
        if(firstBasket != null && !PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(firstBasket.transform);
        }
        //If it is being carried or the laundry view is closed, show arrow over washing machine
        else if(firstBasket != null || !washingMachine.laundryTaskArea.activeSelf){
            arrow.SetTarget(washingMachine.transform);
        }
        //If the laundryview is opened:
        else if(washingMachine.laundryTaskArea.activeSelf){
            if (!WMinit) {
                WMinit = true;
                detergentSlot = washingMachine.GetComponentInChildren<WashingMachineDetergentSlot>();
                WMDoor = washingMachine.GetComponentInChildren<WashingMachineDoor>();
            }
            if (washingMachine.state != WashingMachineState.Done && !washingMachine.Detergent && washingMachine.basketSlots[0].laundryBasket.basket != null && washingMachine.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
                if (!WMDetergentTooltip) {
                    ToastManager.instance.SayLine("First, put detergent in the machine by clicking the detergent slot.", 3.0f);
                    WMDetergentTooltip = true;
                }
                arrow.SetTarget(detergentSlot.transform);
            }
            else if(washingMachine.state != WashingMachineState.Running) {
                if (!WMDoorTooltip) {
                    ToastManager.instance.SayLine("Then, open the door and drag your dirty clothes inside.", 3.0f);
                    ToastManager.instance.SayLine("When you're done, close the door and hit the ON button!", 3.0f);
                    WMDoorTooltip = true;
                }
                arrow.SetTarget(WMDoor.transform);
            }
            else if(washingMachine.state == WashingMachineState.Running) {
                arrow.Deactivate();
            }
        }
    }

    //Step 2: Dryer
    bool basketTooltip = false;
    bool lintTrapTooltip = false;
    bool dryerDoorTooltip = false;
    bool dryerInit = false;
    DryerLintTrap lintTrap;
    DryerDoor dryerDoor;

    public bool DryerTooltips { get; private set; }

    public void StartStep2() {
        step = 2;
        dryer.Unlock();
        dryer.lintTrapClean = false;
        basketTooltip = false;
        SpawnBoss(bossDryerLocation);
    }

    //Step 2 Update
    private void Step2() {
        if (!basketTooltip) {
            ToastManager.instance.SayLine("Get your clothes in the basket, then bring them over to the dryer!", 2.0f);
            basketTooltip = true;
        }

        if(washingMachine.GetAllContainedGarments().Count > 0) {
            arrow.SetTarget(washingMachine.transform);
        }
        else if(washingMachine.basketSlots[0].laundryBasket.basket != null && washingMachine.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(washingMachine.transform);
        }
        else if (PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(dryer.transform);
        }
        else if (dryer.laundryTaskArea.activeSelf) {
            if (!dryerInit) {
                dryerInit = true;
                dryerDoor = dryer.GetComponentInChildren<DryerDoor>();
                lintTrap = dryer.GetComponentInChildren<DryerLintTrap>();
            }

            if (dryer.state != DryerState.Done && !dryer.lintTrapClean && dryer.basketSlots[0].laundryBasket.basket != null && dryer.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
                if(!lintTrapTooltip) {
                    ToastManager.instance.SayLine("First, clean the lint trap by clicking on it.", 3.0f);
                    lintTrapTooltip = true;
                }
                arrow.SetTarget(lintTrap.transform);
            }
            else if(dryer.state != DryerState.Running) {
                if (!dryerDoorTooltip) {
                    ToastManager.instance.SayLine("Then, open the dryer door and drag your wet clothes into it.", 3.0f);
                    ToastManager.instance.SayLine("When you're done, close the door and hit the ON button!", 3.0f);
                    dryerDoorTooltip = true;
                }
                arrow.SetTarget(dryerDoor.transform);
            }
            else if(dryer.state == DryerState.Running) {
                arrow.Deactivate();
            }
        }
    }

    //Step 3: Ironing Board
    bool boardTooltip = false;
    bool ironTooltip = false;

    public void StartStep3() {
        step = 3;
        basketTooltip = false;
        ironingBoard.Unlock();
        SpawnBoss(bossIronLocation);
    }

    //Step 3 Update
    private void Step3() {
        if (!basketTooltip) {
            ToastManager.instance.SayLine("Put the clothes in the basket, then bring it to the ironing board.", 2.0f);
            basketTooltip = true;
        }

        if (dryer.GetAllContainedGarments().Count > 0) {
            arrow.SetTarget(dryer.transform);
        }
        else if (dryer.basketSlots[0].laundryBasket.basket != null && dryer.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(dryer.transform);
        }
        else if (PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(ironingBoard.transform);
        }
        else if (ironingBoard.laundryTaskArea.activeSelf) {
            arrow.Deactivate();

            if (!boardTooltip) {
                ToastManager.instance.SayLine("Ironin's easy. Put somethin' on the board.", 1.0f);
                boardTooltip = true;
            }

            if(ironingBoard.garmentOnBoard != null & !ironTooltip) {
                ToastManager.instance.SayLine("Okay, grab the iron, and start movin' it back and forth over the fabric.", 2.0f);
                ironTooltip = true;
            }
        }
    }

    //Step 4: Folding
    bool tableTooltips = false;
    public void StartStep4() {
        step = 4;
        table.Unlock();
        basketTooltip = false;
        SpawnBoss(bossTableLocation);
    }

    //Step 4 Update
    private void Step4() {
        if (!basketTooltip) {
            ToastManager.instance.SayLine("When you're done, put everything in a basket, then bring it to the table.", 2.0f);
            basketTooltip = true;
        }

        if (ironingBoard.GetAllContainedGarments().Count > 0) {
            arrow.SetTarget(ironingBoard.transform);
        }
        else if (ironingBoard.basketSlots[0].laundryBasket.basket != null && ironingBoard.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(ironingBoard.transform);
        }
        else if (PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(table.transform);
        }
        else if (table.laundryTaskArea.activeSelf) {
            if (!tableTooltips) {
                ToastManager.instance.SayLine("Put somethin' on the table, then click it 'til it's folded. Easy peasy.", 2.0f);
                ToastManager.instance.SayLine("Put 'em back in a basket when yer done.", 2.0f);
                tableTooltips = true;
            }
        }
    }

    private void OnLaundryGarmentReleased(LaundryGarment laundryGarment) {
        bool progress = false;
        switch (step) {
            case 1:
                if (laundryGarment.garment.Clean) progress = true;
                break;
            case 2:
                if (laundryGarment.garment.Clean && laundryGarment.garment.Dry) progress = true;
                break;
            case 3:
                if (laundryGarment.garment.Pressed) progress = true;
                break;
            case 4:
                if (laundryGarment.garment.Folded) progress = true;
                break;
            default:
                break;
        }
        if (progress && delayedNextStep == null) {
            delayedNextStep = DelayedNextStep(1.0f);
            StartCoroutine(delayedNextStep);
        }
    }

    //Step 5: Bagger
    bool baggerTooltip = false;
    bool bagTooltip = false;
    bool bagReady = false;
    LaundromatBag bag;
    bool bagInit = false;
    public void StartStep5() {
        step = 5;
        bagger.Unlock();
        basketTooltip = false;
        SpawnBoss(bossBaggerLocation);
    }

    //Step 5 Update
    private void Step5() {
        if (!basketTooltip) {
            ToastManager.instance.SayLine("Bring your basket of folded clothes to the bagger!", 2.0f);
            basketTooltip = true;
        }

        if (bagInit && !bagReady && !PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(bag.transform);
        }
        else if(bagInit && !bagReady && PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(pickUpCounter);
        }
        else if (bagReady) {
            arrow.Deactivate();
        }
        else if (table.GetAllContainedGarments().Count > 0) {
            arrow.SetTarget(table.transform);
        }
        else if (table.basketSlots[0].laundryBasket.basket != null && table.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(table.transform);
        }
        else if(table.basketSlots[1].laundryBasket.basket != null && table.basketSlots[1].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(table.transform);
        }
        else if (table.basketSlots[2].laundryBasket.basket != null && table.basketSlots[2].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(table.transform);
        }
        else if (table.basketSlots[3].laundryBasket.basket != null && table.basketSlots[3].laundryBasket.basket.contents.Count > 0) {
            arrow.SetTarget(table.transform);
        }
        else if (PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(bagger.transform);
        }
    }

    private void OnPlayerNearBagger() {
        if(step == 5 && !baggerTooltip) {
            ToastManager.instance.SayLine("Just dump that basket into the chute on the side.", 1.0f);
            ToastManager.instance.SayLine("Make sure you get all the clothes!", 1.0f);
            baggerTooltip = true;
        }
    }

    private void OnBagOutput() {
        if(step == 5 && !bagTooltip) {
            ToastManager.instance.SayLine("There you go! Grab that bag, and put it on the counter, left of the register.", 1.0f);
            bagTooltip = true;
            bag = FindObjectOfType<LaundromatBag>();
            bagInit = true;
        }
    }

    //Step 6: Free practice
    public void StartFreePractice() {
        step = 6;
        UnlockAllWorkstations();
        SpawnBoss(bossFreePracticeLocation);
        SpawnRandomBasket();
    }

    private void OnBagReadyForPickUp(LaundromatBag bag) {
        if (step == 5 && delayedNextStep == null) {
            delayedNextStep = DelayedNextStep(1.0f);
            StartCoroutine(delayedNextStep);
            bagReady = true;
        }

        //Make baskets appear at the counter whenever a bag is produced during Step 10B
        if (step == 6) {
            SpawnRandomBasket();
        }

        StartCoroutine(DestroyBagAfterDelay(bag));
    }

    private IEnumerator DestroyBagAfterDelay(LaundromatBag bag) {
        yield return new WaitForLaundromatSeconds(3.0f);
        Destroy(bag.gameObject);
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

    private LaundromatBasket SpawnRandomBasket() {
        Basket basket = LaundryManager.GetRandomBasket();

        CreateTutorialCustomer(basket);

        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
        return laundromatBasket;
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
