using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class TutorialManager : MonoBehaviour
{
    public Flowchart[] flowcharts;

    public Canvas dialogCanvas;

    public Transform playerSpawnPoint;
    public Transform firstBasketSpawn;

    private GameObject laundromatBasketPrefab;
    private bool tutorialStarted;
    public int tutorialStep;
    public int tutorialSubStep;

    void Start()
    {
        tutorialStep = 1;
        tutorialSubStep = 1;
        laundromatBasketPrefab = (GameObject)Resources.Load("LaundromatBasket");
    }

    private void OnEnable() {
        WashingMachineDoor.GarmentGrabbed += OnRuinedGarmentGrabbed;
        TimeManager.StartOfDay += StartTutorial;
        BaggerAnimator.PlayerNearby += OnPlayerNearBagger;
        Bagger.BasketInput += OnBaggerInput;
        Bagger.BagOutput += OnBagOutput;
    }

    private void OnDisable() {
        WashingMachineDoor.GarmentGrabbed -= OnRuinedGarmentGrabbed;
        TimeManager.StartOfDay -= StartTutorial;
        BaggerAnimator.PlayerNearby -= OnPlayerNearBagger;
        Bagger.BasketInput -= OnBaggerInput;
        Bagger.BagOutput -= OnBagOutput;
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
                    Step10();
                    break;
            }
        }
    }

    public void StartTutorial(int day) {
        if(day == 0) {
            //TODO: Disable literally everything

            DetergentManager.instance.CurrentAmount = 2;

            //Start the first dialog
            PlayerController.instance.transform.position = playerSpawnPoint.position;
            tutorialStarted = true;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void SpawnFirstBasket() {
        Basket basket = new Basket();
        Fabric cotton = new Fabric(FabricType.Cotton);

        basket.AddGarment(new GarmentPants(cotton, GarmentColor.Sky));
        basket.AddGarment(new GarmentUnderwear(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentSock(cotton, GarmentColor.Red));
        basket.AddGarment(new GarmentShirt(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentTop(cotton, GarmentColor.White));
        basket.AddGarment(new GarmentPants(cotton, GarmentColor.Salmon));

        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
    }

    private void SpawnSecondBasket() {
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

        LaundromatBasket laundromatBasket = Instantiate(laundromatBasketPrefab, firstBasketSpawn.position, firstBasketSpawn.rotation).GetComponent<LaundromatBasket>();
        laundromatBasket.basket = basket;
    }

    private bool playerHasMoved;
    private float timeSinceMoved;
    private void Step1A() {        
        //TODO: Allow movement but not dash

        //Wait a bit after the player starts moving around
        if (PlayerStateManager.instance.Walking) playerHasMoved = true;
        if (playerHasMoved) {
            timeSinceMoved += TimeManager.instance.deltaTime;
        }

        if(timeSinceMoved > 3.0f) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step1B() {
        //TODO: Enable dash

        //When the player goes through the counter into the laundromat, trigger step 2
        if(PlayerStateManager.instance.CurrentRoomIndex == 0 && !PlayerStateManager.instance.Dashing) {
            tutorialStep = 2;
            tutorialSubStep = 0;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep);
        }
    }

    public WashingMachine washingMachine;
    private bool firstBasketSpawned;
    private void Step2() {
        //Create and/or enable the basket
        if (!firstBasketSpawned) {
            SpawnFirstBasket();
            firstBasketSpawned = true;
        }

        //Allow placing it in the washing machine with either space or E, but don't enable the Laundry View just yet

        //When the player places the basket into the washing machine, trigger step 3
        if (washingMachine.ContainsBasket()) {
            tutorialStep = 3;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private bool laundryViewOpened;
    private float timeSinceLaundryViewOpened;
    private void Step3A() {
        if (TaskView.instance.open) {
            laundryViewOpened = true;
        }

        if (laundryViewOpened) {
            timeSinceLaundryViewOpened += TimeManager.instance.deltaTime;
        }

        if(timeSinceLaundryViewOpened > 1.0f) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3B() {
        if(washingMachine.state == WashingMachineState.DoorOpen) {
            tutorialSubStep = 3;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3C() {
        if(washingMachine.state == WashingMachineState.DoorClosed && (int)washingMachine.CurrentLoad() == washingMachine.Capacity) {
            tutorialSubStep = 4;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3D() {
        if (washingMachine.Detergent) {
            //Enable start button
        }

        if(washingMachine.state == WashingMachineState.Running) {
            tutorialSubStep = 5;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private IEnumerator step3ECR;

    private void Step3E() {
        //TODO: Auto-complete the wash cycle
        
        //Wait for player to take out ruined garment
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
        yield return new WaitForLaundromatSeconds(1.0f);
        tutorialSubStep = 6;
        TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        step3ECR = null;
    }

    public TableWorkstation table;

    private bool secondBasketSpawned;
    private void Step3F() {
        //Spawn a new basket
        if (!secondBasketSpawned) {
            SpawnSecondBasket();
            
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

            secondBasketSpawned = true;
        }

        //TODO: Enable putting baskets into the Table

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
        //TODO: Disable folding clothes
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

            whiteBasketSpawned = true;
        }
        //Then, detect when the clothes in the basket have been washed
        bool allGarmentsWashed = true;
        for(int i = 0; i < garmentsToWash.Count; i++) {
            if (!garmentsToWash[i].Clean) allGarmentsWashed = false;
        }

        //Then, trigger step 5
        if (allGarmentsWashed) {
            tutorialStep = 5;
            tutorialSubStep = 0;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    public Dryer dryer;
    private void Step5() {
        //TODO: Enable the dryer 
        //Detect when the basket is input to the dryer
        if (dryer.ContainsBasket()) {
            tutorialStep = 6;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step6A() {
        //TODO: Activate start button when dryer is full and door is closed
        //Detect when the dryer is running
        if(dryer.state == DryerState.Running) {
            //TODO: Auto-complete the dryer cycle
            //Then, trigger step 6B
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    public IroningBoard ironingBoard;
    private void Step6B() {
        //TODO: Enable the ironing board
        //TODO: When all clothes are removed from the dryer, disable it
        //When the basket is input to the ironing board, trigger step 7
        if (ironingBoard.ContainsBasket()) {
            tutorialStep = 7;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step7A() {
        //Wait for the player to iron all the clothes and remove them from the board
        //TODO: Check that all garments are ironed or burned
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
        //If the table contains all the garments, proceed to step 8
        if (table.basketSlots[0].laundryBasket.basket.contents.Count == garmentsToWash.Count) {
            tutorialStep = 8;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step7C() {
        //TODO: Reset to the start of Step 7
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
        //Wait for the player to approach the bagger with the basket
    }

    private void OnPlayerNearBagger() {
        if (tutorialStep == 8 && tutorialSubStep == 2 && PlayerStateManager.instance.Carrying) {
            tutorialStep = 9;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step9A() {
        //Wait for the player to dump the basket into the bagger
    }

    private void OnBaggerInput() {
        if(tutorialStep == 9 && tutorialSubStep == 1) {
            tutorialSubStep = 2;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step9B() {
        //Wait for the player to buy detergent, then trigger step 9C
        if(DetergentManager.instance.CurrentAmount > 0) {
            tutorialSubStep = 3;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step9C() {
        //Wait for the bagger to produce a bag
    }

    private void OnBagOutput() {
        if (tutorialStep == 9 && tutorialSubStep == 3) {
            tutorialStep = 10;
            tutorialSubStep = 1;
            TutorialFlowchartManager.instance.StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step10() {
        //Free practice
        //Enable everything, make baskets appear at the counter whenever a bag is produced
        //Spawn the Boss and make him interactable
        //On interaction, start day 1
    }

    private void OnReady() {
        TimeManager.instance.EndDay();
    }
}
