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

    public float AmountPerStar { get => MaxAmount / 5.0f; }

    public int PerfectGarmentRep;
    public int RagequitRep;
    public int NotDoneRep;
    public int RuinedGarmentRep;
    public int LaunderedGarmentRep;

    public Image fillImage;

    public PoolManager starPool;

    public int HighScore { get => Mathf.Max(m_HighScore, CurrentAmount); set => m_HighScore = value; }
    private int m_HighScore;

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
        public Transform position;
        public bool positive;
        public int amount;
    }

    private void OnBagPickedUp(LaundromatBag bag) {
        int value = 0;
        value += bag.perfectGarments * PerfectGarmentRep;
        value += (bag.launderedGarments - bag.perfectGarments) * LaunderedGarmentRep;
        value += bag.ruinedGarments * RuinedGarmentRep;
        value += (bag.totalGarments - bag.launderedGarments - bag.ruinedGarments) * NotDoneRep;
        Customer customer = null;
        for (int i = 0; i < CustomerManager.instance.customersInLaundromat.Count; i++)
            if (CustomerManager.instance.customersInLaundromat[i].ticketNumber == bag.customerID) customer = CustomerManager.instance.customersInLaundromat[i];
        if (customer != null)
            ModifyCurrentAmount(value, customer.transform);
        else
            ModifyCurrentAmount(value, null);
    }

    private void OnRagequit() {
        ModifyCurrentAmount(RagequitRep);
    }

    public void ModifyCurrentAmount(int amount) {
        CurrentAmount = Mathf.Clamp(CurrentAmount + amount, 0, MaxAmount);
    }

    public void ModifyCurrentAmount(int amount, Transform position) {
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
        SoundName sound = sign ? SoundName.ReputationGain : SoundName.ReputationLoss;
        AudioManager.instance.PlaySound(sound);
    }

    private IEnumerator SpawnFlyingStars(StarData starData) {
        for(int i = 0; i < starData.amount; i++) {
            if(starData.position != null) {
                FlyingStar star = starPool.SpawnObject(starData.position.position).GetComponent<FlyingStar>();
                star.SetSignAndTarget(starData.positive, starData.position);
            }
            yield return new WaitForLaundromatSeconds(0.1f);
        }

        spawnStarCoroutine = null;
    }

    private void OnGUI() {
        fillImage.fillAmount = (float) displayedAmount / MaxAmount;
    }

    public void SetCurrentAmount(int amount) {
        CurrentAmount = amount;
        displayedAmount = amount;
    }
}
