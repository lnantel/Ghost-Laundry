using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaundromatBag : MonoBehaviour
{
    public List<Garment> contents;

    public int customerID;
    public int totalGarments;
    public int launderedGarments;
    public int perfectGarments;
    public int perfectIronedGarments;
    public int ruinedGarments;

    public bool ReadyForPickUp;

    public GameObject PopUp;
    public TextMeshPro TXT_NotLaundered;
    public TextMeshPro TXT_Laundered;
    public TextMeshPro TXT_PerfectlyLaundered;
    public TextMeshPro TXT_Ruined;

    private void Start() {
        TXT_NotLaundered.text = (totalGarments - launderedGarments - ruinedGarments).ToString();
        TXT_Laundered.text = (launderedGarments - perfectGarments).ToString();
        TXT_PerfectlyLaundered.text = (perfectGarments).ToString();
        TXT_Ruined.text = (ruinedGarments).ToString();
    }

    private void Update() {
        if(ReadyForPickUp && !PopUp.activeSelf) {
            PopUp.SetActive(true);
        }
    }
}
