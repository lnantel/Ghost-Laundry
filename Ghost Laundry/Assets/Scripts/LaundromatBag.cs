using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaundromatBag : MonoBehaviour
{
    public List<Garment> contents;

    public int customerID;
    public int totalGarments;
    public int launderedGarments;
    public int perfectGarments;
    public int ruinedGarments;

    public bool ReadyForPickUp;
}
