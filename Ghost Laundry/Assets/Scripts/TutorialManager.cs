using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Canvas skipCanvas;

    public TutorialArrow arrow;

    public Transform playerSpawnPoint;
    public Transform firstBasketSpawn;

    private GameObject laundromatBasketPrefab;
    private bool tutorialStarted;
    public int tutorialStep;
    public int tutorialSubStep;

    public List<List<Garment>> tutorialCustomers;

    public Transform pickUpCounter;

    void Start()
    {
        tutorialStep = 1;
        tutorialSubStep = 1;
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");
        tutorialCustomers = new List<List<Garment>>();
    }

    private void OnEnable() {
        WashingMachineDoor.GarmentGrabbed += OnRuinedGarmentGrabbed;
        BaggerAnimator.PlayerNearby += OnPlayerNearBagger;
        Bagger.BasketInput += OnBaggerInput;
        Bagger.BagOutput += OnBagOutput;
        PickUpCounter.BagReadyForPickUp += OnBagReadyForPickUp;
    }

    private void OnDisable() {
        WashingMachineDoor.GarmentGrabbed -= OnRuinedGarmentGrabbed;
        BaggerAnimator.PlayerNearby -= OnPlayerNearBagger;
        Bagger.BasketInput -= OnBaggerInput;
        Bagger.BagOutput -= OnBagOutput;
        PickUpCounter.BagReadyForPickUp -= OnBagReadyForPickUp;
    }

    void Update()
    {
        if (tutorialStarted) {
            switch (tutorialStep) {
                case 1:
                    switch (tutorialSubStep) {
                        case 1:
                            Step1A();
                            break;
                        case 2:
                            Step1B();
                            break;
                    }
                    break;
                case 2:
                    Step2();
                    break;
                case 3:
                    switch (tutorialSubStep) {
                        case 1:
                            Step3A();
                            break;
                        case 2:
                            Step3B();
                            break;
                        case 3:
                            Step3C();
                            break;
                        case 4:
                            Step3D();
                            break;
                        case 5:
                            Step3E();
                            break;
                        case 6:
                            Step3F();
                            break;
                    }
                    break;
                case 4:
                    switch (tutorialSubStep) {
                        case 1:
                            Step4A();
                            break;
                        case 2:
                            Step4B();
                            break;
                    }
                    break;
                case 5:
                    Step5();
                    break;
                case 6:
                    switch (tutorialSubStep) {
                        case 1:
                            Step6A();
                            break;
                        case 2:
                            Step6B();
                            break;
                    }
                    break;
                case 7:
                    switch (tutorialSubStep) {
                        case 1:
                            Step7A();
                            break;
                        case 2:
                            Step7B();
                            break;
                        case 3:
                            Step7C();
                            break;
                    }
                    break;
                case 8:
                    switch (tutorialSubStep) {
                        case 1:
                            Step8A();
                            break;
                        case 2:
                            Step8B();
                            break;
                    }
                    break;
                case 9:
                    switch (tutorialSubStep) {
                        case 1:
                            Step9A();
                            break;
                        case 2:
                            Step9B();
                            break;
                        case 3:
                            Step9C();
                            break;
                    }
                    break;
                case 10:
                    switch (tutorialSubStep) {
                        case 1:
                            Step10A();
                            break;
                        case 2:
                            Step10B();
                            break;
                    }
                    break;
            }
        }
    }

    public void StartTutorial() {
        GameManager.instance.HideCursor();
        skipCanvas.gameObject.SetActive(false);
        StartCoroutine(StartTutorialCoroutine());
    }

    private IEnumerator StartTutorialCoroutine() {
        if (GameManager.FadeIn != null) GameManager.FadeIn();

        //Disable literally everything
        //Washing Machine
        washingMachine.Lock();
        washingMachine.DoorLocked = true;
        washingMachine.StartButtonLocked = true;
        washingMachine.DetergentSlotLocked = true;
        washingMachine.SettingsLocked = true;

        //Dryer
        dryer.Lock();

        //Table
        table.Lock();
        table.FoldingLocked = true;

        //Ironing Board
        ironingBoard.Lock();

        //Bagger
        bagger.Lock();

        //Shop
        if(ShopInteractable.LockShop != null) ShopInteractable.LockShop();

        //Player
        PlayerStateManager.instance.LockMove();
        PlayerStateManager.instance.LockDash();

        DetergentManager.instance.CurrentAmount = 2;

        PlayerController.instance.transform.position = playerSpawnPoint.position;
        tutorialStarted = true;

        //Start the first dialog
        yield return new WaitForSecondsRealtime(1.0f);
        TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
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

    private bool moveUnlocked;
    private void Step1A() {
        //Allow movement but not dash
        if (!moveUnlocked) {
            PlayerStateManager.instance.UnlockMove();
            moveUnlocked = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (tutorialStep == 1 && tutorialSubStep == 1 && collision.gameObject.tag.Equals("Player")) {
            tutorialSubStep = 2;
            PlayerStateManager.instance.UnlockDash();
            StartCoroutine(WaitForCameraToPanUp());
        }
    }

    private IEnumerator WaitForCameraToPanUp() {
        PlayerStateManager.instance.LockMove();
        PlayerStateManager.instance.LockDash();

        yield return new WaitForLaundromatSeconds(1.0f);

        TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);

        PlayerStateManager.instance.UnlockMove();
        PlayerStateManager.instance.UnlockDash();
    }

    private void Step1B() {
        //When the player goes through the counter into the laundromat, trigger step 2
        if(PlayerStateManager.instance.CurrentRoomIndex == 0 && !PlayerStateManager.instance.Dashing) {
            tutorialStep = 2;
            tutorialSubStep = 0;
            StartCoroutine(WaitForCameraToPanUp());
        }
    }

    public WashingMachine washingMachine;
    private LaundromatBasket firstBasket;
    private bool firstBasketSpawned;
    private void Step2() {
        //Create and/or enable the basket
        if (!firstBasketSpawned) {
            firstBasket = SpawnFirstBasket();
            firstBasketSpawned = true;
            //Allow placing it in the washing machine with space
            washingMachine.Unlock();
        }

        if (firstBasketSpawned && !PlayerStateManager.instance.Carrying) {
            if (firstBasket != null) {
                arrow.SetTarget(firstBasket.transform);
            }
            else {
                arrow.Deactivate();
            }
        }
        else {
            arrow.SetTarget(washingMachine.transform);
        }

        //When the player places the basket into the washing machine, trigger step 3
        if (washingMachine.ContainsBasket()) {
            tutorialStep = 3;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private bool laundryViewOpened;
    private float timeSinceLaundryViewOpened;

    private WashingMachineDoor washingMachineDoor;
    private WashingMachineDetergentSlot washingMachineDetergentSlot;
    private LaundryButton washingMachineStartButton;

    private void Step3A() {
        if (TaskView.instance.open) {
            laundryViewOpened = true;
            washingMachineDoor = washingMachine.GetComponentInChildren<WashingMachineDoor>();
            washingMachineDetergentSlot = washingMachine.GetComponentInChildren<WashingMachineDetergentSlot>();
            washingMachineStartButton = washingMachine.GetComponentInChildren<LaundryButton>();
        }

        if (laundryViewOpened) {
            timeSinceLaundryViewOpened += TimeManager.instance.deltaTime;
        }

        if(timeSinceLaundryViewOpened > 1.0f) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);

            //Unlock WM door
            washingMachine.DoorLocked = false;
        }
    }

    private void Step3B() {

        if(washingMachineDoor != null) {
            arrow.SetTarget(washingMachineDoor.transform);
        }

        if(washingMachine.state == WashingMachineState.DoorOpen) {
            tutorialSubStep = 3;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3C() {

        if(washingMachine.state == WashingMachineState.DoorClosed && (int)washingMachine.CurrentLoad() < washingMachine.Capacity) {
            arrow.SetTarget(washingMachineDoor.transform);
        }else if (LaundryTaskController.instance.grabbedObject != null && washingMachine.state == WashingMachineState.DoorOpen) {
            arrow.SetTarget(washingMachineDoor.transform);
        }else if ((int)washingMachine.CurrentLoad() == washingMachine.Capacity && washingMachine.state == WashingMachineState.DoorOpen) {
            arrow.SetTarget(washingMachineDoor.transform);
        }
        else if (washingMachine.state == WashingMachineState.DoorOpen && LaundryTaskController.instance.grabbedObject == null) {
            arrow.SetTarget(washingMachine.basketSlots[0].laundryBasket.transform);
        }

        if (washingMachine.state == WashingMachineState.DoorClosed && (int)washingMachine.CurrentLoad() == washingMachine.Capacity) {
            tutorialSubStep = 4;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
            washingMachine.DoorLocked = true;
            washingMachine.DetergentSlotLocked = false;
        }
    }

    private void Step3D() {
        if (washingMachine.Detergent) {
            //Enable start button
            washingMachine.StartButtonLocked = false;
            arrow.SetTarget(washingMachineStartButton.transform);
        }
        
        if(!washingMachine.Detergent || washingMachineDetergentSlot.open)
            arrow.SetTarget(washingMachineDetergentSlot.transform);

        washingMachine.SetAutoCompleteFlag();

        if (washingMachine.state == WashingMachineState.Running) {
            tutorialSubStep = 5;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
            washingMachine.DetergentSlotLocked = true;
            washingMachine.DoorLocked = false;
        }
    }

    private IEnumerator step3ECR;

    private void Step3E() {
        //Wait for player to take out ruined garment
        arrow.SetTarget(washingMachineDoor.transform);
    }

    private void OnRuinedGarmentGrabbed(Garment garment) {
        if(garment.Ruined && tutorialStep == 3 && tutorialSubStep == 5) {
            if (step3ECR == null) {
                step3ECR = Step3ECoroutine();
                StartCoroutine(step3ECR);
            }
        }
    }

    private IEnumerator Step3ECoroutine() {
        //Freeze cursor
        LaundryTaskController.instance.Locked = true;
        yield return new WaitForLaundromatSeconds(1.0f);

        tutorialSubStep = 6;
        TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);

        yield return new WaitForLaundromatSeconds(0.1f);
        LaundryTaskController.instance.Locked = false;
        step3ECR = null;
    }

    public TableWorkstation table;

    private LaundromatBasket secondBasket;
    private bool secondBasketSpawned;
    private void Step3F() {
        //Close washing machine, spawn a new basket
        if (step3ECR == null && !secondBasketSpawned) {
            LaundryTaskController.instance.BackOut();

            secondBasket = SpawnSecondBasket();
            
            //Destroy the old one, as well as all its current and former contents
            washingMachine.OutputBasket();
            while (true) {
                Garment g = washingMachine.RemoveTopGarment();
                if (g == null) break;
            }

            LaundryGarment[] laundryGarments = washingMachine.GetComponentsInChildren<LaundryGarment>();
            for (int i = 0; i < laundryGarments.Length; i++) {
                Destroy(laundryGarments[i].gameObject);
            }

            washingMachine.Lock();
            washingMachine.DetergentSlotLocked = false;
            table.Unlock();

            secondBasketSpawned = true;
        }

        if (secondBasketSpawned && !PlayerStateManager.instance.Carrying) {
            if(secondBasket != null) {
                arrow.SetTarget(secondBasket.transform);
            }
            else {
                arrow.Deactivate();
            }
        }
        else {
            arrow.SetTarget(table.transform);
        }

        //When the new basket is input to the table, move on to step 4
        if (table.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
            tutorialStep = 4;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    int whiteBasketIndex = -1;
    int coloredBasketIndex = -1;

    private void Step4A() {
        //Detect when one basket of the table contains X colored garments and another contains Y white garments

        for(int i = 0; i < table.basketSlots.Length; i++) {
            if(table.basketSlots[i].laundryBasket.basket.contents.Count == 3) {
                whiteBasketIndex = i;
                for (int j = 0; j < table.basketSlots[i].laundryBasket.basket.contents.Count; j++) {
                    if (table.basketSlots[i].laundryBasket.basket.contents[j].Colored()) whiteBasketIndex = -1;
                }
            }

            if(table.basketSlots[i].laundryBasket.basket.contents.Count == 5) {
                coloredBasketIndex = i;
                for (int j = 0; j < table.basketSlots[i].laundryBasket.basket.contents.Count; j++) {
                    if (!table.basketSlots[i].laundryBasket.basket.contents[j].Colored()) coloredBasketIndex = -1;
                }
            }
        }

        //Then move on to step 4 B
        if (whiteBasketIndex != -1 && coloredBasketIndex != -1) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
            table.basketSlots[coloredBasketIndex].Lock();
        }
    }

    Basket whiteBasket;
    Basket coloredBasket;

    bool whiteBasketSpawned = false;

    List<Garment> garmentsToWash = new List<Garment>();

    private void Step4B() {
        if (!whiteBasketSpawned) {
            //Close table view and give basket of white clothing to player
            LaundryTaskController.instance.BackOut();

            whiteBasket = table.basketSlots[whiteBasketIndex].laundryBasket.basket;
            coloredBasket = table.basketSlots[coloredBasketIndex].laundryBasket.basket;

            LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
            laundromatBasket.basket = new Basket();
            laundromatBasket.basket.tag = whiteBasket.tag;

            for (int i = 0; i < whiteBasket.contents.Count; i++) {
                laundromatBasket.basket.AddGarment(whiteBasket.contents[i]);
                garmentsToWash.Add(whiteBasket.contents[i]);
            }

            PlayerController.instance.Take(laundromatBasket.gameObject);

            table.basketSlots[whiteBasketIndex].laundryBasket.basket.RemoveAll();
            if (WorkStation.BasketSlotsChanged != null) WorkStation.BasketSlotsChanged();

            table.Lock();
            washingMachine.Unlock();

            whiteBasketSpawned = true;
        }

        arrow.SetTarget(washingMachine.transform);

        //Then, detect when the clothes in the basket have been washed
        washingMachine.SetAutoCompleteFlag();
        bool allGarmentsWashed = true;
        for(int i = 0; i < garmentsToWash.Count; i++) {
            if (!garmentsToWash[i].Clean) allGarmentsWashed = false;
        }

        //If the player is out of detergent, the machine is done, and not all clothes are clean,
        //replenish a single dose of detergent so they don't run out
        if (DetergentManager.instance.CurrentAmount == 0 && washingMachine.state == WashingMachineState.Done && !allGarmentsWashed) {
            DetergentManager.instance.CurrentAmount = 1;
        }

        //Then, if all clothes are clean, trigger step 5
        if (allGarmentsWashed) {
            tutorialStep = 5;
            tutorialSubStep = 0;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    public Dryer dryer;
    private void Step5() {
        //When all clothes are removed from the WM, disable it
        bool washingMachineContainsGarments = washingMachine.ContainsAGarment(garmentsToWash.ToArray());
        if (!washingMachineContainsGarments) {
            washingMachine.Lock();
            //and enable the dryer
            dryer.Unlock();
            arrow.SetTarget(dryer.transform);
        }
        else {
            arrow.SetTarget(washingMachine.transform);
        }

        //Detect when the basket is input to the dryer
        if (dryer.ContainsBasket()) {
            tutorialStep = 6;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step6A() {
        dryer.SetAutoCompleteFlag();

        //Detect when all garments are dry
        bool allGarmentsDry = true;
        for (int i = 0; i < garmentsToWash.Count; i++) {
            if (!garmentsToWash[i].Dry) allGarmentsDry = false;
        }

        if (allGarmentsDry) {
            //Then, trigger step 6B
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    public IroningBoard ironingBoard;
    private void Step6B() {
        //When all clothes are removed from the dryer, disable it
        bool dryerContainsGarments = dryer.ContainsAGarment(garmentsToWash.ToArray());
        if (!dryerContainsGarments) {
            dryer.Lock();
            //Enable the ironing board
            ironingBoard.Unlock();
            arrow.SetTarget(ironingBoard.transform);
        }
        else {
            arrow.SetTarget(dryer.transform);
        }

        //When the basket is input to the ironing board, trigger step 7
        if (ironingBoard.ContainsBasket()) {
            tutorialStep = 7;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step7A() {
        //Wait for the player to iron all the clothes and remove them from the board
        //Check that all garments are ironed or burned
        bool allGarmentsPressedOrBurned = true;

        for(int i = 0; i < garmentsToWash.Count; i++) {
            if (!(garmentsToWash[i].Pressed || garmentsToWash[i].Burned)) allGarmentsPressedOrBurned = false;
        }

        if(allGarmentsPressedOrBurned && ironingBoard.garmentOnBoard == null) {
            bool allBurned = true;
            for (int i = 0; i < garmentsToWash.Count; i++) {
                if (!garmentsToWash[i].Burned) allBurned = false;
            }
            if (allBurned)
                tutorialSubStep = 3;
            else
                tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step7B() {

        //When all garments are removed from the ironingBoard, lock it and unlock the table
        bool ironingBoardContainsGarments = ironingBoard.ContainsAGarment(garmentsToWash.ToArray());
        if (!ironingBoardContainsGarments) {
            ironingBoard.Lock();
            //Enable the table
            table.Unlock();
            arrow.SetTarget(table.transform);
        }
        else {
            arrow.SetTarget(ironingBoard.transform);
        }

        //If the table contains all the garments, proceed to step 8
        if (table.ContainsAllGarments(garmentsToWash.ToArray())) {
            tutorialStep = 8;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
            table.FoldingLocked = false;
        }
    }

    private void Step7C() {
        //Reset to the start of Step 7
        for(int i = 0; i < garmentsToWash.Count; i++) {
            garmentsToWash[i].Burned = false;
            garmentsToWash[i].Pressed = false;
        }

        LaundryGarment[] laundryGarments = ironingBoard.GetComponentsInChildren<LaundryGarment>();
        for(int i = 0; i < laundryGarments.Length; i++) {
            laundryGarments[i].UpdateAppearance();
        }

        tutorialSubStep = 1;
    }

    private void Step8A() {
        //If all the clean garments are folded, proceed to 8B
        bool allFolded = true;
        for (int i = 0; i < garmentsToWash.Count; i++) {
            if (!garmentsToWash[i].Folded) allFolded = false;
        }

        if (allFolded) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    public Bagger bagger;
    private void Step8B() {
        //When all clothes are removed from the table, disable it
        bool tableContainsGarments = table.ContainsAGarment(garmentsToWash.ToArray());
        if (!tableContainsGarments) {
            table.Lock();
            //Unlock the bagger
            bagger.Unlock();
            arrow.SetTarget(bagger.transform);
        }
        else {
            arrow.SetTarget(table.transform);
        }
    }

    private void OnPlayerNearBagger() {
        if (!bagger.Locked && tutorialStep == 8 && tutorialSubStep == 2 && PlayerStateManager.instance.Carrying) {
            tutorialStep = 9;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step9A() {
        //Wait for the player to dump the basket into the bagger
        arrow.SetTarget(bagger.transform);
    }

    private void OnBaggerInput() {
        if(tutorialStep == 9 && tutorialSubStep == 1) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
            //Enable shop
            if (ShopInteractable.UnlockShop != null) ShopInteractable.UnlockShop();
        }
    }

    public Transform WallToShop;
    public Transform Shop;

    private void Step9B() {
        //Wait for the player to buy detergent, then trigger step 9C
        if(PlayerStateManager.instance.CurrentRoomIndex == 0) {
            arrow.SetTarget(WallToShop);
        }else if(PlayerStateManager.instance.CurrentRoomIndex == 2) {
            arrow.SetTarget(Shop);
        }

        if(DetergentManager.instance.CurrentAmount > 0) {
            tutorialSubStep = 3;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
            arrow.Deactivate();
            //Enable everything
            washingMachine.Unlock();
            washingMachine.DoorLocked = false;
            washingMachine.StartButtonLocked = false;
            washingMachine.DetergentSlotLocked = false;
            washingMachine.SettingsLocked = false;

            dryer.Unlock();
            ironingBoard.Unlock();
            table.Unlock();
            table.FoldingLocked = false;
            table.basketSlots[coloredBasketIndex].Unlock();
        }
    }

    private void Step9C() {
        //Wait for the bagger to produce a bag
    }

    private LaundromatBag laundromatBag;

    private void OnBagOutput() {
        if (tutorialStep == 9 && tutorialSubStep == 3) {
            tutorialStep = 10;
            tutorialSubStep = 1;

            laundromatBag = FindObjectOfType<LaundromatBag>();

            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    public TutorialBoss tutorialBoss;

    private void Step10A() {
        //Wait for the player to place the bag on the counter
        if (PlayerStateManager.instance.Carrying) {
            arrow.SetTarget(pickUpCounter);
        }
        else if(laundromatBag != null && !laundromatBag.ReadyForPickUp) {
            arrow.SetTarget(laundromatBag.transform);
        }
        else {
            arrow.Deactivate();
        }
    }

    private void OnBagReadyForPickUp(LaundromatBag bag) {
        if(tutorialStep == 10 && tutorialSubStep == 1) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }

        //Make baskets appear at the counter whenever a bag is produced during Step 10B
        if (tutorialStep == 10 && tutorialSubStep == 2) {
            SpawnRandomBasket();
        }

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
        tutorialBoss.gameObject.SetActive(true);
    }

    public void OnReady() {
        MoneyManager.instance.CurrentAmount = 0;
        TimeManager.instance.EndDay();
    }
}
