using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class TutorialManager : MonoBehaviour
{
    public Flowchart[] flowcharts;

    public Canvas dialogCanvas;

    public Transform playerSpawnPoint;

    private bool tutorialStarted;
    public int tutorialStep;
    public int tutorialSubStep;

    void Start()
    {
        tutorialStep = 1;
        tutorialSubStep = 1;
    }

    private void OnEnable() {
        WashingMachineDoor.GarmentGrabbed += OnRuinedGarmentGrabbed;
        TimeManager.StartOfDay += StartTutorial;
    }

    private void OnDisable() {
        WashingMachineDoor.GarmentGrabbed -= OnRuinedGarmentGrabbed;
        TimeManager.StartOfDay -= StartTutorial;
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

            //Start the first dialog
            PlayerController.instance.transform.position = playerSpawnPoint.position;
            tutorialStarted = true;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void StartDialog(int step, int substep = 0) {
        flowcharts[step - 1].gameObject.SetActive(true);
        dialogCanvas.gameObject.SetActive(true);
        if (GameManager.ShowDialog != null) GameManager.ShowDialog();
    }

    private void OnDialogEnd(int step, int substep = 0) {
        flowcharts[step - 1].gameObject.SetActive(false);
        StartCoroutine(DisableDialogCanvas());
        if (GameManager.HideDialog != null) GameManager.HideDialog();
    }
    
    private IEnumerator DisableDialogCanvas() {
        yield return null;
        dialogCanvas.gameObject.SetActive(false);
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
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step1B() {
        //TODO: Enable dash

        //When the player goes through the counter into the laundromat, trigger step 2
        if(PlayerStateManager.instance.CurrentRoomIndex == 0) {
            tutorialStep = 2;
            tutorialSubStep = 0;
            StartDialog(tutorialStep);
        }
    }

    public WashingMachine washingMachine;
    private void Step2() {
        //Create and/or enable the basket
        //Allow placing it in the washing machine with either space or E, but don't enable the Laundry View just yet

        //When the player places the basket into the washing machine, trigger step 3
        if (washingMachine.ContainsBasket()) {
            tutorialStep = 3;
            tutorialSubStep = 1;
            StartDialog(tutorialStep, tutorialSubStep);
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
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3B() {
        if(washingMachine.state == WashingMachineState.DoorOpen) {
            tutorialSubStep = 3;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3C() {
        if(washingMachine.state == WashingMachineState.DoorClosed && (int)washingMachine.CurrentLoad() == washingMachine.Capacity) {
            tutorialSubStep = 4;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step3D() {
        if (washingMachine.Detergent) {
            //Enable start button
        }

        if(washingMachine.state == WashingMachineState.Running) {
            tutorialSubStep = 5;
            StartDialog(tutorialStep, tutorialSubStep);
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
        StartDialog(tutorialStep, tutorialSubStep);
        step3ECR = null;
    }

    public TableWorkstation table;

    private void Step3F() {
        //Spawn a new basket
        //Destroy the old one, as well as all its current and former contents
        //Enable putting baskets into the Table

        //When the new basket is input to the table, move on to step 4
        if (table.basketSlots[0].laundryBasket.basket.contents.Count > 0) {
            tutorialStep = 4;
            tutorialSubStep = 1;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step4A() {
        //Detect when one basket of the table contains X colored garments and another contains Y white garments
        //Then move on to step 4 B
    }

    private void Step4B() {
        //Detect when one basket is taken out of the table
        //Then, lock the table
        //Then, detect when the clothes in the basket have been washed
        //Then, trigger step 5
    }

    public Dryer dryer;
    private void Step5() {
        //Enable the dryer & Detect when the basket is input to the dryer
        if (dryer.ContainsBasket()) {
            tutorialStep = 6;
            tutorialSubStep = 1;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step6A() {
        //Detect when the dryer is running
        //Auto-complete the dryer cycle
        //Then, trigger step 6B
    }

    public IroningBoard ironingBoard;
    private void Step6B() {
        //Enable the ironing board
        //When all clothes are removed from the dryer, disable it
        //When the basket is input to the ironing board, trigger step 7
        if (ironingBoard.ContainsBasket()) {
            tutorialStep = 7;
            tutorialSubStep = 1;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step7A() {
        //Wait for the player to iron all the clothes and remove them from the board
        //TODO: Cache the laundryIroningBoard ref
        //TODO: Check that all garments are ironed or burned
        if(ironingBoard.GetComponentInChildren<LaundryIroningBoard>().garmentOnBoard == null) {

        }
    }

    private void Step7B() {

        //If the table contains all the garments, proceed to step 8
    }

    private void Step7C() {
        //Reset to the start of Step 7
    }

    private void Step8A() {
        //If all the clean garments are folded, proceed to 8B
    }

    private void Step8B() {
        //Wait for the player to approach the bagger with the basket

    }

    private void Step9A() {
        //Wait for the player to dump the basket into the bagger
    }

    private void Step9B() {
        //Wait for the player to buy detergent, then trigger step 9C
        if(DetergentManager.instance.CurrentAmount > 0) {
            tutorialSubStep = 3;
            StartDialog(tutorialStep, tutorialSubStep);
        }
    }

    private void Step9C() {
        //Wait for the bagger to produce a bag
    }

    private void OnBagOutput() {
        if (tutorialStep == 9 && tutorialSubStep == 3) {
            tutorialStep = 10;
            tutorialSubStep = 1;
            StartDialog(tutorialStep, tutorialSubStep);
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
