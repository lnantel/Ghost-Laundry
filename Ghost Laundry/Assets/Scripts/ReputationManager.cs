using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReputationManager : MonoBehaviour {
    public static ReputationManager instance;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public int CurrentAmount;
    public int MaxAmount;

    public int PerfectGarmentRep;
    public int RagequitRep;
    public int NotDoneRep;
    public int RuinedGarmentRep;
    public int LaunderedGarmentRep;

    public Image fillImage;

    public PoolManager starPool;

    private int displayedAmount;
    private IEnumerator spawnStarCoroutine;

    private List<StarData> starQueue;

    private void OnEnable() {
        Customer.Ragequit += OnRagequit;
        Customer.BagPickedUp += OnBagPickedUp;
        FlyingStar.ReachedDestination += OnStarReachedReputationBar;
    }

    private void OnDisable() {
        Customer.Ragequit -= OnRagequit;
        Customer.BagPickedUp -= OnBagPickedUp;
        FlyingStar.ReachedDestination -= OnStarReachedReputationBar;
    }

    private void Start() {
        CurrentAmount = 0;
        starQueue = new List<StarData>();
    }

    private struct StarData{
        public Vector3 position;
        public bool positive;
        public int amount;
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        int value = 0;
        value += bag.perfectGarments * PerfectGarmentRep;
        value += (bag.launderedGarments - bag.perfectGarments) * LaunderedGarmentRep;
        value += bag.ruinedGarments * RuinedGarmentRep;
        value += (bag.totalGarments - bag.launderedGarments - bag.ruinedGarments) * NotDoneRep;
        ModifyCurrentAmount(value, bag.transform.position);
    }

    private void OnRagequit() {
        ModifyCurrentAmount(RagequitRep);
    }

    public void ModifyCurrentAmount(int amount) {
        CurrentAmount = Mathf.Clamp(CurrentAmount + amount, 0, MaxAmount);
    }

    public void ModifyCurrentAmount(int amount, Vector3 position) {
        ModifyCurrentAmount(amount);
        StarData starData = new StarData();
        starData.amount = Mathf.Abs(amount);
        starData.position = position;
        starData.positive = amount >= 0;
        starQueue.Add(starData);
    }

    private void Update() {
        if(starQueue.Count > 0) {
            spawnStarCoroutine = SpawnFlyingStars(starQueue[0]);
            StartCoroutine(spawnStarCoroutine);
            starQueue.RemoveAt(0);
        }else if(spawnStarCoroutine == null && displayedAmount != CurrentAmount) {
            displayedAmount = CurrentAmount;
        }
    }

    private void OnStarReachedReputationBar(bool sign) {
        displayedAmount += sign ? 1 : -1;
        Sounds sound = sign ? Sounds.MoneyTallyGain : Sounds.MoneyTallyLoss;
        AudioManager.instance.PlaySound(sound, 0.7f);
    }

    private IEnumerator SpawnFlyingStars(StarData starData) {
        for(int i = 0; i < starData.amount; i++) {
            FlyingStar star = starPool.SpawnObject(starData.position).GetComponent<FlyingStar>();
            star.SetSign(starData.positive);
            yield return new WaitForLaundromatSeconds(0.05f);
        }

        spawnStarCoroutine = null;
    }

    private void OnGUI() {
        fillImage.fillAmount = (float) displayedAmount / MaxAmount;
    }
}
