using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTileAnimator : MonoBehaviour
{
    public GameObject GreyedOut;
    public GameObject Highlighted;

    public void Flip() {
        GreyedOut.SetActive(!GreyedOut.activeSelf);
        Highlighted.SetActive(!Highlighted.activeSelf);
    }
}
